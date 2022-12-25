using System.Collections.Generic;

namespace JWT.Business.Modules.UserModule.Responses
{
    public class UserInfoResponse : Response
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
