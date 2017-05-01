using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceBackoffice.Models
{
    public class UsersViewModel : ProfileViewModel
    {
        public List<UserProfileDTO> Users { get; set; }
    }
}
