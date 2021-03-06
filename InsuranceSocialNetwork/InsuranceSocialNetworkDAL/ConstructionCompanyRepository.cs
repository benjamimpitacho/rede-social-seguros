﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class ConstructionCompanyRepository
    {
        public static ConstructionCompany GetConstructionCompany(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetConstructionCompany(context, id);
            }
        }

        public static ConstructionCompany GetConstructionCompany(BackofficeUnitOfWork context, long id)
        {
            return context.ConstructionCompany
                .Fetch()
                .Include(i=>i.ConstructionCompanyFavorite)
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

        public static List<ConstructionCompany> GetConstructionCompanies()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .ConstructionCompany
                    .Fetch()
                    .Include(i => i.ConstructionCompanyFavorite)
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

        public static long CreateConstructionCompany(ConstructionCompany constructionCompany)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                constructionCompany.Active = true;
                constructionCompany.CreateDate = constructionCompany.LastChangeDate = DateTime.Now;

                context.ConstructionCompany.Create(constructionCompany);
                context.Save();

                return constructionCompany.ID;
            }
        }

        public static bool EditConstructionCompany(ConstructionCompany constructionCompany)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ConstructionCompany item = context.ConstructionCompany.Get(constructionCompany.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = constructionCompany.Address;
                item.ContactEmail = constructionCompany.ContactEmail;
                item.Description = constructionCompany.Description;
                item.Address = constructionCompany.Address;
                item.PostalCode = constructionCompany.PostalCode;
                item.ID_District = constructionCompany.ID_District;
                item.ID_County = constructionCompany.ID_County;
                item.ID_Parish = constructionCompany.ID_Parish;
                item.SameInformationForInvoice = constructionCompany.SameInformationForInvoice;
                item.Address = constructionCompany.Address;
                item.PostalCode = constructionCompany.PostalCode;
                item.Invoice_ID_District = constructionCompany.Invoice_ID_District;
                item.Invoice_ID_County = constructionCompany.Invoice_ID_County;
                item.Invoice_ID_Parish = constructionCompany.Invoice_ID_Parish;
                item.ID_Service = constructionCompany.ID_Service;
                item.MobilePhone_1 = constructionCompany.MobilePhone_1;
                item.MobilePhone_2 = constructionCompany.MobilePhone_2;
                item.Fax = constructionCompany.Fax;
                item.Name = constructionCompany.Name;
                item.BusinessName = constructionCompany.BusinessName;
                item.NIF = constructionCompany.NIF;
                item.OfficialAgent = constructionCompany.OfficialAgent;
                item.OfficialPartner = constructionCompany.OfficialPartner;
                item.Telephone_1 = constructionCompany.Telephone_1;
                item.Telephone_2 = constructionCompany.Telephone_2;
                item.Website = constructionCompany.Website;
                item.LogoPhoto = null != constructionCompany.LogoPhoto ? constructionCompany.LogoPhoto : item.LogoPhoto;
                item.LibaxEntityID = constructionCompany.LibaxEntityID;

                if (null == item.Payment && null != constructionCompany.Payment)
                {
                    item.Payment = constructionCompany.Payment;
                }
                else if (null != item.Payment && null != constructionCompany.Payment && item.Payment.Count != constructionCompany.Payment.Count)
                {
                    item.Payment.Add(constructionCompany.Payment.Last());
                }

                context.ConstructionCompany.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteConstructionCompany(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ConstructionCompany item = context.ConstructionCompany.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;
                item.LastChangeDate = DateTime.Now;

                context.ConstructionCompany.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool ActivateConstructionCompany(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ConstructionCompany constructionCompany = context.ConstructionCompany.Get(id);

                constructionCompany.Active = true;
                constructionCompany.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }

        public static bool DeactivateConstructionCompany(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ConstructionCompany constructionCompany = context.ConstructionCompany.Get(id);

                constructionCompany.Active = false;
                constructionCompany.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }
    }
}
