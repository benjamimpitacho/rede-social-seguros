using InsuranceSocialNetworkBusiness;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceWebsite.Commons
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FunctionalityAutorizeAttribute : AuthorizeAttribute
    {
        //Entities context = new Entities(); // my entity  
        public string _functionality;

        public FunctionalityAutorizeAttribute(string functionality)
        {
            this._functionality = functionality;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return InsuranceBusiness.BusinessLayer.IsUserAuthorizedToFunctionality(httpContext.User.Identity.Name, _functionality);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}