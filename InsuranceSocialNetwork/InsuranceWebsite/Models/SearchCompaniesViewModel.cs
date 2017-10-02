using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class SearchCompaniesViewModel
    {
        public SearchCompaniesViewModel()
        {
            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectDistrict } };
            this.DistrictList = initList.Concat(InsuranceBusiness.BusinessLayer.GetDistricts().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.CountyList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } };
        }

        public List<SelectListItem> DistrictList { get; set; }

        public List<SelectListItem> CountyList { get; set; }

        public List<SelectListItem> ParishList { get; set; }

        #region Search Fields

        public List<CompanyDTO> ResultCompanyItems { get; set; }

        #region Garages Search

        public string GarageName { get; set; }
        public string GarageDistrict { get; set; }
        public string GarageCounty { get; set; }
        public string GarageService { get; set; }
        public string GaragePartnership { get; set; }
        public string GarageOfficialAgent { get; set; }

        #endregion Garages Search

        #region Clinics Search

        public string ClinicName { get; set; }
        public string ClinicDistrict { get; set; }
        public string ClinicCounty { get; set; }
        public string ClinicService { get; set; }
        public string ClinicPartnership { get; set; }
        public string ClinicOfficialAgent { get; set; }

        #endregion Clinics Search

        #region ConstructionCompany Search

        public string ConstructionCompanyName { get; set; }
        public string ConstructionCompanyDistrict { get; set; }
        public string ConstructionCompanyCounty { get; set; }
        public string ConstructionCompanyService { get; set; }
        public string ConstructionCompanyPartnership { get; set; }
        public string ConstructionCompanyOfficialAgent { get; set; }

        #endregion ConstructionCompany Search

        #region HomeApplianceRepair Search

        public string HomeApplianceRepairName { get; set; }
        public string HomeApplianceRepairDistrict { get; set; }
        public string HomeApplianceRepairCounty { get; set; }
        public string HomeApplianceRepairService { get; set; }
        public string HomeApplianceRepairPartnership { get; set; }
        public string HomeApplianceRepairOfficialAgent { get; set; }

        #endregion HomeApplianceRepair Search

        #region InsuranceContact Search

        public string InsuranceContactName { get; set; }
        public string InsuranceContactDistrict { get; set; }
        public string InsuranceContactCounty { get; set; }
        public string InsuranceContactService { get; set; }
        public string InsuranceContactPartnership { get; set; }
        public string InsuranceContactOfficialAgent { get; set; }

        #endregion InsuranceContact Search

        #endregion Search Fields

    }
}
