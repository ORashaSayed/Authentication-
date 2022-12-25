using System.Collections.Generic;

namespace JWT.Business.Modules.UserModule.Responses
{
    public class GetRolesResponse : Response
    {

        public IEnumerable<string> Roles { get; set; }
    }
}
