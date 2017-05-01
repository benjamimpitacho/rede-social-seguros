using InsuranceSocialNetworkDTO.MedicalClinic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceBackoffice.Models
{
    public class MedicalClinicsViewModel : ProfileViewModel
    {
        public List<MedicalClinicDTO> MedicalClinics { get; set; }
    }
}
