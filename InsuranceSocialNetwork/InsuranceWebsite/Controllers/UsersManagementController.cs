using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class UsersManagementController : Controller
    {
        public static UserProfileDTO CurrentUser;

        public UsersManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Index(UsersManagementViewModel model)
        {
            model = new UsersManagementViewModel();

            return View(model);
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();

            return View(model);
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public JsonResult Get()
        {
            //ClientFilter filter = GetFilter();

            return Json(InsuranceBusiness.BusinessLayer.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}