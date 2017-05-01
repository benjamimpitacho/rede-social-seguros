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
        public static List<Garage> GetGarages()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Garage
                    .Fetch()
                    .Select(i => i)
                    .OrderBy(i => i.Name)
                    .ToList();
            }
        }
    }
}
