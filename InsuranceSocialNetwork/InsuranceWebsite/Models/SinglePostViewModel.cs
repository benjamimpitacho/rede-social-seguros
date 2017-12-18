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
    public class SinglePostViewModel : ProfileViewModel
    {
        public PostDTO Item { get; set; }
    }
}
