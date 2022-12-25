using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JWT.DataAccess.Models
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { set; get; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Region { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { set; get; }
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
