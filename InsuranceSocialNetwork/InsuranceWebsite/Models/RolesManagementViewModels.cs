using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class RolesManagementViewModel : ProfileViewModel
    {
        public RolesManagementViewModel()
        {
        }
        
        public List<RoleModelObject> UserProfiles { get; set; }

    }

    public class RoleModelObject
    {
        public long ID { get; set; }
    }
}
