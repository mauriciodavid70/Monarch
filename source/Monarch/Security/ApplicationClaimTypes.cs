using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monarch.Security
{
    public class ApplicationClaimTypes
    {
        public const string SessionId = "http://schemas.monarch.com.co/claims/sessionid";

        public const string Action = "http://schemas.monarch.com.co/claims/authorization/action";
        public const string Resource = "http://schemas.monarch.com.co/claims/authorization/resource";

        public const string SecretNumber = "http://schemas.monarch.com.co/claims/secretnumber";
        public const string LastVisitedUrl = "http://schemas.monarch.com.co/claims/lastvisitedUrl";

    }
}