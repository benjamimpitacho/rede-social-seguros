using InsuranceBackoffice.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceSocialNetworkDTO.Garage;
using InsuranceSocialNetworkDTO.MedicalClinic;

namespace InsuranceBackoffice.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region Roles
        
        public ActionResult Roles()
        {
            RolesViewModel model = new RolesViewModel();

            model.Roles = GetRoles().ToList();

            return View(model);
        }

        public ActionResult Roles_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetRoles().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private static IEnumerable<RoleDTO> GetRoles()
        {
            return InsuranceBusiness.BusinessLayer.GetRoles();
        }

        #endregion

        #region Users

        public ActionResult Users()
        {
            UsersViewModel model = new UsersViewModel();

            model.Users = GetUsers().ToList();

            return View(model);
        }

        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetUsers().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private static IEnumerable<UserProfileDTO> GetUsers()
        {
            return InsuranceBusiness.BusinessLayer.GetUsers();
        }

        #endregion Users

        #region Garages

        public ActionResult Garages()
        {
            GaragesViewModel model = new GaragesViewModel();

            model.Garages = GetGarages().ToList();

            return View(model);
        }

        public ActionResult Garages_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetGarages().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private static IEnumerable<GarageDTO> GetGarages()
        {
            return InsuranceBusiness.BusinessLayer.GetGarages();
        }

        #endregion Garages

        #region Medical Clinics

        public ActionResult MedicalClinics()
        {
            MedicalClinicsViewModel model = new MedicalClinicsViewModel();

            model.MedicalClinics = GetMedicalClinics().ToList();

            return View(model);
        }

        public ActionResult MedicalClinics_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetMedicalClinics().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private static IEnumerable<MedicalClinicDTO> GetMedicalClinics()
        {
            return InsuranceBusiness.BusinessLayer.GetMedicalClinics();
        }

        #endregion Medical Clinics

    }
}
