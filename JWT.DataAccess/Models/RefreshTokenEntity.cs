using System;

namespace JWT.DataAccess.Models
{
    public class RefreshTokenEntity
    {
        public Guid Id { set; get; }
        public string Token { set; get; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
        public Guid Jti { get; set; }
        public virtual UserEntity User { set; get; }
        public DateTime CreatedAt { get; set; }
    }
}
