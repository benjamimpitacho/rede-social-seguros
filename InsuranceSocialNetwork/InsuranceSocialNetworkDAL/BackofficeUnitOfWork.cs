using InsuranceSocialNetworkDAL.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class BackofficeUnitOfWork : UnitOfWork<InsurancePortal_dbEntities>
    {
        private IRepository<Profile> profileRepository;
        private IRepository<ProfileSettings> profileSettingsRepository;
        private IRepository<Notification> notificationRepository;
        private IRepository<NotificationType> notificationTypeRepository;
        private IRepository<AspNetRoles> aspNetRolesRepository;
        private IRepository<AspNetUsers> aspNetUsersRepository;
        private IRepository<AspNetRolesFunctionalities> aspNetRolesFunctionalitiesRepository;
        private IRepository<Post> postRepository;
        private IRepository<PostImage> postImageRepository;
        private IRepository<PostHidden> postHiddenRepository;
        private IRepository<PostType> postTypeRepository;
        private IRepository<PostSubject> postSubjectRepository;
        private IRepository<PostLike> postLikeRepository;
        private IRepository<PostComment> postCommentRepository;
        private IRepository<Garage> garageRepository;
        private IRepository<MedicalClinic> medicalClinicRepository;
        private IRepository<ConstructionCompany> constructionCompanyRepository;
        private IRepository<HomeApplianceRepair> homeApplianceRepairRepository;
        private IRepository<InsuranceCompanyContact> insuranceCompanyContactRepository;
        private IRepository<GarageFavorite> garageFavoriteRepository;
        private IRepository<MedicalClinicFavorite> medicalClinicFavoriteRepository;
        private IRepository<ConstructionCompanyFavorite> constructionCompanyFavoriteRepository;
        private IRepository<HomeApplianceRepairFavorite> homeApplianceRepairFavoriteRepository;
        private IRepository<InsuranceCompanyContactFavorite> insuranceCompanyContactFavoriteRepository;
        private IRepository<Friend> friendRepository;
        private IRepository<FriendStatus> friendStatusRepository;
        private IRepository<Chat> chatRepository;
        private IRepository<ChatMember> chatMemberRepository;
        private IRepository<ChatMessage> chatMessageRepository;
        private IRepository<Banner> bannerRepository;
        private IRepository<BannerType> bannerTypeRepository;
        private IRepository<District> districtRepository;
        private IRepository<County> countyRepository;
        private IRepository<Parish> parishRepository;

        private IRepository<CompanyType> companyTypeRepository;
        private IRepository<CompanyService> companyServiceRepository;
        private IRepository<AuthorizedEmail> authorizedEmailRepository;

        private bool AutoDetectChangesEnabled;
        private int DatabaseTimeout;

        #region Constructors

        public BackofficeUnitOfWork()
        {
            this.AutoDetectChangesEnabled = true;
        }

        public BackofficeUnitOfWork(int databaseTimeout)
        {
            this.AutoDetectChangesEnabled = true;
            this.DatabaseTimeout = databaseTimeout;
        }

        public BackofficeUnitOfWork(DbContext context)
            : base(context as InsurancePortal_dbEntities)
        {
            this.AutoDetectChangesEnabled = true;
        }

        public BackofficeUnitOfWork(bool autoDetectChangesEnabled)
        {
            this.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public BackofficeUnitOfWork(bool autoDetectChangesEnabled, int databaseTimeout)
        {
            this.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            this.DatabaseTimeout = databaseTimeout;
        }

        #endregion Constructors
        
        public IRepository<Chat> Chat
        {
            get
            {
                if (this.chatRepository == null)
                {
                    this.chatRepository =
                        new Repository<InsurancePortal_dbEntities, Chat>(DataContext, AutoDetectChangesEnabled);
                }
                return chatRepository;
            }
        }

        public IRepository<ChatMessage> ChatMessage
        {
            get
            {
                if (this.chatMessageRepository == null)
                {
                    this.chatMessageRepository =
                        new Repository<InsurancePortal_dbEntities, ChatMessage>(DataContext, AutoDetectChangesEnabled);
                }
                return chatMessageRepository;
            }
        }

        public IRepository<ChatMember> ChatMember
        {
            get
            {
                if (this.chatMemberRepository == null)
                {
                    this.chatMemberRepository =
                        new Repository<InsurancePortal_dbEntities, ChatMember>(DataContext, AutoDetectChangesEnabled);
                }
                return chatMemberRepository;
            }
        }

        public IRepository<Profile> Profile
        {
            get
            {
                if (this.profileRepository == null)
                {
                    this.profileRepository =
                        new Repository<InsurancePortal_dbEntities, Profile>(DataContext, AutoDetectChangesEnabled);
                }
                return profileRepository;
            }
        }

        public IRepository<ProfileSettings> ProfileSettings
        {
            get
            {
                if (this.profileSettingsRepository == null)
                {
                    this.profileSettingsRepository =
                        new Repository<InsurancePortal_dbEntities, ProfileSettings>(DataContext, AutoDetectChangesEnabled);
                }
                return profileSettingsRepository;
            }
        }

        public IRepository<AspNetUsers> AspNetUsers
        {
            get
            {
                if (this.aspNetUsersRepository == null)
                {
                    this.aspNetUsersRepository =
                        new Repository<InsurancePortal_dbEntities, AspNetUsers>(DataContext, AutoDetectChangesEnabled);
                }
                return aspNetUsersRepository;
            }
        }

        public IRepository<AspNetRoles> AspNetRoles
        {
            get
            {
                if (this.aspNetRolesRepository == null)
                {
                    this.aspNetRolesRepository =
                        new Repository<InsurancePortal_dbEntities, AspNetRoles>(DataContext, AutoDetectChangesEnabled);
                }
                return aspNetRolesRepository;
            }
        }

        public IRepository<AspNetRolesFunctionalities> AspNetRolesFunctionalities
        {
            get
            {
                if (this.aspNetRolesFunctionalitiesRepository == null)
                {
                    this.aspNetRolesFunctionalitiesRepository =
                        new Repository<InsurancePortal_dbEntities, AspNetRolesFunctionalities>(DataContext, AutoDetectChangesEnabled);
                }
                return aspNetRolesFunctionalitiesRepository;
            }
        }

        public IRepository<Banner> Banner
        {
            get
            {
                if (this.bannerRepository == null)
                {
                    this.bannerRepository =
                        new Repository<InsurancePortal_dbEntities, Banner>(DataContext, AutoDetectChangesEnabled);
                }
                return bannerRepository;
            }
        }

        public IRepository<BannerType> BannerType
        {
            get
            {
                if (this.bannerTypeRepository == null)
                {
                    this.bannerTypeRepository =
                        new Repository<InsurancePortal_dbEntities, BannerType>(DataContext, AutoDetectChangesEnabled);
                }
                return bannerTypeRepository;
            }
        }

        public IRepository<Notification> Notification
        {
            get
            {
                if (this.notificationRepository == null)
                {
                    this.notificationRepository =
                        new Repository<InsurancePortal_dbEntities, Notification>(DataContext, AutoDetectChangesEnabled);
                }
                return notificationRepository;
            }
        }

        public IRepository<NotificationType> NotificationType
        {
            get
            {
                if (this.notificationTypeRepository == null)
                {
                    this.notificationTypeRepository =
                        new Repository<InsurancePortal_dbEntities, NotificationType>(DataContext, AutoDetectChangesEnabled);
                }
                return notificationTypeRepository;
            }
        }

        public IRepository<Post> Post
        {
            get
            {
                if (this.postRepository == null)
                {
                    this.postRepository =
                        new Repository<InsurancePortal_dbEntities, Post>(DataContext, AutoDetectChangesEnabled);
                }
                return postRepository;
            }
        }

        public IRepository<PostImage> PostImage
        {
            get
            {
                if (this.postImageRepository == null)
                {
                    this.postImageRepository =
                        new Repository<InsurancePortal_dbEntities, PostImage>(DataContext, AutoDetectChangesEnabled);
                }
                return postImageRepository;
            }
        }

        public IRepository<PostHidden> PostHidden
        {
            get
            {
                if (this.postHiddenRepository == null)
                {
                    this.postHiddenRepository =
                        new Repository<InsurancePortal_dbEntities, PostHidden>(DataContext, AutoDetectChangesEnabled);
                }
                return postHiddenRepository;
            }
        }

        public IRepository<PostType> PostType
        {
            get
            {
                if (this.postTypeRepository == null)
                {
                    this.postTypeRepository =
                        new Repository<InsurancePortal_dbEntities, PostType>(DataContext, AutoDetectChangesEnabled);
                }
                return postTypeRepository;
            }
        }

        public IRepository<PostSubject> PostSubject
        {
            get
            {
                if (this.postSubjectRepository == null)
                {
                    this.postSubjectRepository =
                        new Repository<InsurancePortal_dbEntities, PostSubject>(DataContext, AutoDetectChangesEnabled);
                }
                return postSubjectRepository;
            }
        }

        public IRepository<PostLike> PostLike
        {
            get
            {
                if (this.postLikeRepository == null)
                {
                    this.postLikeRepository =
                        new Repository<InsurancePortal_dbEntities, PostLike>(DataContext, AutoDetectChangesEnabled);
                }
                return postLikeRepository;
            }
        }

        public IRepository<PostComment> PostComment
        {
            get
            {
                if (this.postCommentRepository == null)
                {
                    this.postCommentRepository =
                        new Repository<InsurancePortal_dbEntities, PostComment>(DataContext, AutoDetectChangesEnabled);
                }
                return postCommentRepository;
            }
        }

        public IRepository<Friend> Friend
        {
            get
            {
                if (this.friendRepository == null)
                {
                    this.friendRepository =
                        new Repository<InsurancePortal_dbEntities, Friend>(DataContext, AutoDetectChangesEnabled);
                }
                return friendRepository;
            }
        }

        public IRepository<FriendStatus> FriendStatus
        {
            get
            {
                if (this.friendStatusRepository == null)
                {
                    this.friendStatusRepository =
                        new Repository<InsurancePortal_dbEntities, FriendStatus>(DataContext, AutoDetectChangesEnabled);
                }
                return friendStatusRepository;
            }
        }

        public IRepository<District> District
        {
            get
            {
                if (this.districtRepository == null)
                {
                    this.districtRepository =
                        new Repository<InsurancePortal_dbEntities, District>(DataContext, AutoDetectChangesEnabled);
                }
                return districtRepository;
            }
        }

        public IRepository<County> County
        {
            get
            {
                if (this.countyRepository == null)
                {
                    this.countyRepository =
                        new Repository<InsurancePortal_dbEntities, County>(DataContext, AutoDetectChangesEnabled);
                }
                return countyRepository;
            }
        }

        public IRepository<Parish> Parish
        {
            get
            {
                if (this.parishRepository == null)
                {
                    this.parishRepository =
                        new Repository<InsurancePortal_dbEntities, Parish>(DataContext, AutoDetectChangesEnabled);
                }
                return parishRepository;
            }
        }

        public IRepository<Garage> Garage
        {
            get
            {
                if (this.garageRepository == null)
                {
                    this.garageRepository =
                        new Repository<InsurancePortal_dbEntities, Garage>(DataContext, AutoDetectChangesEnabled);
                }
                return garageRepository;
            }
        }

        public IRepository<GarageFavorite> GarageFavorite
        {
            get
            {
                if (this.garageFavoriteRepository == null)
                {
                    this.garageFavoriteRepository =
                        new Repository<InsurancePortal_dbEntities, GarageFavorite>(DataContext, AutoDetectChangesEnabled);
                }
                return garageFavoriteRepository;
            }
        }

        public IRepository<MedicalClinic> MedicalClinic
        {
            get
            {
                if (this.medicalClinicRepository == null)
                {
                    this.medicalClinicRepository =
                        new Repository<InsurancePortal_dbEntities, MedicalClinic>(DataContext, AutoDetectChangesEnabled);
                }
                return medicalClinicRepository;
            }
        }

        public IRepository<MedicalClinicFavorite> MedicalClinicFavorite
        {
            get
            {
                if (this.medicalClinicFavoriteRepository == null)
                {
                    this.medicalClinicFavoriteRepository =
                        new Repository<InsurancePortal_dbEntities, MedicalClinicFavorite>(DataContext, AutoDetectChangesEnabled);
                }
                return medicalClinicFavoriteRepository;
            }
        }

        public IRepository<ConstructionCompany> ConstructionCompany
        {
            get
            {
                if (this.constructionCompanyRepository == null)
                {
                    this.constructionCompanyRepository =
                        new Repository<InsurancePortal_dbEntities, ConstructionCompany>(DataContext, AutoDetectChangesEnabled);
                }
                return constructionCompanyRepository;
            }
        }

        public IRepository<ConstructionCompanyFavorite> ConstructionCompanyFavorite
        {
            get
            {
                if (this.constructionCompanyFavoriteRepository == null)
                {
                    this.constructionCompanyFavoriteRepository =
                        new Repository<InsurancePortal_dbEntities, ConstructionCompanyFavorite>(DataContext, AutoDetectChangesEnabled);
                }
                return constructionCompanyFavoriteRepository;
            }
        }

        public IRepository<HomeApplianceRepair> HomeApplianceRepair
        {
            get
            {
                if (this.homeApplianceRepairRepository == null)
                {
                    this.homeApplianceRepairRepository =
                        new Repository<InsurancePortal_dbEntities, HomeApplianceRepair>(DataContext, AutoDetectChangesEnabled);
                }
                return homeApplianceRepairRepository;
            }
        }

        public IRepository<HomeApplianceRepairFavorite> HomeApplianceRepairFavorite
        {
            get
            {
                if (this.homeApplianceRepairFavoriteRepository == null)
                {
                    this.homeApplianceRepairFavoriteRepository =
                        new Repository<InsurancePortal_dbEntities, HomeApplianceRepairFavorite>(DataContext, AutoDetectChangesEnabled);
                }
                return homeApplianceRepairFavoriteRepository;
            }
        }

        public IRepository<InsuranceCompanyContact> InsuranceCompanyContact
        {
            get
            {
                if (this.insuranceCompanyContactRepository == null)
                {
                    this.insuranceCompanyContactRepository =
                        new Repository<InsurancePortal_dbEntities, InsuranceCompanyContact>(DataContext, AutoDetectChangesEnabled);
                }
                return insuranceCompanyContactRepository;
            }
        }

        public IRepository<InsuranceCompanyContactFavorite> InsuranceCompanyContactFavorite
        {
            get
            {
                if (this.insuranceCompanyContactFavoriteRepository == null)
                {
                    this.insuranceCompanyContactFavoriteRepository =
                        new Repository<InsurancePortal_dbEntities, InsuranceCompanyContactFavorite>(DataContext, AutoDetectChangesEnabled);
                }
                return insuranceCompanyContactFavoriteRepository;
            }
        }

        public IRepository<CompanyType> CompanyType
        {
            get
            {
                if (this.companyTypeRepository == null)
                {
                    this.companyTypeRepository =
                        new Repository<InsurancePortal_dbEntities, CompanyType>(DataContext, AutoDetectChangesEnabled);
                }
                return companyTypeRepository;
            }
        }

        public IRepository<CompanyService> CompanyService
        {
            get
            {
                if (this.companyServiceRepository == null)
                {
                    this.companyServiceRepository =
                        new Repository<InsurancePortal_dbEntities, CompanyService>(DataContext, AutoDetectChangesEnabled);
                }
                return companyServiceRepository;
            }
        }

        public IRepository<AuthorizedEmail> AuthorizedEmail
        {
            get
            {
                if (this.authorizedEmailRepository == null)
                {
                    this.authorizedEmailRepository =
                        new Repository<InsurancePortal_dbEntities, AuthorizedEmail>(DataContext, AutoDetectChangesEnabled);
                }
                return authorizedEmailRepository;
            }
        }
    }
}
