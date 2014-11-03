using Monarch.Owin;
using Monarch.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.Owin
{
    public static class OwinContextExtensions
    {
        public static bool CheckAccess(this IOwinContext context, string[] resources, string action)
        {
            var resourceClaims = new Collection<Claim>();
            foreach (var resource in resources)
            {
                resourceClaims.Add(new Claim(ApplicationClaimTypes.Resource, resource));
            }
            var actionClaims = new Collection<Claim>()
            {
                new Claim(ApplicationClaimTypes.Action, action)
            };

            var authorizationContext = new AuthorizationContext(context.Authentication.User, resourceClaims, actionClaims);

            return context.GetAuthorizationManager().CheckAccess(authorizationContext);
        }

        public static void SetAuthorizationManager( this IOwinContext context, ClaimsAuthorizationManager authorizationManager)
        {
            context.Environment.Add(ApplicationEnvironmentKeys.ClaimsAuthorizationManager, authorizationManager);
        }

        public static ClaimsAuthorizationManager GetAuthorizationManager(this IOwinContext context)
        {
            return context.Get<ClaimsAuthorizationManager>(ApplicationEnvironmentKeys.ClaimsAuthorizationManager);
        }

        public static void SetClaimsIdentityCache(this IOwinContext context, ClaimsIdentityCache identityCache)
        {
            context.Environment.Add(ApplicationEnvironmentKeys.ClaimsIdentityCache, identityCache);
        }

        public static ClaimsIdentityCache GetClaimsIdentityCache(this IOwinContext context)
        {
            return context.Get<ClaimsIdentityCache>(ApplicationEnvironmentKeys.ClaimsIdentityCache);
        }
       
    }
}