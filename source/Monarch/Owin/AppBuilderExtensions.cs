using Monarch.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseClaimsAuthentication(this IAppBuilder app, ClaimsAuthenticationManager authenticationManager)
        {
            app.Use<ClaimsAuthenticationMiddleware>(authenticationManager);
            return app;
        }

        public static IAppBuilder UseClaimsAuthorization(this IAppBuilder app, ClaimsAuthorizationManager authorizationManager)
        {
            app.Use<ClaimsAuthorizationMiddleware>(authorizationManager);
            return app;
        }
    }
}