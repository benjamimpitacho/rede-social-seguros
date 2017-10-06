using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceWebsite.Models
{
    public class NotificationItemsViewModel
    {
        public UserProfileDTO Profile { get; set; }

        public List<NotificationDTO> Items { get; set; }

        public List<ListItemString> UsersNames { get; set; }
        public List<ListItemObject> UsersPhotos { get; set; }
    }
}