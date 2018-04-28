using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class GarageRepository
    {
        public static Garage GetGarage(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetGarage(context, id);
            }
        }

        public static Garage GetGarage(BackofficeUnitOfWork context, long id)
        {
            return context.Garage
                .Fetch()
                .Include(i => i.GarageFavorite)
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

        public static List<Garage> GetGarages()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Garage
                    .Fetch()
                    .Include(i => i.GarageFavorite)
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

        public static long CreateGarage(Garage garage)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                garage.Active = true;
                garage.CreateDate = garage.LastChangeDate = DateTime.Now;

                context.Garage.Create(garage);
                context.Save();

                return garage.ID;
            }
        }

        public static bool EditGarage(Garage garage)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Garage item = GetGarage(garage.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = garage.Address;
                item.ContactEmail = garage.ContactEmail;
                item.Description = garage.Description;
                item.Address = garage.Address;
                item.PostalCode = garage.PostalCode;
                item.ID_District = garage.ID_District;
                item.ID_County = garage.ID_County;
                item.ID_Parish = garage.ID_Parish;
                item.SameInformationForInvoice = garage.SameInformationForInvoice;
                item.Address = garage.Address;
                item.PostalCode = garage.PostalCode;
                item.Invoice_ID_District = garage.Invoice_ID_District;
                item.Invoice_ID_County = garage.Invoice_ID_County;
                item.Invoice_ID_Parish = garage.Invoice_ID_Parish;
                item.ID_Service = garage.ID_Service;
                item.MobilePhone_1 = garage.MobilePhone_1;
                item.MobilePhone_2 = garage.MobilePhone_2;
                item.Fax = garage.Fax;
                item.Name = garage.Name;
                item.NIF = garage.NIF;
                item.OfficialAgent = garage.OfficialAgent;
                item.OfficialPartner = garage.OfficialPartner;
                item.Telephone_1 = garage.Telephone_1;
                item.Telephone_2 = garage.Telephone_2;
                item.Website = garage.Website;
                item.LogoPhoto = null != garage.LogoPhoto ? garage.LogoPhoto : item.LogoPhoto;
                item.LibaxEntityID = garage.LibaxEntityID;

                if (null == item.Payment && null != garage.Payment)
                {
                    item.Payment = garage.Payment;
                    context.Payment.Create(garage.Payment.Last());
                }
                else if (null != item.Payment && null != garage.Payment && item.Payment.Count != garage.Payment.Count)
                {
                    item.Payment.Add(garage.Payment.Last());
                    context.Payment.Create(garage.Payment.Last());
                }

                context.Garage.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteGarage(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Garage item = context.Garage.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;
                item.LastChangeDate = DateTime.Now;

                context.Garage.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool ActivateGarage(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Garage garage = context.Garage.Get(id);

                garage.Active = true;
                garage.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }

        public static bool DeactivateGarage(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Garage garage = context.Garage.Get(id);

                garage.Active = false;
                garage.LastChangeDate = DateTime.Now;

                context.Save();

                return true;
            }
        }
    }
}
