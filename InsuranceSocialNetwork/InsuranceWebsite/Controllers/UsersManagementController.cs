using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using InsuranceWebsite.Utils;
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
        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserProfileDTO, UserProfileModelObject>();
            cfg.CreateMap<UserProfileModelObject, UserProfileDTO>();

            cfg.CreateMap<UserDTO, UserModelObject>();
            cfg.CreateMap<UserModelObject, UserDTO>();
        });

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
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}