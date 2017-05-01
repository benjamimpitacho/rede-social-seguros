using InsuranceSocialNetworkDTO.Role;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceBackoffice.Models
{
    public class RolesViewModel : ProfileViewModel
    {
        public List<RoleDTO> Roles { get; set; }
    }
}
