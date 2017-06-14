﻿using InsuranceSocialNetworkDTO.UserProfile;
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
        public static Profile GetProfile(long Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .FirstOrDefault(i => i.ID == Id);
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

        public static List<Profile> SearchProfiles(string searchTerm, long currentUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Where(i => i.ID != currentUserId
                        && (
                            i.FirstName.ToLower().Contains(searchTerm.ToLower())
                            || i.LastName.ToLower().Contains(searchTerm.ToLower())
                            || i.AspNetUsers.UserName.ToLower().Contains(searchTerm.ToLower())
                        )
                    )
                    .Select(i => i).OrderBy(i => i.AspNetUsers.UserName).ToList();
            }
        }

        public static List<long> GetUserFriendsIDs(long currentUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<long> friends1 = context
                    .Friend
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Include(i => i.AspNetUsers.Profile)
                    .Where(i => i.AspNetUsers.Profile.FirstOrDefault().ID == currentUserId)
                    .Select(i => i.AspNetUsers1.Profile.FirstOrDefault().ID)
                    .ToList();

                List<long> friends2 = context
                    .Friend
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Include(i => i.AspNetUsers.Profile)
                    .Where(i => i.AspNetUsers1.Profile.FirstOrDefault().ID == currentUserId)
                    .Select(i => i.AspNetUsers.Profile.FirstOrDefault().ID)
                    .ToList();

                return friends1.Concat(friends2).ToList();
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

                //AspNetUsers user = context.AspNetUsers.Get(profile.ID_User);
                //user.AspNetRoles.Add(RoleRepository.GetRole(context, InsuranceSocialNetworkCore.Enums.RoleEnum.USER));
                
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

        public static AspNetUsers GetUser(string username)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .AspNetUsers
                    .Fetch()
                    .Include(i => i.AspNetRoles)
                    .FirstOrDefault(i => i.UserName == username);
            }
        }

        public static bool IsUserAuthorizedToFunctionality(string username, string functionality)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var user = context
                    .AspNetUsers
                    .Fetch()
                    .Include(i => i.AspNetRoles)
                    .FirstOrDefault(i => i.UserName == username);

                if (null == user)
                    return false;

                var rolesIDs = user.AspNetRoles.Select(i => i.Id).ToList();

                List<string> functionalityRolesAllowed = context.AspNetRolesFunctionalities.Fetch().Where(i => i.Token == functionality && i.Active).Select(i => i.RoleId).ToList();

                foreach(var role in user.AspNetRoles)
                {
                    if (functionalityRolesAllowed.Contains(role.Id))
                        return true;
                }
            }
            return false;
        }
    }
}
