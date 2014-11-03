using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;

namespace Monarch.Security
{
    public class ApplicationAuthorizeAttribute : AuthorizeAttribute
    {
        private string action;
        private string[] resources;
        private bool allowAnonymous;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationAuthorizeAttribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor with action and resources
        /// </summary>
        /// <param name="action"></param>
        /// <param name="resources"></param>
        public ApplicationAuthorizeAttribute(string action, bool allowAnonymous = false, params string[] resources)
            : base()
        {
            this.action = action;
            this.resources = resources;
            this.allowAnonymous = allowAnonymous;
        }

        /// <summary>
        /// Authorize method - Declarative
        /// </summary>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (this.allowAnonymous || base.AuthorizeCore(httpContext))
            {
                return httpContext.GetOwinContext().CheckAccess(this.resources, this.action);
            }
            return false;
        }

        /// <summary>
        /// Authorize method - imperative
        /// </summary>
        protected virtual bool CheckAccess(HttpContextBase httpContext, string action,  bool allowAnonymous = false, params string[] resources)
        {
            this.action = action;
            this.resources = resources;
            this.allowAnonymous = allowAnonymous;

            return this.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// OnAuthorize method triggered everytime a process requires authorization
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(action))
            {
                this.action = filterContext.RouteData.Values["action"] as string;
                this.resources = new string[] { filterContext.RouteData.Values["controller"] as string };
            }
            base.OnAuthorization(filterContext);

        }




    }
}