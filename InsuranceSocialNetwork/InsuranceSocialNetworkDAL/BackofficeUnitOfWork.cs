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
        private IRepository<Notification> notificationRepository;
        private IRepository<NotificationType> notificationTypeRepository;
        private IRepository<AspNetRoles> aspNetRolesRepository;
        private IRepository<AspNetUsers> aspNetUsersRepository;
        private IRepository<Post> postRepository;
        private IRepository<PostType> postTypeRepository;
        private IRepository<PostSubject> postSubjectRepository;
        private IRepository<PostLike> postLikeRepository;
        private IRepository<PostComment> postCommentRepository;
        private IRepository<Garage> garageRepository;
        private IRepository<MedicalClinic> medicalClinicRepository;

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
    }
}
