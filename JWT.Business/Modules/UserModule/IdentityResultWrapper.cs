using System.Collections.Generic;

namespace JWT.Business.Modules.UserModule
{
    public class IdentityResultWrapper : Response
    {
        public IEnumerable<Error> Errors { set; get; }
    }
}
