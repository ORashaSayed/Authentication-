using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JWT.Model.Models
{
    public class AppUser : IdentityUser
    {
        public string Region { get; set; }

        public ICollection<RefreshTokenEntity> RefreshTokens { set; get; }
    }
}
