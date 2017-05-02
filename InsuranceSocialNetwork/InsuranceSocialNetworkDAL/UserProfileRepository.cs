using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class UserProfileRepository
    {
        public static Profile GetProfile(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .FirstOrDefault(i => i.ID_User == Id);
            }
        }

        public static List<Profile> GetProfiles()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Select(i => i).OrderBy(i => i.AspNetUsers.UserName).ToList();
            }
        }

        public static long CreateDefaultProfile(Profile profile)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                profile.Active = false;
                profile.CreateDate = DateTime.Now;
                profile.LastChangeDate = DateTime.Now;

                context.Profile.Create(profile);

                AspNetUsers user = context.AspNetUsers.Get(profile.ID_User);
                user.AspNetRoles.Add(RoleRepository.GetRole(context, InsuranceSocialNetworkCore.Enums.RoleEnum.USER));
                
                context.Save();

                return profile.ID;
            }
        }

        public static bool DeleteUserProfile(long userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Profile profile = context.Profile.Get(userId);

                context.Notification.Delete(i => i.ID_User == profile.ID_User);
                context.AspNetUsers.Delete(profile.ID_User);
                //user.AspNetRoles.Add(RoleRepository.GetRole(context, InsuranceSocialNetworkCore.Enums.RoleEnum.USER));
                context.Profile.Delete(userId);

                context.Save();

                return true;
            }
        }
    }
}
