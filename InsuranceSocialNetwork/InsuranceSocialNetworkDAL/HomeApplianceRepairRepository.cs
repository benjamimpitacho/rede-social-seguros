using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class HomeApplianceRepairRepository
    {
        public static HomeApplianceRepair GetHomeApplianceRepair(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetHomeApplianceRepair(context, id);
            }
        }

        public static HomeApplianceRepair GetHomeApplianceRepair(BackofficeUnitOfWork context, long id)
        {
            return context.HomeApplianceRepair
                .Fetch()
                .Include(i => i.HomeApplianceRepairFavorite)
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

        public static List<HomeApplianceRepair> GetHomeApplianceRepairs()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .HomeApplianceRepair
                    .Fetch()
                    .Include(i=>i.HomeApplianceRepairFavorite)
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

        public static long CreateHomeApplianceRepair(HomeApplianceRepair homeApplianceRepair)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                homeApplianceRepair.Active = true;
                homeApplianceRepair.CreateDate = homeApplianceRepair.LastChangeDate = DateTime.Now;

                context.HomeApplianceRepair.Create(homeApplianceRepair);
                context.Save();

                return homeApplianceRepair.ID;
            }
        }

        public static bool EditHomeApplianceRepair(HomeApplianceRepair homeApplianceRepair)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                HomeApplianceRepair item = context.HomeApplianceRepair.Get(homeApplianceRepair.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = homeApplianceRepair.Address;
                item.ContactEmail = homeApplianceRepair.ContactEmail;
                item.Description = homeApplianceRepair.Description;
                item.Address = homeApplianceRepair.Address;
                item.PostalCode = homeApplianceRepair.PostalCode;
                item.ID_District = homeApplianceRepair.ID_District;
                item.ID_County = homeApplianceRepair.ID_County;
                item.ID_Parish = homeApplianceRepair.ID_Parish;
                item.SameInformationForInvoice = homeApplianceRepair.SameInformationForInvoice;
                item.Address = homeApplianceRepair.Address;
                item.PostalCode = homeApplianceRepair.PostalCode;
                item.ID_District = homeApplianceRepair.Invoice_ID_District;
                item.ID_County = homeApplianceRepair.Invoice_ID_County;
                item.ID_Parish = homeApplianceRepair.Invoice_ID_Parish;
                item.ID_Service = homeApplianceRepair.ID_Service;
                item.MobilePhone_1 = homeApplianceRepair.MobilePhone_1;
                item.MobilePhone_2 = homeApplianceRepair.MobilePhone_2;
                item.Fax = homeApplianceRepair.Fax;
                item.Name = homeApplianceRepair.Name;
                item.NIF = homeApplianceRepair.NIF;
                item.OfficialAgent = homeApplianceRepair.OfficialAgent;
                item.OfficialPartner = homeApplianceRepair.OfficialPartner;
                item.Telephone_1 = homeApplianceRepair.Telephone_1;
                item.Telephone_2 = homeApplianceRepair.Telephone_2;
                item.Website = homeApplianceRepair.Website;
                item.LogoPhoto = null != homeApplianceRepair.LogoPhoto ? homeApplianceRepair.LogoPhoto : item.LogoPhoto;
                item.LibaxEntityID = homeApplianceRepair.LibaxEntityID;

                if (null == item.Payment && null != homeApplianceRepair.Payment)
                {
                    item.Payment = homeApplianceRepair.Payment;
                }
                else if (null != item.Payment && null != homeApplianceRepair.Payment && item.Payment.Count != homeApplianceRepair.Payment.Count)
                {
                    item.Payment.Add(homeApplianceRepair.Payment.Last());
                }

                context.HomeApplianceRepair.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteHomeApplianceRepair(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                HomeApplianceRepair item = context.HomeApplianceRepair.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;
                item.LastChangeDate = DateTime.Now;

                context.HomeApplianceRepair.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool ActivateHomeApplianceRepair(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                HomeApplianceRepair homeApplianceRepair = context.HomeApplianceRepair.Get(id);

                homeApplianceRepair.Active = true;
                homeApplianceRepair.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }

        public static bool DeactivateHomeApplianceRepair(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                HomeApplianceRepair homeApplianceRepair = context.HomeApplianceRepair.Get(id);

                homeApplianceRepair.Active = false;
                homeApplianceRepair.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }
    }
}
