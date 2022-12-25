using JWT.Api.Registrars;
using JWT.API.Modules.Authentication;
using JWT.API.Modules.Authentication.Models;
using JWT.Bootstrapper;
using JWT.Caching;
using JWT.DependencyInjection.Registrars;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JWT.API.Helpers;

namespace JWT.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string lookupLocationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private static readonly string searchCriteria = "*.dll";
        private const string SecretKey = "QeShVmYq3t6w9z$C&F)J@NcRfUjWnZr4u7x!A%D*G-KaPdSgVkYp2s5v8y/B?E(H";
        public static IServiceCollection AddRegistrars(this IServiceCollection services, IConfiguration configuration)
        {
            using var loader = new ModuleLoader(lookupLocationPath, searchCriteria);
            var moduleRegistrar = new ModuleRegistrar(services);
            var databaseRegistrar = new DatabaseRegistrar(services, configuration.GetConnectionString("DefaultConnection"));
            var IdentityRegistrar = new IdentityRegistrar(services);

            loader.Load<IModuleRegistrar>(moduleRegistrar);
            loader.Load<IDatabaseRegistrar>(databaseRegistrar);
            loader.Load<IIdentityRegistrar>(IdentityRegistrar);

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            using var loader = new ModuleLoader(lookupLocationPath, searchCriteria);
            var profilesRegistrar = new AutoMapperProfilesRegistrar();

            loader.Load<IAutoMapperProfilesRegistrar>(profilesRegistrar);
            services.AddAutoMapper(profilesRegistrar.ProfileTypes.ToArray());

            return services;
        }

        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSigningSecretKey")));

            services.Configure<JwtIssuerOptions>(configuration.GetSection("JwtIssuerOptions"));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshTokenOptions"));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Don't changes token keys to default URLs keys
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            return services;
        }

        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<IMemoryCaching, MemoryCaching>();
            services.AddSingleton(new MemoryCachingOptions
            {
                AbsoluteExpirationRelativeToNow = (DateTime.Now.AddSeconds(double.Parse(configuration.GetSection("Caching:AbsoluteExpirationRelativeToNow").Value)) - DateTime.Now)
            });

            return services;
        }
    }

}
