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
                .Include(i => i.MedicalClinicFavorite)
                .Include(i => i.Payment)
                .Include(i => i.Parish)
                .Include(i => i.County)
                .Include(i => i.District)
                .Include(i => i.Parish1)
                .Include(i => i.County1)
                .Include(i => i.District1)
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
                    .Include(i=>i.MedicalClinicFavorite)
                    .Include(i => i.Payment)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
                    .Include(i => i.Parish1)
                    .Include(i => i.County1)
                    .Include(i => i.District1)
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
                item.Address = medicalClinic.Address;
                item.PostalCode = medicalClinic.PostalCode;
                item.ID_District = medicalClinic.ID_District;
                item.ID_County = medicalClinic.ID_County;
                item.ID_Parish = medicalClinic.ID_Parish;
                item.SameInformationForInvoice = medicalClinic.SameInformationForInvoice;
                item.Address = medicalClinic.Address;
                item.PostalCode = medicalClinic.PostalCode;
                item.Invoice_ID_District = medicalClinic.Invoice_ID_District;
                item.Invoice_ID_County = medicalClinic.Invoice_ID_County;
                item.Invoice_ID_Parish = medicalClinic.Invoice_ID_Parish;
                item.ID_Service = medicalClinic.ID_Service;
                item.MobilePhone_1 = medicalClinic.MobilePhone_1;
                item.MobilePhone_2 = medicalClinic.MobilePhone_2;
                item.Fax = medicalClinic.Fax;
                item.Name = medicalClinic.Name;
                item.NIF = medicalClinic.NIF;
                item.OfficialAgent = medicalClinic.OfficialAgent;
                item.OfficialPartner = medicalClinic.OfficialPartner;
                item.Telephone_1 = medicalClinic.Telephone_1;
                item.Telephone_2 = medicalClinic.Telephone_2;
                item.Website = medicalClinic.Website;
                item.LogoPhoto = null != medicalClinic.LogoPhoto ? medicalClinic.LogoPhoto : item.LogoPhoto;
                item.LibaxEntityID = medicalClinic.LibaxEntityID;

                if (null == item.Payment && null != medicalClinic.Payment)
                {
                    item.Payment = medicalClinic.Payment;
                }
                else if (null != item.Payment && null != medicalClinic.Payment && item.Payment.Count != medicalClinic.Payment.Count)
                {
                    item.Payment.Add(medicalClinic.Payment.Last());
                }

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
