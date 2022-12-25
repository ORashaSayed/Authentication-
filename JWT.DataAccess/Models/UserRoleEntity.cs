using Microsoft.AspNetCore.Identity;

namespace JWT.DataAccess.Models
{
    public class UserRoleEntity : IdentityUserRole<string>
    {
        public virtual RoleEntity Role { set; get; }
        public virtual UserEntity User { set; get; }
    }
}
