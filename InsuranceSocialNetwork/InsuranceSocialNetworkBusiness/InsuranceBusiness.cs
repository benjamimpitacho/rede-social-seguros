//using AutoMapper;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDAL;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Chat;
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
                    .ForMember(dest => dest.User, opts => opts.MapFrom(src => src.AspNetUsers));
                cfg.CreateMap<UserProfileDTO, Profile>();

                cfg.CreateMap<Notification, NotificationDTO>();
                cfg.CreateMap<NotificationDTO, Notification>();

                cfg.CreateMap<NotificationType, NotificationTypeDTO>();
                cfg.CreateMap<NotificationTypeDTO, NotificationType>();

                cfg.CreateMap<Post, PostDTO>()
                    .ForMember(dest => dest.PostOwner, opts => opts.MapFrom(src => src.AspNetUsers.Profile.FirstOrDefault()));
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
                cfg.CreateMap<UserDTO, AspNetUsers>()
                    .ForMember(dto => dto.Chat, opt => opt.Ignore())
                    .ForMember(dto => dto.ChatMember, opt => opt.Ignore())
                    .ForMember(dto => dto.ChatMessage, opt => opt.Ignore());

                cfg.CreateMap<Chat, ChatDTO>();
                cfg.CreateMap<ChatDTO, Chat>();

                cfg.CreateMap<ChatMember, ChatMemberDTO>();
                cfg.CreateMap<ChatMemberDTO, ChatMember>();

                cfg.CreateMap<ChatMessage, ChatMessageDTO>();
                cfg.CreateMap<ChatMessageDTO, ChatMessage>();

                cfg.CreateMap<Garage, GarageDTO>();
                cfg.CreateMap<GarageDTO, Garage>();

                cfg.CreateMap<MedicalClinic, MedicalClinicDTO>();
                cfg.CreateMap<MedicalClinicDTO, MedicalClinic>();

                cfg.CreateMap<Banner, BannerDTO>();
                cfg.CreateMap<BannerDTO, Banner>();
                cfg.CreateMap<BannerType, BannerTypeDTO>();
                cfg.CreateMap<BannerTypeDTO, BannerType>();
            });

            #endregion
        }

        #region Roles

        public List<RoleDTO> GetRoles()
        {
            List<AspNetRoles> roles = RoleRepository.GetRoles();
            return AutoMapper.Mapper.Map<List<RoleDTO>>(roles);
        }

        public List<RoleDTO> GetRolesFunctionalitiesMatrix()
        {
            List<AspNetRoles> roles = RoleRepository.GetRoles();
            List<string> functionalityNames = FunctionalityRepository.GetFunctionalityNames();
            List<AspNetRolesFunctionalities> functionalities = FunctionalityRepository.GetFunctionalities();

            return AutoMapper.Mapper.Map<List<RoleDTO>>(roles);
        }

        public List<RoleDTO> GetRegisterRoles()
        {
            List<string> registerRoles = new List<string>()
            {
                "NORMAL_USER"
                //,"INSURANCE_PROFESSIONAL"
                //,"ASSOCIATED_PREMIUM"
            };
            List<AspNetRoles> roles = RoleRepository.GetRoles().Where(i => registerRoles.Contains(i.Name)).OrderBy(i => i.Id).ToList();
            return AutoMapper.Mapper.Map<List<RoleDTO>>(roles);
        }

        #endregion Roles

        #region Profile

        public UserProfileDTO GetUserProfile(string Id)
        {
            Profile item = UserProfileRepository.GetProfile(Id);
            UserProfileDTO profile = AutoMapper.Mapper.Map<UserProfileDTO>(item);
            profile.Role = AutoMapper.Mapper.Map<RoleDTO>(item.AspNetUsers.AspNetRoles.FirstOrDefault());
            return profile;
        }

        public UserProfileDTO GetUserProfile(long Id)
        {
            Profile item = UserProfileRepository.GetProfile(Id);
            UserProfileDTO profile = AutoMapper.Mapper.Map<UserProfileDTO>(item);
            profile.Role = AutoMapper.Mapper.Map<RoleDTO>(item.AspNetUsers.AspNetRoles.FirstOrDefault());
            return profile;
        }

        public string GetUserIdFromProfileId(long profileId)
        {
            return UserProfileRepository.GetProfile(profileId).ID_User;
        }

        //public UserProfileDTO GetUser(string username)
        //{
        //    AspNetUsers user = UserProfileRepository.GetUser(username);
        //    return AutoMapper.Mapper.Map<List<UserProfileDTO>>(users);
        //}

        public bool IsUserAuthorizedToFunctionality(string username, string functionality)
        {
            return UserProfileRepository.IsUserAuthorizedToFunctionality(username, functionality);
        }

        public List<UserProfileDTO> GetUsers()
        {
            List<Profile> users = UserProfileRepository.GetProfiles();
            if (users.Count > 0)
                users[0].AspNetUsers.EmailConfirmed = false;
            return AutoMapper.Mapper.Map<List<UserProfileDTO>>(users);
        }

        public long CreateDefaultUserProfile(string Id, string UserName, string Email, string fName, string lName)
        {
            string trimmedName = fName.Trim();
            string firstName = !string.IsNullOrEmpty(fName) ? trimmedName : string.Empty;
            string lastName = !string.IsNullOrEmpty(lName) ? lName.Trim() : string.Empty;
            if (trimmedName.Contains(" ") && string.IsNullOrEmpty(lName))
            {
                firstName = trimmedName.Substring(0, trimmedName.IndexOf(" "));
                lastName = trimmedName.Substring(trimmedName.IndexOf(" ") + 1);
            }
            return UserProfileRepository.CreateDefaultProfile(AutoMapper.Mapper.Map<Profile>(new UserProfileDTO() { ID_User = Id, ContactEmail = Email, FirstName = firstName, LastName = lastName }));
        }

        public bool UpdateProfile(UserProfileDTO profile)
        {
            return UserProfileRepository.UpdateUserProfile(AutoMapper.Mapper.Map<Profile>(profile));
        }

        public bool DeleteUserProfile(long userId)
        {
            return UserProfileRepository.DeleteUserProfile(userId);
        }

        #endregion Profile

        #region Messages / Chats

        public ChatDTO GetChat(string userId, string userId2)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Chat chat = ChatRepository.GetChat(context, userId, userId2);

                if (null == chat)
                {
                    DateTime currentTime = DateTime.Now;
                    chat = new Chat()
                    {
                        ID_ChatCreator_User = userId,
                        CreateDate = currentTime,
                        LastChangeDate = currentTime,
                        ChatMember = new List<ChatMember>()
                        {
                            new ChatMember() {ID_User = userId, CreateDate=currentTime, LastChangeDate = currentTime, Active = true },
                            new ChatMember() {ID_User = userId2, CreateDate=currentTime, LastChangeDate = currentTime, Active = true }
                        }
                    };

                    ChatRepository.CreateChat(context, chat);
                }

                return AutoMapper.Mapper.Map<ChatDTO>(chat);
            }
        }

        public ChatDTO GetChat(long chatId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Chat chat = ChatRepository.GetChat(context, chatId);

                return AutoMapper.Mapper.Map<ChatDTO>(chat);
            }
        }

        public List<ChatDTO> GetChats(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Chat> chats = ChatRepository.GetChats(context, userId);

                return AutoMapper.Mapper.Map<List<ChatDTO>>(chats);
            }
        }

        public void SaveMessage(string userId, string chatId, string message)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                DateTime currentTime = DateTime.Now;

                Chat chat = context.Chat.Fetch().FirstOrDefault(i => i.ID_Chat == chatId);

                if (null == chat)
                    return;

                AspNetUsers user = context.AspNetUsers.Fetch().FirstOrDefault(i => i.Id == userId);

                if (null == user)
                    return;

                ChatMessage chatMessage = new ChatMessage()
                {
                    Active = true,
                    ID_Chat = chat.ID,
                    ID_User = user.Id,
                    CreateDate = currentTime,
                    LastChangeDate = currentTime,
                    Text = message
                };

                context.ChatMessage.Create(chatMessage);
                context.Save();
            }
        }

        #endregion Messages / Chats

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

        public bool CreateNotification(string toUserId, string fromUserId, NotificationTypeEnum type)
        {
            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                ToUserID = toUserId,
                FromUserID = string.IsNullOrEmpty(fromUserId) ? null : fromUserId
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

        public List<PostDTO> GetUserRelatedPosts(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetUserRelatedPosts(context, Id);

                //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
                //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
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

        #region Banners

        public BannerDTO GetBanner(long id)
        {
            Banner banner = BannerRepository.GetBanner(id);
            return AutoMapper.Mapper.Map<BannerDTO>(banner);
        }

        public List<BannerDTO> GetBanners()
        {
            List<Banner> banners = BannerRepository.GetBanners();
            return AutoMapper.Mapper.Map<List<BannerDTO>>(banners);
        }

        public List<BannerDTO> GetActiveBanners(BannerTypeEnum bannerType)
        {
            List<Banner> banners = BannerRepository.GetActiveBanners(bannerType);
            return AutoMapper.Mapper.Map<List<BannerDTO>>(banners);
        }

        public List<BannerTypeDTO> GetBannerTypes()
        {
            List<BannerType> bannerTypes = BannerRepository.GetBannerTypes();
            return AutoMapper.Mapper.Map<List<BannerTypeDTO>>(bannerTypes);
        }

        public bool CreateBanner(BannerDTO item)
        {
            Banner banner = AutoMapper.Mapper.Map<Banner>(item);
            return BannerRepository.CreateBanner(banner);
        }

        public bool EditBanner(BannerDTO item)
        {
            Banner banner = AutoMapper.Mapper.Map<Banner>(item);
            return BannerRepository.EditBanner(banner);
        }

        public bool DeleteBanner(long id)
        {
            return BannerRepository.DeleteBanner(id);
        }

        #endregion Banners

        #region Search Users

        public List<UserProfileDTO> SearchUsers(string searchTerm, long currentUserId)
        {
            List<Profile> users = UserProfileRepository.SearchProfiles(searchTerm, currentUserId);
            return AutoMapper.Mapper.Map<List<UserProfileDTO>>(users);
        }

        #endregion Search Users

        #region Friends

        public List<long> GetFriendsIDs(long currentUserId)
        {
            return UserProfileRepository.GetUserFriendsIDs(currentUserId);
        }

        public List<UserProfileDTO> GetFriends(string currentUserId)
        {
            List<Profile> friends = UserProfileRepository.GetUserFriends(currentUserId);
            return AutoMapper.Mapper.Map<List<UserProfileDTO>>(friends);
        }

        public bool AddFriend(long currentUserId, long newFriendId)
        {
            return FriendsRepository.AddFriend(currentUserId, newFriendId);
        }

        #endregion Friends

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
