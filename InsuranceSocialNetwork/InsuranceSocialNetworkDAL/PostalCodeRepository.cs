using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using InsuranceSocialNetworkCore.Types;

namespace InsuranceSocialNetworkDAL
{
    public class PostalCodeRepository
    {
        public static PostalCode GetInformation(int postalCode, int? postalSubCode)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PostalCode
                    .Fetch()
                    .FirstOrDefault(i => i.Codigo_Postal == postalCode
                        && (!postalSubCode.HasValue || i.Codigo_Arteria.Value == postalSubCode.Value));
            }
        }

        public static List<ListItem> GetDistrictList()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.District
                    .Fetch()
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Name })
                    .ToList();
            }
        }

        public static List<ListItem> GetCountyListByDistrict(long districtId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.County
                    .Fetch()
                    .Where(i => i.District_Code == districtId)
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Name })
                    .ToList();
            }
        }

        public static List<ListItem> GetParishListByCounty(int countyId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Parish
                    .Fetch()
                    .Where(i => i.County_Code == countyId)
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Name })
                    .ToList();
            }
        }
    }
}
