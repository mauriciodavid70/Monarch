using Microsoft.Owin.Security.Cookies;
using System;
using System.Security.Claims;

namespace Monarch.Security
{

    public class ApplicationCookieAuthenticationProvider : CookieAuthenticationProvider
    {
        public override void ResponseSignIn(CookieResponseSignInContext context)
        {
            context.Identity.AddClaim(new Claim(ApplicationClaimTypes.SessionId, Guid.NewGuid().ToString()));
        }

        //TODO: Check why can´t logout 
        //public override async Task ValidateIdentity(CookieValidateIdentityContext context)
        //{
        //    await SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
        //                           validateInterval: TimeSpan.FromMinutes(20),
        //                           regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))(context);

        //}

    }
}