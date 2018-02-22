using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class PostViewModel : ProfileViewModel
    {
        public PostViewModel() { }

        public PostDTO Post { get; set; }

        public bool RemoveImage { get; set; }
        public bool RemoveFile { get; set; }
    }
}
