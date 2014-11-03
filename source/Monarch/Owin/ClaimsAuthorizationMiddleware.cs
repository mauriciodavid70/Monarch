using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Monarch.Owin
{
    public class ClaimsAuthorizationMiddleware : OwinMiddleware
    {
        private readonly ClaimsAuthorizationManager authorizationManager;

        public ClaimsAuthorizationMiddleware(OwinMiddleware next, ClaimsAuthorizationManager authorizationManager)
            : base(next)
        {
            this.authorizationManager = authorizationManager;
        }

        public override Task Invoke(IOwinContext context)
        {
            context.SetAuthorizationManager(this.authorizationManager);
            return Next.Invoke(context);
        }
    }
}