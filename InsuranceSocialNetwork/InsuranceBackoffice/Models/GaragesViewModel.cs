using InsuranceSocialNetworkDTO.Garage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceBackoffice.Models
{
    public class GaragesViewModel : ProfileViewModel
    {
        public List<GarageDTO> Garages { get; set; }
    }
}
