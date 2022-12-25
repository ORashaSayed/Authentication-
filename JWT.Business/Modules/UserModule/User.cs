using System;
using System.Collections.Generic;

namespace JWT.Business.Modules.UserModule
{
    public class User
    {
        private string id;
        public string Id
        {
            get => string.IsNullOrWhiteSpace(id) ? id = Guid.NewGuid().ToString() : id;
            set => id = value;
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { set; get; }
        public string Region { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
