using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JWT.DataAccess.Models
{
    public class RoleEntity : IdentityRole
    {
        public ICollection<UserRoleEntity> UserRoles { set; get; }
    }
}
