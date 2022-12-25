namespace JWT.Business.Modules.UserModule
{
    public static class PasswordOptions
    {
        public static readonly int RequiredLength = 7;
        public static readonly bool RequireNonAlphanumeric = true;
        public static readonly bool RequireDigit = true;
        public static readonly bool RequireLowercase = true;
        public static readonly bool RequireUppercase = true;
    }

    public static class UserOptions
    {
        public static readonly bool RequireUniqueEmail = true;
    }
}
