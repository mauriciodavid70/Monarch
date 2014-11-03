using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Monarch.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Monarch.Security;
using System.Runtime.Caching;
using System.Linq;

namespace Monarch
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        
            // Use cookie based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new ApplicationCookieAuthenticationProvider()
            });

            // Use cookie based authentication with external identitites
            app.UseExternalSignInCookie();

            // Authenticate users with Google OAuth2 
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "516397871686-dav9fml8553j46uljqcplfhd7jlc5dt2.apps.googleusercontent.com",
                ClientSecret = "HNW7MVocX0vlzPboiPrYjtqr"
            });

            // Configures Identity with application specific claims
            app.UseClaimsAuthentication(new ApplicationAuthenticationManager());

            // Configure Claims based authorization
            app.UseClaimsAuthorization(new ApplicationAuthorizationManager());
        }





    }
}