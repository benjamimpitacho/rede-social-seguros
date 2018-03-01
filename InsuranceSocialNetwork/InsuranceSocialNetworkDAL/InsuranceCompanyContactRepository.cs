using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class InsuranceCompanyContactRepository
    {
        public static InsuranceCompanyContact GetInsuranceCompanyContact(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetInsuranceCompanyContact(context, id);
            }
        }

        public static InsuranceCompanyContact GetInsuranceCompanyContact(BackofficeUnitOfWork context, long id)
        {
            return context.InsuranceCompanyContact
                .Fetch()
                .Include("InsuranceCompanyContactFavorite")
                .Where(i => i.Active)
                .FirstOrDefault(i => i.ID == id);
        }

        public static List<InsuranceCompanyContact> GetInsuranceCompanyContacts()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .InsuranceCompanyContact
                    .Fetch()
                    .Include("InsuranceCompanyContactFavorite")
                    .Select(i => i)
                    .OrderBy(i => i.Name)
                    .ToList();
            }
        }

        public static long CreateInsuranceCompanyContact(InsuranceCompanyContact insuranceCompanyContact)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                insuranceCompanyContact.Active = true;
                insuranceCompanyContact.CreateDate = insuranceCompanyContact.LastChangeDate = DateTime.Now;

                context.InsuranceCompanyContact.Create(insuranceCompanyContact);
                context.Save();

                return insuranceCompanyContact.ID;
            }
        }

        public static bool EditInsuranceCompanyContact(InsuranceCompanyContact insuranceCompanyContact)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                InsuranceCompanyContact item = context.InsuranceCompanyContact.Get(insuranceCompanyContact.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = insuranceCompanyContact.Address;
                item.ContactEmail = insuranceCompanyContact.ContactEmail;
                item.Description = insuranceCompanyContact.Description;
                item.ID_District = insuranceCompanyContact.ID_District;
                item.ID_County = insuranceCompanyContact.ID_County;
                item.ID_Parish = insuranceCompanyContact.ID_Parish;
                item.ID_Service = insuranceCompanyContact.ID_Service;
                item.MobilePhone_1 = insuranceCompanyContact.MobilePhone_1;
                item.MobilePhone_2 = insuranceCompanyContact.MobilePhone_2;
                item.Name = insuranceCompanyContact.Name;
                item.NIF = insuranceCompanyContact.NIF;
                item.OfficialAgent = insuranceCompanyContact.OfficialAgent;
                item.OfficialPartner = insuranceCompanyContact.OfficialPartner;
                item.Telephone_1 = insuranceCompanyContact.Telephone_1;
                item.Telephone_2 = insuranceCompanyContact.Telephone_2;
                item.Website = insuranceCompanyContact.Website;
                item.LogoPhoto = null != insuranceCompanyContact.LogoPhoto ? insuranceCompanyContact.LogoPhoto : item.LogoPhoto;

                context.InsuranceCompanyContact.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteInsuranceCompanyContact(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                InsuranceCompanyContact item = context.InsuranceCompanyContact.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;
                item.LastChangeDate = DateTime.Now;

                context.InsuranceCompanyContact.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool ActivateInsuranceCompanyContact(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                InsuranceCompanyContact insuranceCompanyContact = context.InsuranceCompanyContact.Get(id);

                insuranceCompanyContact.Active = true;
                insuranceCompanyContact.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }

        public static bool DeactivateInsuranceCompanyContact(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                InsuranceCompanyContact insuranceCompanyContact = context.InsuranceCompanyContact.Get(id);

                insuranceCompanyContact.Active = false;
                insuranceCompanyContact.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }
    }
}
