using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;

namespace Monarch.Security
{
    public class ClaimsIdentityCache
    {
        private const string prefix = "cache:identity_";

        public ClaimsIdentity this[string sessionId]
        {

            get {
                var cacheKey = prefix + sessionId;
                return MemoryCache.Default[cacheKey] as ClaimsIdentity; 
            }
            set
            {
                var cacheKey = prefix + sessionId;
                MemoryCache.Default.Set(cacheKey, value, DateTime.Now.AddMinutes(20));
            }
        }
    }
}