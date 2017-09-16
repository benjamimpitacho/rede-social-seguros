using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class BannersManagementViewModel : ProfileViewModel
    {
        public BannersManagementViewModel()
        {
        }
        
        public List<BannerModelObject> Banners { get; set; }

        public BannerModelObject newBanner { get; set; }

    }

    public class BannerModelObject
    {
        public BannerModelObject()
        {
            _bannerTypes = InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.GetBannerTypes().Select(i => new ListItem() { Id = i.ID, Name = i.Description }).ToList();
        }

        public long ID { get; set; }
        [Required]
        public long ID_Banner_Type { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public bool Active { get; set; }

        private List<ListItem> _bannerTypes;
        public IEnumerable<SelectListItem> BannerTypesList
        {
            get { return new SelectList(_bannerTypes, "Id", "Name"); }
        }
    }

    public class ListItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
