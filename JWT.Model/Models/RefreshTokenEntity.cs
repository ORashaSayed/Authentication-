using System;
using JWT.Model.Contracts;

namespace JWT.Model.Models
{
    public class RefreshTokenEntity : ICreatedAt
    {
        public Guid Id { set; get; }
        public string Token { set; get; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
        public Guid Jti { get; set; }
        public virtual AppUser User { set; get; }
        public DateTime CreatedAt { get; set; }
    }
}
