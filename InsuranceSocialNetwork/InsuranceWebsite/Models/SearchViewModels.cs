using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class SearchViewModel : ProfileViewModel
    {
        public SearchViewModel()
        {
        }

        [Required]
        public string SearchTerm { get; set; }

        public List<UserProfileDTO> Users { get; set; }
        public List<long> AlreadyFriends { get; set; }

    }
}
