﻿using System;
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
                .Include(i => i.InsuranceCompanyContactFavorite)
                .Include(i => i.Payment)
                .Include(i => i.Parish)
                .Include(i => i.County)
                .Include(i=>i.District)
                .Include(i => i.Parish1)
                .Include(i => i.County1)
                .Include(i => i.District1)
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
                    .Include(i=>i.InsuranceCompanyContactFavorite)
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
                item.Address = insuranceCompanyContact.Address;
                item.PostalCode = insuranceCompanyContact.PostalCode;
                item.ID_District = insuranceCompanyContact.ID_District;
                item.ID_County = insuranceCompanyContact.ID_County;
                item.ID_Parish = insuranceCompanyContact.ID_Parish;
                item.SameInformationForInvoice = insuranceCompanyContact.SameInformationForInvoice;
                item.Address = insuranceCompanyContact.Address;
                item.PostalCode = insuranceCompanyContact.PostalCode;
                item.Invoice_ID_District = insuranceCompanyContact.Invoice_ID_District;
                item.Invoice_ID_County = insuranceCompanyContact.Invoice_ID_County;
                item.Invoice_ID_Parish = insuranceCompanyContact.Invoice_ID_Parish;
                item.ID_Service = insuranceCompanyContact.ID_Service;
                item.MobilePhone_1 = insuranceCompanyContact.MobilePhone_1;
                item.MobilePhone_2 = insuranceCompanyContact.MobilePhone_2;
                item.Fax = insuranceCompanyContact.Fax;
                item.Name = insuranceCompanyContact.Name;
                item.BusinessName = insuranceCompanyContact.BusinessName;
                item.NIF = insuranceCompanyContact.NIF;
                item.OfficialAgent = insuranceCompanyContact.OfficialAgent;
                item.OfficialPartner = insuranceCompanyContact.OfficialPartner;
                item.Telephone_1 = insuranceCompanyContact.Telephone_1;
                item.Telephone_2 = insuranceCompanyContact.Telephone_2;
                item.Website = insuranceCompanyContact.Website;
                item.LogoPhoto = null != insuranceCompanyContact.LogoPhoto ? insuranceCompanyContact.LogoPhoto : item.LogoPhoto;
                item.LibaxEntityID = insuranceCompanyContact.LibaxEntityID;

                if (null == item.Payment && null != insuranceCompanyContact.Payment)
                {
                    item.Payment = insuranceCompanyContact.Payment;
                }
                else if (null != item.Payment && null != insuranceCompanyContact.Payment && item.Payment.Count != insuranceCompanyContact.Payment.Count)
                {
                    item.Payment.Add(insuranceCompanyContact.Payment.Last());
                }

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
