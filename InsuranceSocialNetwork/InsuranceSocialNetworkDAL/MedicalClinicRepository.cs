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
        public static MedicalClinic GetMedicalClinic(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetMedicalClinic(context, id);
            }
        }

        public static MedicalClinic GetMedicalClinic(BackofficeUnitOfWork context, long id)
        {
            return context.MedicalClinic
                .Fetch()
                .Where(i => i.Active)
                .FirstOrDefault(i => i.ID == id);
        }

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

        public static long CreateMedicalClinic(MedicalClinic medicalClinic)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                medicalClinic.Active = true;
                medicalClinic.CreateDate = medicalClinic.LastChangeDate = DateTime.Now;

                context.MedicalClinic.Create(medicalClinic);
                context.Save();

                return medicalClinic.ID;
            }
        }

        public static bool EditMedicalClinic(MedicalClinic medicalClinic)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                MedicalClinic item = context.MedicalClinic.Get(medicalClinic.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = medicalClinic.Address;
                item.ContactEmail = medicalClinic.ContactEmail;
                item.Description = medicalClinic.Description;
                item.ID_District = medicalClinic.ID_District;
                item.ID_County = medicalClinic.ID_County;
                item.ID_Parish = medicalClinic.ID_Parish;
                item.ID_Service = medicalClinic.ID_Service;
                item.MobilePhone_1 = medicalClinic.MobilePhone_1;
                item.MobilePhone_2 = medicalClinic.MobilePhone_2;
                item.Name = medicalClinic.Name;
                item.NIF = medicalClinic.NIF;
                item.OfficialAgent = medicalClinic.OfficialAgent;
                item.OfficialPartner = medicalClinic.OfficialPartner;
                item.Telephone_1 = medicalClinic.Telephone_1;
                item.Telephone_2 = medicalClinic.Telephone_2;
                item.Website = medicalClinic.Website;
                item.LogoPhoto = null != medicalClinic.LogoPhoto ? medicalClinic.LogoPhoto : item.LogoPhoto;

                context.MedicalClinic.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteMedicalClinic(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                MedicalClinic item = context.MedicalClinic.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;
                item.LastChangeDate = DateTime.Now;

                context.MedicalClinic.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool ActivateMedicalClinic(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                MedicalClinic medicalClinic = context.MedicalClinic.Get(id);

                medicalClinic.Active = true;
                medicalClinic.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }

        public static bool DeactivateMedicalClinic(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                MedicalClinic medicalClinic = context.MedicalClinic.Get(id);

                medicalClinic.Active = false;
                medicalClinic.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }
    }
}
