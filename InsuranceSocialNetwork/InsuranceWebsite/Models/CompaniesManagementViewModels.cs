using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class CompaniesManagementViewModel : ProfileViewModel
    {
        public CompaniesManagementViewModel()
        {
        }
        
        public List<CompanyModelObject> Companies { get; set; }

        public CompanyModelObject newCompany { get; set; }

        public CompanyTypeEnum CompanyType { get; set; }

    }

    public class CompanyModelObject
    {
        public CompanyModelObject()
        {
            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectDistrict } };
            this.DistrictList = initList.Concat(InsuranceBusiness.BusinessLayer.GetDistricts().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.CountyList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } };
        }

        public CompanyTypeEnum CompanyType { get; set; }

        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string NIF { get; set; }
        public string BusinessName { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        [Required]
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        //public long? ID_PostalCode { get; set; }
        public long? ID_District { get; set; }
        public long? ID_County { get; set; }
        public long? ID_Parish { get; set; }
        public long? ID_Service { get; set; }
        public byte[] LogoPhoto { get; set; }
        [Url]
        public string Website { get; set; }
        public string OfficialPartner { get; set; }
        public string OfficialAgent { get; set; }

        public bool CreatePayment { get; set; }
        public long ID_PaymentType { get; set; }
        public string IBAN { get; set; }
        public decimal Value { get; set; }

        public bool Active { get; set; }

        public List<SelectListItem> DistrictList { get; set; }

        public List<SelectListItem> CountyList { get; set; }

        public List<SelectListItem> ParishList { get; set; }

        public List<SelectListItem> ServiceList { get; set; }

        public List<SelectListItem> PaymentTypeList { get; set; }

        public List<PaymentDTO> Payments { get; set; }

        public bool HasPendingPayment
        {
            get { return (null == Payments || Payments.Count == 0) ? false : Payments.Exists(i => i.ID_PaymentStatus == (int)PaymentStatusEnum.PENDING); }
        }
    }
}
