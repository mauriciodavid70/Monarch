using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;

namespace Monarch.Security
{
    public class ApplicationAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            ClaimsPrincipal appPrincipal;
            if (incomingPrincipal.Identity.IsAuthenticated)
            {
                appPrincipal = CreateAuthenticatedPrincipal(incomingPrincipal);
            }
            else
            {
                appPrincipal = CreateAnonymousPrincipal(incomingPrincipal);
            }

            return base.Authenticate(resourceName, appPrincipal);
        }

        private ClaimsPrincipal CreateAnonymousPrincipal(ClaimsPrincipal principal)
        {
            //get the current identity
            var identity = new ClaimsIdentity(principal.Identity);

            //add application specific claims
            identity.AddClaims(new Claim[]{
                    new Claim(ClaimTypes.Anonymous, ""),
                    new Claim(ApplicationClaimTypes.SecretNumber, "987654321")});

            //return the principal (with app specific claims)
            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal CreateAuthenticatedPrincipal(ClaimsPrincipal principal)
        {
            //get the current identity
            var identity = new ClaimsIdentity(principal.Identity);

            //add application specific claims
            identity.AddClaim(new Claim(ApplicationClaimTypes.SecretNumber, "123456789"));

            //return the principal (with app specific claims)
            return new ClaimsPrincipal(identity);
        }

    }
}