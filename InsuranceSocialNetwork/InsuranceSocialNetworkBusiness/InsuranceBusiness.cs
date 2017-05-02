//using AutoMapper;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDAL;
using InsuranceSocialNetworkDTO.Garage;
using InsuranceSocialNetworkDTO.MedicalClinic;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkBusiness
{
    public class InsuranceBusiness
    {
        public static InsuranceBusiness BusinessLayer = new InsuranceBusiness();

        public InsuranceBusiness()
        {
            #region AutoMapper

            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<Profile, UserProfileDTO>()
                    .ForMember(dest => dest.User,
                       opts => opts.MapFrom(src => src.AspNetUsers));
                cfg.CreateMap<UserProfileDTO, Profile>();

                cfg.CreateMap<Notification, NotificationDTO>();
                cfg.CreateMap<NotificationDTO, Notification>();

                cfg.CreateMap<Post, PostDTO>()
                    .ForMember(dest => dest.PostOwner,
                       opts => opts.MapFrom(src => src.AspNetUsers.Profile.FirstOrDefault()));
                cfg.CreateMap<PostDTO, Post>();

                cfg.CreateMap<PostTypeDTO, PostType>();
                cfg.CreateMap<PostType, PostTypeDTO>();

                cfg.CreateMap<PostSubjectDTO, PostSubject>();
                cfg.CreateMap<PostSubject, PostSubjectDTO>();

                cfg.CreateMap<PostLikeDTO, PostLike>();
                cfg.CreateMap<PostLike, PostLikeDTO>();

                cfg.CreateMap<PostImageDTO, PostImage>();
                cfg.CreateMap<PostImage, PostImageDTO>();

                cfg.CreateMap<PostCommentDTO, PostComment>();
                cfg.CreateMap<PostComment, PostCommentDTO>()
                    .ForMember(dest => dest.CommentOwner,
                       opts => opts.MapFrom(src => src.AspNetUsers.Profile.FirstOrDefault()));

                cfg.CreateMap<AspNetRoles, RoleDTO>();
                cfg.CreateMap<RoleDTO, AspNetRoles>();

                cfg.CreateMap<AspNetUsers, UserDTO>();
                cfg.CreateMap<UserDTO, AspNetUsers>();

                cfg.CreateMap<Garage, GarageDTO>();
                cfg.CreateMap<GarageDTO, Garage>();

                cfg.CreateMap<MedicalClinic, MedicalClinicDTO>();
                cfg.CreateMap<MedicalClinicDTO, MedicalClinic>();
            });

            #endregion
        }

        #region Roles

        public List<RoleDTO> GetRoles()
        {
            List<AspNetRoles> roles = RoleRepository.GetRoles();
            return AutoMapper.Mapper.Map<List<RoleDTO>>(roles);
        }

        #endregion Roles

        #region Profile

        public UserProfileDTO GetUserProfile(string Id)
        {
            Profile item = UserProfileRepository.GetProfile(Id);
            return AutoMapper.Mapper.Map<UserProfileDTO>(item);
        }

        public List<UserProfileDTO> GetUsers()
        {
            List<Profile> users = UserProfileRepository.GetProfiles();
            return AutoMapper.Mapper.Map<List<UserProfileDTO>>(users);
        }

        public long CreateDefaultUserProfile(string Id, string UserName, string Email, string name)
        {
            string trimmedName = name.Trim();
            string firstName = trimmedName;
            string lastName = string.Empty;
            if(trimmedName.Contains(" "))
            {
                firstName = trimmedName.Substring(0, trimmedName.IndexOf(" "));
                lastName = trimmedName.Substring(trimmedName.IndexOf(" ") + 1);
            }
            return UserProfileRepository.CreateDefaultProfile(AutoMapper.Mapper.Map<Profile>(new UserProfileDTO() { ID_User = Id, ContactEmail = Email, FirstName = firstName, LastName = lastName}));
        }

        public bool DeleteUserProfile(long userId)
        {
            return UserProfileRepository.DeleteUserProfile(userId);
        }

        #endregion Profile

        #region Notifications

        public List<NotificationDTO> GetUserNotifications(string Id)
        {
            List<Notification> list = NotificationRepository.GetUserNotifications(Id);
            return AutoMapper.Mapper.Map<List<NotificationDTO>>(list);
        }

        public bool CreateNotification(NotificationDTO item)
        {
            Notification notification = AutoMapper.Mapper.Map<Notification>(item);

            notification.ID_NotificationType = NotificationRepository.GetNotificationType(item.Type.ToString()).ID;

            return NotificationRepository.CreateNotification(notification);
        }

        public bool CreateNotification(string userId, NotificationTypeEnum type)
        {
            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                ID_User = userId
            };

            return NotificationRepository.CreateNotification(notification);
        }

        #endregion Notifications

        #region Posts

        public List<PostDTO> GetUserPosts(string Id)
        {
            List<Post> list = PostRepository.GetUserPosts(Id);

            list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
            list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


            return AutoMapper.Mapper.Map<List<PostDTO>>(list);
        }

        public bool CreatePost(PostDTO item)
        {
            Post post = AutoMapper.Mapper.Map<Post>(item);

            post.ID_PostType = PostRepository.GetPostType(item.Type.ToString()).ID;
            post.ID_PostSubject = PostRepository.GetPostSubject(item.Subject.ToString()).ID;

            if(null != item.Image)
            {
                post.PostImage = new List<PostImage>();
                post.PostImage.Add(new PostImage()
                {
                    Active = true,
                    //AspNetUsers = post.AspNetUsers,
                    Date = DateTime.Now,
                    //ID_User = post.ID_User,
                    Image = item.Image
                });
            }

            return PostRepository.CreatePost(post);
        }

        public bool LikePost(long postId, string userId)
        {
            return PostRepository.LikePost(postId, userId);
        }

        public bool UnlikePost(long postId, string userId)
        {
            return PostRepository.UnlikePost(postId, userId);
        }

        #endregion Posts

        #region Comments

        public bool CreateComment(PostCommentDTO item)
        {
            PostComment comment = AutoMapper.Mapper.Map<PostComment>(item);

            return CommentRepository.CreateComment(comment);
        }

        #endregion Comments

        #region Garages

        public List<GarageDTO> GetGarages()
        {
            List<Garage> items = GarageRepository.GetGarages();
            return AutoMapper.Mapper.Map<List<GarageDTO>>(items);
        }

        #endregion Garages

        #region Medical Clinics

        public List<MedicalClinicDTO> GetMedicalClinics()
        {
            List<MedicalClinic> items = MedicalClinicRepository.GetMedicalClinics();
            return AutoMapper.Mapper.Map<List<MedicalClinicDTO>>(items);
        }

        #endregion Medical Clinics
    }
}
