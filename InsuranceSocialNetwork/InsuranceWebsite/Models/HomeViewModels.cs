using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class HomeViewModel : ProfileViewModel
    {
        public HomeViewModel()
        {
            this.IsPostsView = true;
            this.IsMessagesView = false;
            this.IsNotificationsView = false;
        }
        public bool IsPostsView { get; set; }
        public bool IsMessagesView { get; set; }
        public bool IsNotificationsView { get; set; }

        public List<PostDTO> Posts { get; set; }

    }
}
