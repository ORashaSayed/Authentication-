using System.ComponentModel.DataAnnotations;

namespace JWT.API.Modules.Authentication.ViewModels
{
    public class ExchangeTokenViewModel
    {
        [Required]
        public string AccessToken { set; get; }
        [Required]
        public string RefreshToken { get; set; }
    }
}