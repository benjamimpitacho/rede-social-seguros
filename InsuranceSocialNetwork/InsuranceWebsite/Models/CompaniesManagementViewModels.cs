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

    }

    public class CompanyModelObject
    {
        public CompanyModelObject()
        {
            //_bannerTypes = InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.GetBannerTypes().Select(i => new ListItem() { Id = i.ID, Name = Resources.Resources.ResourceManager.GetString(i.Description) }).ToList();
        }

        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string NIF { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        public long? ID_PostalCode { get; set; }
        public byte[] LogoPhoto { get; set; }
        [Url]
        public string Website { get; set; }
        public string OficialPartner { get; set; }
        public string OficialAgent { get; set; }
        public bool Active { get; set; }
        
        //private List<ListItem> _bannerTypes;
        //public IEnumerable<SelectListItem> BannerTypesList
        //{
        //    get { return new SelectList(_bannerTypes, "Id", "Name"); }
        //}
    }
}
