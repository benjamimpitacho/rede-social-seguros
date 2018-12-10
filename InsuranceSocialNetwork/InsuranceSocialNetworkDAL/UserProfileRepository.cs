using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;

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
                    .Include(i => i.AspNetUsers.AspNetRoles)
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
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
                    .Include(i => i.AspNetUsers.AspNetRoles)
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
                    .FirstOrDefault(i => i.ID == Id);
            }
        }

        public static string GetProfileId(long Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .FirstOrDefault(i => i.ID == Id)
                    .ID_User;
            }
        }

        public static long GetProfileId(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .FirstOrDefault(i => i.ID_User == Id)
                    .ID;
            }
        }

        public static List<Profile> GetProfiles()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var rolesIds = context.AspNetRoles
                    .Fetch()
                    .Where(i => i.Name == "ADMINISTRATOR"
                    || i.Name == "ASSOCIATED_PREMIUM"
                    || i.Name == "INSURANCE_PROFESSIONAL"
                    || i.Name == "NORMAL_USER"
                    || i.Name == "PORTAL_COMMERCIAL")
                    .Select(i => i.Id);

                return context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
                    .Where(i => rolesIds.Contains(i.AspNetUsers.AspNetRoles.FirstOrDefault().Id))
                    .Select(i => i)
                    .OrderBy(i => i.AspNetUsers.UserName)
                    .ToList();
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
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
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

        public static List<ListItemString> GetNamesForUsers(List<string> list)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Where(i => list.Contains(i.ID_User))
                    .Select(i => new ListItemString() { Key = i.ID_User, Value = i.FirstName + " " + i.LastName })
                    .ToList();
            }
        }

        public static List<ListItemObject> GetPhotosForUsers(List<string> list)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .Profile
                    .Fetch()
                    .Where(i => list.Contains(i.ID_User))
                    .Select(i => new ListItemObject() { Key = i.ID_User, Value = i.ProfilePhoto })
                    .ToList();
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

        public static List<Profile> GetUserFriends(string currentUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<string> friends1Ids = context.Friend.Fetch().Where(j => j.FriendStatus.Token == FriendStatusEnum.REQUEST_ACCEPTED.ToString() && j.AspNetUsers1.Id == currentUserId).Select(j => j.AspNetUsers.Id).ToList();
                List<string> friends2Ids = context.Friend.Fetch().Where(j => j.FriendStatus.Token == FriendStatusEnum.REQUEST_ACCEPTED.ToString() && j.AspNetUsers.Id == currentUserId).Select(j => j.AspNetUsers1.Id).ToList();
                friends1Ids = friends1Ids.Concat(friends2Ids).ToList();

                List<Profile> friends = context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
                    .Where(i => friends1Ids.Contains(i.AspNetUsers.Id))
                    .Select(i => i)
                    .ToList();

                return friends;
            }
        }

        public static List<Profile> GetUserPendingFriendRequests(string currentUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<string> friends1Ids = context.Friend.Fetch().Where(j => j.FriendStatus.Token == FriendStatusEnum.REQUEST_SENT.ToString() && j.AspNetUsers1.Id == currentUserId).Select(j => j.AspNetUsers.Id).ToList();
                //List<string> friends2Ids = context.Friend.Fetch().Where(j => j.FriendStatus.Token == FriendStatusEnum.REQUEST_ACCEPTED.ToString() && j.AspNetUsers.Id == currentUserId).Select(j => j.AspNetUsers1.Id).ToList();
                //friends1Ids = friends1Ids.Concat(friends2Ids).ToList();

                List<Profile> friends = context
                    .Profile
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    .Include(i => i.ProfileSettings)
                    .Include(i => i.Parish)
                    .Include(i => i.County)
                    .Include(i => i.District)
                    .Where(i => friends1Ids.Contains(i.AspNetUsers.Id))
                    .Select(i => i)
                    .ToList();

                return friends;
            }
        }

        public static List<string> GetUsersToNotify()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<string> roleIds = context.AspNetRoles
                    .Fetch()
                    .Where(j => 
                        j.Name == "NORMAL_USER" 
                        || j.Name == "ASSOCIATED_PREMIUM" 
                        | j.Name == "ADMINISTRATOR")
                    .Select(j => j.Id)
                    .ToList();

                return context.AspNetUsers
                    .Fetch()
                    .Include(i => i.AspNetRoles)
                    .Where(i => roleIds.Contains(i.AspNetRoles.FirstOrDefault().Id))
                    .Select(i => i.Id)
                    .ToList();
            }
        }

        public static long CreateDefaultProfile(Profile profile)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                profile.Active = false;
                profile.CreateDate = DateTime.Now;
                profile.LastChangeDate = DateTime.Now;

                profile.ProfileSettings.Add(new ProfileSettings()
                {
                    Active = true,
                    CreateDate = profile.CreateDate,
                    LastChangeDate = profile.LastChangeDate,
                    ShowSocialNetworks = true,
                    ShowBirthDate = true,
                    ShowContactInformation = true,
                    ShowDisplayName = true,
                    LikesOnYourPosts = true,
                    CommentsOnYourPosts = true,
                    ReceiveNotificationsByEmail = true
                });

                context.Profile.Create(profile);

                //AspNetUsers user = context.AspNetUsers.Get(profile.ID_User);
                //user.AspNetRoles.Add(RoleRepository.GetRole(context, InsuranceSocialNetworkCore.Enums.RoleEnum.USER));

                context.Save();

                return profile.ID;
            }
        }

        public static bool UpdateUserProfile(Profile profile)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                profile.LastChangeDate = DateTime.Now;
                context.Profile.Update(profile);
                context.Save();

                return true;
            }
        }

        public static bool DeleteUserProfile(long userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Profile profile = context.Profile.Get(userId);

                context.Notification.Delete(i => i.ToUserID == profile.ID_User);
                context.Notification.Delete(i => i.FromUserID == profile.ID_User);
                context.ProfileSettings.Delete(i => i.ID_Profile == profile.ID);
                context.Payment.Delete(i => i.ID_Profile == profile.ID);
                context.PostHidden.Delete(i => i.ID_User == profile.ID_User);
                context.PostComment.Delete(i => i.ID_User == profile.ID_User);
                //context.PostCommentLike.Delete(i => i.ID_User == profile.ID_User);
                context.PostLike.Delete(i => i.ID_User == profile.ID_User);
                context.Post.Delete(i => i.ID_User == profile.ID_User);
                context.Friend.Delete(i => i.ID_User == profile.ID_User);
                context.Friend.Delete(i => i.ID_User_Friend == profile.ID_User);

                context.AspNetUsers.Delete(profile.ID_User);
                context.Profile.Delete(userId);

                context.Save();

                return true;
            }
        }

        public static bool ActivateUser(long profileId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Profile profile = context.Profile.Get(profileId);

                //profile.AspNetUsers.EmailConfirmed = true;
                profile.AspNetUsers.LockoutEndDateUtc = null;
                profile.LastChangeDate = DateTime.Now;
                context.Profile.Update(profile);

                context.Save();

                return true;
            }
        }

        public static bool ActivateUser(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                AspNetUsers user = context.AspNetUsers.Get(userId);
                
                user.LockoutEndDateUtc = null;
                context.AspNetUsers.Update(user);

                context.Save();

                return true;
            }
        }

        public static bool DeactivateUser(long profileId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Profile profile = context.Profile.Get(profileId);

                profile.AspNetUsers.LockoutEndDateUtc = DateTime.MaxValue;
                profile.LastChangeDate = DateTime.Now;
                context.Profile.Update(profile);

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

                foreach (var role in user.AspNetRoles)
                {
                    if (functionalityRolesAllowed.Contains(role.Id))
                        return true;
                }
            }
            return false;
        }

        public static bool IsUserInRoleByUsername(string username, string role)
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

                var myRole = user.AspNetRoles.FirstOrDefault(i => i.Name == role);

                if (null == myRole)
                    return false;

                return true;
            }
        }

        public static bool IsUserInRoleByUserID(string userID, string role)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var user = context
                    .AspNetUsers
                    .Fetch()
                    .Include(i => i.AspNetRoles)
                    .FirstOrDefault(i => i.Id == userID);

                if (null == user)
                    return false;

                var myRole = user.AspNetRoles.FirstOrDefault(i => i.Name == role);

                if (null == myRole)
                    return false;

                return true;
            }
        }

        public static List<AspNetUsers> GetAdministratorsList()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                AspNetRoles adminRole = RoleRepository.GetRole(context, InsuranceSocialNetworkCore.Enums.RoleEnum.ADMINISTRATOR);

                return context
                    .AspNetUsers
                    .Fetch()
                    .Include(i => i.AspNetRoles)
                    .Where(i => i.AspNetRoles.Select(j => j.Id).ToList().Contains(adminRole.Id))
                    .Select(i => i).ToList();
            }
        }

        public static ProfileSettings GetProfileSettings(long Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .ProfileSettings
                    .Fetch()
                    .Include(i => i.Profile)
                    .FirstOrDefault(i => i.ID_Profile == Id);
            }
        }

        public static bool UpdateUserProfileSettings(ProfileSettings profileSettings)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                profileSettings.LastChangeDate = DateTime.Now;
                context.ProfileSettings.Update(profileSettings);
                context.Save();

                return true;
            }
        }

        public static List<ConsultantStatisticsDTO> GetConsultantsStatistics()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var garagesStatistics = context
                    .Payment
                    .Fetch()
                    .Include(i => i.Garage)
                    .Include(i => i.Garage.AspNetUsers)
                    .Include(i => i.Garage.AspNetUsers1)
                    .Include(i => i.Garage.AspNetUsers1.Profile)
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID_Garage.HasValue)
                    .Select(i => new ConsultantStatisticsDTO()
                    {
                        Consultant_ID_User = i.Garage.AspNetUsers1.Id,
                        Consultant_Username = i.Garage.AspNetUsers1.UserName,
                        Consultant_FirstName = i.Garage.AspNetUsers1.Profile.FirstOrDefault().FirstName,
                        Consultant_LastName = i.Garage.AspNetUsers1.Profile.FirstOrDefault().LastName,

                        Company_ID = i.Garage.ID,
                        Company_ID_User = i.Garage.ID_User,
                        Company_Name = i.Garage.Name,
                        Company_BusinessName = i.Garage.BusinessName,
                        Company_Description = i.Garage.Description,
                        Company_NIF = i.Garage.NIF,
                        Company_ContactEmail = i.Garage.ContactEmail,
                        Company_CompanyType = CompanyTypeEnum.GARAGE.ToString(),
                        Company_CreationDate = i.Garage.CreateDate,

                        Payment_ID = i.ID,
                        Payment_LiquidValue = i.LiquidValue,
                        Payment_TaxValue = i.TaxValue,
                        Payment_TotalValue = i.TotalValue,
                        Payment_Type = i.PaymentType.Token.ToString(),
                        Payment_Status = i.PaymentStatus.Token.ToString(),
                        Payment_CreationDate = i.CreateDate,
                        Payment_LastUpdateDate = i.LastChangeDate,
                        Payment_PaymentDate = i.PaymentDate
                    })
                    .ToList();

                var medicalClinicStatistics = context
                    .Payment
                    .Fetch()
                    .Include(i => i.MedicalClinic)
                    .Include(i => i.MedicalClinic.AspNetUsers)
                    .Include(i => i.MedicalClinic.AspNetUsers1)
                    .Include(i => i.MedicalClinic.AspNetUsers1.Profile)
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID_MedicalClinic.HasValue)
                    .Select(i => new ConsultantStatisticsDTO()
                    {
                        Consultant_ID_User = i.MedicalClinic.AspNetUsers1.Id,
                        Consultant_Username = i.MedicalClinic.AspNetUsers1.UserName,
                        Consultant_FirstName = i.MedicalClinic.AspNetUsers1.Profile.FirstOrDefault().FirstName,
                        Consultant_LastName = i.MedicalClinic.AspNetUsers1.Profile.FirstOrDefault().LastName,

                        Company_ID = i.MedicalClinic.ID,
                        Company_ID_User = i.MedicalClinic.ID_User,
                        Company_Name = i.MedicalClinic.Name,
                        Company_BusinessName = i.MedicalClinic.BusinessName,
                        Company_Description = i.MedicalClinic.Description,
                        Company_NIF = i.MedicalClinic.NIF,
                        Company_ContactEmail = i.MedicalClinic.ContactEmail,
                        Company_CompanyType = CompanyTypeEnum.MEDICAL_CLINIC.ToString(),
                        Company_CreationDate = i.MedicalClinic.CreateDate,

                        Payment_ID = i.ID,
                        Payment_LiquidValue = i.LiquidValue,
                        Payment_TaxValue = i.TaxValue,
                        Payment_TotalValue = i.TotalValue,
                        Payment_Type = i.PaymentType.Token.ToString(),
                        Payment_Status = i.PaymentStatus.Token.ToString(),
                        Payment_CreationDate = i.CreateDate,
                        Payment_LastUpdateDate = i.LastChangeDate,
                        Payment_PaymentDate = i.PaymentDate
                    })
                    .ToList();

                var constructionCompanyStatistics = context
                    .Payment
                    .Fetch()
                    .Include(i => i.ConstructionCompany)
                    .Include(i => i.ConstructionCompany.AspNetUsers)
                    .Include(i => i.ConstructionCompany.AspNetUsers1)
                    .Include(i => i.ConstructionCompany.AspNetUsers1.Profile)
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID_ConstructionCompany.HasValue)
                    .Select(i => new ConsultantStatisticsDTO()
                    {
                        Consultant_ID_User = i.ConstructionCompany.AspNetUsers1.Id,
                        Consultant_Username = i.ConstructionCompany.AspNetUsers1.UserName,
                        Consultant_FirstName = i.ConstructionCompany.AspNetUsers1.Profile.FirstOrDefault().FirstName,
                        Consultant_LastName = i.ConstructionCompany.AspNetUsers1.Profile.FirstOrDefault().LastName,

                        Company_ID = i.ConstructionCompany.ID,
                        Company_ID_User = i.ConstructionCompany.ID_User,
                        Company_Name = i.ConstructionCompany.Name,
                        Company_BusinessName = i.ConstructionCompany.BusinessName,
                        Company_Description = i.ConstructionCompany.Description,
                        Company_NIF = i.ConstructionCompany.NIF,
                        Company_ContactEmail = i.ConstructionCompany.ContactEmail,
                        Company_CompanyType = CompanyTypeEnum.CONSTRUCTION_COMPANY.ToString(),
                        Company_CreationDate = i.ConstructionCompany.CreateDate,

                        Payment_ID = i.ID,
                        Payment_LiquidValue = i.LiquidValue,
                        Payment_TaxValue = i.TaxValue,
                        Payment_TotalValue = i.TotalValue,
                        Payment_Type = i.PaymentType.Token.ToString(),
                        Payment_Status = i.PaymentStatus.Token.ToString(),
                        Payment_CreationDate = i.CreateDate,
                        Payment_LastUpdateDate = i.LastChangeDate,
                        Payment_PaymentDate = i.PaymentDate
                    })
                    .ToList();

                var homeApplianceRepairStatistics = context
                    .Payment
                    .Fetch()
                    .Include(i => i.HomeApplianceRepair)
                    .Include(i => i.HomeApplianceRepair.AspNetUsers)
                    .Include(i => i.HomeApplianceRepair.AspNetUsers1)
                    .Include(i => i.HomeApplianceRepair.AspNetUsers1.Profile)
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID_HomeApplianceRepair.HasValue)
                    .Select(i => new ConsultantStatisticsDTO()
                    {
                        Consultant_ID_User = i.HomeApplianceRepair.AspNetUsers1.Id,
                        Consultant_Username = i.HomeApplianceRepair.AspNetUsers1.UserName,
                        Consultant_FirstName = i.HomeApplianceRepair.AspNetUsers1.Profile.FirstOrDefault().FirstName,
                        Consultant_LastName = i.HomeApplianceRepair.AspNetUsers1.Profile.FirstOrDefault().LastName,

                        Company_ID = i.HomeApplianceRepair.ID,
                        Company_ID_User = i.HomeApplianceRepair.ID_User,
                        Company_Name = i.HomeApplianceRepair.Name,
                        Company_BusinessName = i.HomeApplianceRepair.BusinessName,
                        Company_Description = i.HomeApplianceRepair.Description,
                        Company_NIF = i.HomeApplianceRepair.NIF,
                        Company_ContactEmail = i.HomeApplianceRepair.ContactEmail,
                        Company_CompanyType = CompanyTypeEnum.HOME_APPLIANCES_REPAIR.ToString(),
                        Company_CreationDate = i.HomeApplianceRepair.CreateDate,

                        Payment_ID = i.ID,
                        Payment_LiquidValue = i.LiquidValue,
                        Payment_TaxValue = i.TaxValue,
                        Payment_TotalValue = i.TotalValue,
                        Payment_Type = i.PaymentType.Token.ToString(),
                        Payment_Status = i.PaymentStatus.Token.ToString(),
                        Payment_CreationDate = i.CreateDate,
                        Payment_LastUpdateDate = i.LastChangeDate,
                        Payment_PaymentDate = i.PaymentDate
                    })
                    .ToList();

                var insuranceCompanyContactStatistics = context
                    .Payment
                    .Fetch()
                    .Include(i => i.InsuranceCompanyContact)
                    .Include(i => i.InsuranceCompanyContact.AspNetUsers)
                    .Include(i => i.InsuranceCompanyContact.AspNetUsers1)
                    .Include(i => i.InsuranceCompanyContact.AspNetUsers1.Profile)
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID_InsuranceCompanyContact.HasValue)
                    .Select(i => new ConsultantStatisticsDTO()
                    {
                        Consultant_ID_User = i.InsuranceCompanyContact.AspNetUsers1.Id,
                        Consultant_Username = i.InsuranceCompanyContact.AspNetUsers1.UserName,
                        Consultant_FirstName = i.InsuranceCompanyContact.AspNetUsers1.Profile.FirstOrDefault().FirstName,
                        Consultant_LastName = i.InsuranceCompanyContact.AspNetUsers1.Profile.FirstOrDefault().LastName,

                        Company_ID = i.InsuranceCompanyContact.ID,
                        Company_ID_User = i.InsuranceCompanyContact.ID_User,
                        Company_Name = i.InsuranceCompanyContact.Name,
                        Company_BusinessName = i.InsuranceCompanyContact.BusinessName,
                        Company_Description = i.InsuranceCompanyContact.Description,
                        Company_NIF = i.InsuranceCompanyContact.NIF,
                        Company_ContactEmail = i.InsuranceCompanyContact.ContactEmail,
                        Company_CompanyType = CompanyTypeEnum.INSURANCE_COMPANY_CONTACT.ToString(),
                        Company_CreationDate = i.InsuranceCompanyContact.CreateDate,

                        Payment_ID = i.ID,
                        Payment_LiquidValue = i.LiquidValue,
                        Payment_TaxValue = i.TaxValue,
                        Payment_TotalValue = i.TotalValue,
                        Payment_Type = i.PaymentType.Token.ToString(),
                        Payment_Status = i.PaymentStatus.Token.ToString(),
                        Payment_CreationDate = i.CreateDate,
                        Payment_LastUpdateDate = i.LastChangeDate,
                        Payment_PaymentDate = i.PaymentDate
                    })
                    .ToList();

                return garagesStatistics.Concat(medicalClinicStatistics).Concat(constructionCompanyStatistics).Concat(homeApplianceRepairStatistics).Concat(insuranceCompanyContactStatistics).ToList();
            }
        }
    }
}
