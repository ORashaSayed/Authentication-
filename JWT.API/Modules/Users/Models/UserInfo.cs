using System.Collections.Generic;
using System.Security.Claims;

namespace JWT.API.Modules.Users.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}