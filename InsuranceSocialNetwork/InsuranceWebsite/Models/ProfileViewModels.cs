using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class ProfileViewModel
    {
        public UserProfileDTO Profile { get; set; }

        public List<NotificationDTO> Notifications { get; set; }

        public int TotalUnreadNotifications { get { return null == Notifications ? 0 : Notifications.Count(i => !i.Read); } }

    }
}
