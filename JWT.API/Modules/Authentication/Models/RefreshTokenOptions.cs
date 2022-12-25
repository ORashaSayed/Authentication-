using System;

namespace JWT.API.Modules.Authentication.Models
{
    public class RefreshTokenOptions
    {
        public DateTime ExpirationTime => DateTime.UtcNow.AddSeconds(ValidTime);
        public int ValidTime { set; get; }
    }
}
