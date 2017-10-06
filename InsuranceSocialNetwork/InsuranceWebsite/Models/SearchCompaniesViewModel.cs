using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
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

            List<SelectListItem> serviceList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectService } };
            this.GarageServiceList = serviceList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(CompanyTypeEnum.GARAGE).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.ConstructionCompanyServiceList = serviceList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(CompanyTypeEnum.CONSTRUCTION_COMPANY).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.HomeApplianceRepairServiceList = serviceList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(CompanyTypeEnum.HOME_APPLIANCES_REPAIR).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.InsuranceContactServiceList = serviceList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(CompanyTypeEnum.INSURANCE_COMPANY_CONTACT).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.ClinicServiceList = serviceList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(CompanyTypeEnum.MEDICAL_CLINIC).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
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
        public long? GarageServiceID { get; set; }
        public List<SelectListItem> GarageServiceList { get; set; }
        public string GaragePartnership { get; set; }
        public string GarageOfficialAgent { get; set; }

        #endregion Garages Search

        #region Clinics Search

        public string ClinicName { get; set; }
        public string ClinicDistrict { get; set; }
        public string ClinicCounty { get; set; }
        public long? ClinicServiceID { get; set; }
        public List<SelectListItem> ClinicServiceList { get; set; }
        public string ClinicPartnership { get; set; }
        public string ClinicOfficialAgent { get; set; }

        #endregion Clinics Search

        #region ConstructionCompany Search

        public string ConstructionCompanyName { get; set; }
        public string ConstructionCompanyDistrict { get; set; }
        public string ConstructionCompanyCounty { get; set; }
        public long? ConstructionCompanyServiceID { get; set; }
        public List<SelectListItem> ConstructionCompanyServiceList { get; set; }
        public string ConstructionCompanyPartnership { get; set; }
        public string ConstructionCompanyOfficialAgent { get; set; }

        #endregion ConstructionCompany Search

        #region HomeApplianceRepair Search

        public string HomeApplianceRepairName { get; set; }
        public string HomeApplianceRepairDistrict { get; set; }
        public string HomeApplianceRepairCounty { get; set; }
        public long? HomeApplianceRepairServiceID { get; set; }
        public List<SelectListItem> HomeApplianceRepairServiceList { get; set; }
        public string HomeApplianceRepairPartnership { get; set; }
        public string HomeApplianceRepairOfficialAgent { get; set; }

        #endregion HomeApplianceRepair Search

        #region InsuranceContact Search

        public string InsuranceContactName { get; set; }
        public string InsuranceContactDistrict { get; set; }
        public string InsuranceContactCounty { get; set; }
        public long? InsuranceContactServiceID { get; set; }
        public List<SelectListItem> InsuranceContactServiceList { get; set; }
        public string InsuranceContactPartnership { get; set; }
        public string InsuranceContactOfficialAgent { get; set; }

        #endregion InsuranceContact Search

        #endregion Search Fields

    }
}
