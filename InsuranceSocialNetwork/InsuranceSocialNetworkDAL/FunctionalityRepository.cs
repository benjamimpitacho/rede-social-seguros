using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class FunctionalityRepository
    {
        public static List<string> GetFunctionalityNames()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.AspNetRolesFunctionalities.Fetch().Select(i => i.Token).OrderBy(i => i).ToList();
            }
        }
        public static List<AspNetRolesFunctionalities> GetFunctionalities()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.AspNetRolesFunctionalities
                    .Fetch()
                    .Where(i=>i.Active)
                    .Select(i => i)
                    .OrderBy(i => i.Token)
                    .ToList();
            }
        }
    }
}
