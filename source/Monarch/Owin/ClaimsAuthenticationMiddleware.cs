using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Monarch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Monarch.Owin
{
    public class ClaimsAuthenticationMiddleware : OwinMiddleware
    {
        private readonly ClaimsAuthenticationManager authenticationManager;
        private readonly ClaimsIdentityCache identityCache;

        public ClaimsAuthenticationMiddleware(OwinMiddleware next, ClaimsAuthenticationManager authenticationManager)
            : base(next)
        {
            this.authenticationManager = authenticationManager;
            this.identityCache = new ClaimsIdentityCache();
        }

        public override Task Invoke(IOwinContext context)
        {
            var incomingPrincipal = context.Authentication.User;

            if (incomingPrincipal != null)
            {
                ClaimsPrincipal applicationPrincipal;

                var sessionClaim = incomingPrincipal.Claims.Where(c => c.Type == ApplicationClaimTypes.SessionId).FirstOrDefault();
                if (sessionClaim != null)
                {
                    var sessionId = sessionClaim.Value;
                    var cachedIdentity = this.identityCache[sessionId];
                    if (cachedIdentity != null)
                    {
                        //set the application principal from cached identity
                        applicationPrincipal = new ClaimsPrincipal(cachedIdentity);
                    }
                    else
                    {
                        //set the application principal from authentication manager
                        applicationPrincipal = this.authenticationManager.Authenticate(context.Request.Uri.AbsoluteUri, context.Authentication.User);
                        //place the identity in the cache
                        this.identityCache[sessionId] = applicationPrincipal.Identity as ClaimsIdentity;
                    }
                }
                else
                {
                    //set the application principal from authentication manager
                    applicationPrincipal = this.authenticationManager.Authenticate(context.Request.Uri.AbsoluteUri, context.Authentication.User);
                }

                //set the context authentication user to application principal
                context.Authentication.User = applicationPrincipal;
            }

            //place identitycache in owin context
            context.SetClaimsIdentityCache(this.identityCache);

            return Next.Invoke(context);
        }

     
    }
}



