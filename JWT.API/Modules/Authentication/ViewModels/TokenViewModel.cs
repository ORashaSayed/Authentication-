namespace JWT.API.Modules.Authentication.ViewModels
{
    public class TokenViewModel
    {
        public string AccessToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public int ExpiresIn { get; internal set; }
    }
}
