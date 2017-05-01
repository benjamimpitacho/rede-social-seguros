using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class MedicalClinicRepository
    {
        public static List<MedicalClinic> GetMedicalClinics()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .MedicalClinic
                    .Fetch()
                    .Select(i => i)
                    .OrderBy(i => i.Name)
                    .ToList();
            }
        }
    }
}
