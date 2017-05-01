using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class RoleRepository
    {
        public static AspNetRoles GetRole(RoleEnum role)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetRole(context, role);
            }
        }

        public static AspNetRoles GetRole(BackofficeUnitOfWork context, RoleEnum role)
        {
            return context.AspNetRoles.Fetch().FirstOrDefault(i => i.Name.ToUpper() == role.ToString().ToUpper());
        }

        public static List<AspNetRoles> GetRoles()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.AspNetRoles.Fetch().Select(i => i).OrderBy(i => i.Name).ToList();
            }
        }
    }
}
