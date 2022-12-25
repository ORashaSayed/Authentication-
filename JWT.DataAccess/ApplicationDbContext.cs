using JWT.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace JWT.DataAccess
{
    internal class ApplicationDbContext
        : IdentityDbContext<UserEntity, RoleEntity, string, IdentityUserClaim<string>,
            UserRoleEntity, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public DbSet<RefreshTokenEntity> RefreshTokens { set; get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the built-in model configurations first so our hand-made configurations override what is already built-in
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        
    }
}
