using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class PostViewModel : ProfileViewModel
    {
        public PostViewModel() { }

        public PostDTO Post { get; set; }

        public bool RemoveImage { get; set; }
        public bool RemoveFile { get; set; }
        public bool RemoveURL { get; set; }
    }

    public class NewPostViewModel
    {
        public NewPostViewModel() {
            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectDistrict } };
            this.DistrictList = initList.Concat(InsuranceBusiness.BusinessLayer.GetDistricts().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            this.CountyList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } };
        }

        public PostDTO Post { get; set; }

        [Required]
        public long? ID_District { get; set; }
        [Required]
        public long? ID_County { get; set; }

        public List<SelectListItem> SubjectTypeList { get; set; }
        public List<SelectListItem> DistrictList { get; set; }
        public List<SelectListItem> CountyList { get; set; }
    }
}
