using Monarch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Monarch.Controllers
{
    public class IdentityController : ApiController
    {
        public IEnumerable<ClaimViewModel> Get()
        {
            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            return from c in principal.Claims
                   select new ClaimViewModel
                   {
                       Type = c.Type,
                       Value = c.Value
                   };
        }

       
    }
}
