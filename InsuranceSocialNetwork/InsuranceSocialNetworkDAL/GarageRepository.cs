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
                .Include("GarageFavorite")
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
                    .Include("GarageFavorite")
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
                Garage item = context.Garage.Get(garage.ID);
                item.LastChangeDate = DateTime.Now;
                item.Address = garage.Address;
                item.ContactEmail = garage.ContactEmail;
                item.Description = garage.Description;
                item.ID_District = garage.ID_District;
                item.ID_County = garage.ID_County;
                item.ID_Parish = garage.ID_Parish;
                item.ID_Service = garage.ID_Service;
                item.MobilePhone_1 = garage.MobilePhone_1;
                item.MobilePhone_2 = garage.MobilePhone_2;
                item.Name = garage.Name;
                item.NIF = garage.NIF;
                item.OfficialAgent = garage.OfficialAgent;
                item.OfficialPartner = garage.OfficialPartner;
                item.Telephone_1 = garage.Telephone_1;
                item.Telephone_2 = garage.Telephone_2;
                item.Website = garage.Website;
                item.LogoPhoto = null != garage.LogoPhoto ? garage.LogoPhoto : item.LogoPhoto;

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
