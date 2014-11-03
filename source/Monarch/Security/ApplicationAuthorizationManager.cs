using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Monarch.Security
{
    public class ApplicationAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            Trace.WriteLine("ApplicationAuthorizationManager::CheckAccess()");

            Trace.WriteLine("\nResources:");
            foreach (var claim in context.Resource)
            {
                Trace.WriteLine(string.Format("\t{0}:{1}", claim.Type, claim.Value));
            }
            Trace.WriteLine("\nActions:");
            foreach (var claim in context.Action)
            {
                Trace.WriteLine(string.Format("\t{0}:{1}", claim.Type, claim.Value));
            }
            Trace.WriteLine("\nClaims:");
            foreach (var claim in context.Principal.Claims)
            {
                Trace.WriteLine(string.Format("\t{0}:{1}", claim.Type, claim.Value));
            }

            return true;
        }
    }
}