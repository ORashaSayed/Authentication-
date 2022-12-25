using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JWT.API.Modules.Users.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Region { get; set; }

        public IEnumerable<string> Roles { set; get; }
    }
}
