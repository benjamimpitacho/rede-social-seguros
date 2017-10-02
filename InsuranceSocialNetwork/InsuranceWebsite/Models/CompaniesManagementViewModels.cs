﻿using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
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
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        //public long? ID_PostalCode { get; set; }
        public long? ID_District { get; set; }
        public long? ID_County { get; set; }
        public long? ID_Parish { get; set; }
        public byte[] LogoPhoto { get; set; }
        [Url]
        public string Website { get; set; }
        public string OfficialPartner { get; set; }
        public string OfficialAgent { get; set; }
        public bool Active { get; set; }

        public List<SelectListItem> DistrictList { get; set; }

        public List<SelectListItem> CountyList { get; set; }

        public List<SelectListItem> ParishList { get; set; }

        //private List<ListItem> _bannerTypes;
        //public IEnumerable<SelectListItem> BannerTypesList
        //{
        //    get { return new SelectList(_bannerTypes, "Id", "Name"); }
        //}
    }
}