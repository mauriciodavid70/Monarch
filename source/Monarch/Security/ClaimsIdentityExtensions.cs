using Monarch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace System.Security.Claims
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetSessionId(this ClaimsIdentity identity)
        {
            var sessionIdClaim = identity.Claims.Where(c => c.Type == ApplicationClaimTypes.SessionId).SingleOrDefault();
            if (sessionIdClaim != null)
            {
                return sessionIdClaim.Value;
            }
            return null;

        }
    }
}