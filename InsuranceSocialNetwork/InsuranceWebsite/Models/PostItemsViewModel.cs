using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceWebsite.Models
{
    public class PostItemsViewModel
    {
        public UserProfileDTO Profile { get; set; }

        public List<PostDTO> Items { get; set; }
    }
}