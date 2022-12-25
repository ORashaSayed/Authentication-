using System.ComponentModel.DataAnnotations;

namespace JWT.API.Modules.Users.ViewModels
{
    public class ExchangeTokenRequest
    {
        [Required]
        public string AccessToken { set; get; }
        [Required]
        public string RefreshToken { get; set; }
    }
}