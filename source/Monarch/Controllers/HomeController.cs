using Monarch.Models;
using Monarch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;

namespace Monarch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.UpdateLastVisitedUrl();

            return View();
        }

        public ActionResult About()
        {
            this.UpdateLastVisitedUrl();
    
            ViewBag.Message = "Your application description page.";

            return View();
        }



        public ActionResult Contact()
        {
            this.UpdateLastVisitedUrl();

            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ApplicationAuthorize]
        public ActionResult Secure()
        {
            ViewBag.Message = "Your secure page.";

            var vm = GetIdentityViewModel();

            return View("Identity", vm);
        }

        private IdentityViewModel GetIdentityViewModel()
        {
            var vm = new IdentityViewModel
            {
                PrincipalType = this.User.GetType().Name,
                IdentityType = this.User.Identity.GetType().Name
            };

            var principal = this.User as ClaimsPrincipal;

            foreach (var claim in principal.Claims)
            {
                vm.Claims.Add(new ClaimViewModel { Type = claim.Type, Value = claim.Value });
            }
            return vm;
        }

        [ApplicationAuthorize("Guest", true, "Home")]
        public ActionResult Guest()
        {

            ViewBag.Message = "Your guest page.";
            var vm = GetIdentityViewModel();

            return View("Identity", vm);
        }

        private void UpdateLastVisitedUrl()
        {
            //var identity = this.User.Identity as ClaimsIdentity;
            var identity = ((ClaimsIdentity)User.Identity);

            var sessionId = identity.GetSessionId();
            var cachedIdentity = this.Request.GetOwinContext().GetClaimsIdentityCache()[sessionId];

            identity.AddClaim(new Claim(ApplicationClaimTypes.LastVisitedUrl, this.Request.Url.ToString()));

        }
    }
}