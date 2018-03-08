//using AutoMapper;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDAL;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Chat;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.PostalCode;
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

                cfg.CreateMap<Banner, BannerDTO>();
                cfg.CreateMap<BannerDTO, Banner>();
                cfg.CreateMap<BannerType, BannerTypeDTO>();
                cfg.CreateMap<BannerTypeDTO, BannerType>();

                cfg.CreateMap<Garage, CompanyDTO>()
                    .ForMember(dest => dest.IsFavorite,
                       opts => opts.MapFrom(src => src.GarageFavorite != null && src.GarageFavorite.Count > 0));
                cfg.CreateMap<CompanyDTO, Garage>();
                cfg.CreateMap<MedicalClinic, CompanyDTO>()
                    .ForMember(dest => dest.IsFavorite,
                       opts => opts.MapFrom(src => src.MedicalClinicFavorite != null && src.MedicalClinicFavorite.Count > 0));
                cfg.CreateMap<CompanyDTO, MedicalClinic>();
                cfg.CreateMap<ConstructionCompany, CompanyDTO>()
                    .ForMember(dest => dest.IsFavorite,
                       opts => opts.MapFrom(src => src.ConstructionCompanyFavorite != null && src.ConstructionCompanyFavorite.Count > 0));
                cfg.CreateMap<CompanyDTO, ConstructionCompany>();
                cfg.CreateMap<HomeApplianceRepair, CompanyDTO>()
                    .ForMember(dest => dest.IsFavorite,
                       opts => opts.MapFrom(src => src.HomeApplianceRepairFavorite != null && src.HomeApplianceRepairFavorite.Count > 0));
                cfg.CreateMap<CompanyDTO, HomeApplianceRepair>();
                cfg.CreateMap<InsuranceCompanyContact, CompanyDTO>()
                    .ForMember(dest => dest.IsFavorite,
                       opts => opts.MapFrom(src => src.InsuranceCompanyContactFavorite != null && src.InsuranceCompanyContactFavorite.Count > 0));
                cfg.CreateMap<CompanyDTO, InsuranceCompanyContact>();
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
                ,"INSURANCE_PROFESSIONAL"
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

        public long GetUserProfileIdFromId(string id)
        {
            return UserProfileRepository.GetProfile(id).ID;
        }

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

        public bool DeleteUserProfile(long profileId)
        {
            return UserProfileRepository.DeleteUserProfile(profileId);
        }

        public bool ActivateUser(long profileId)
        {
            return UserProfileRepository.ActivateUser(profileId);
        }

        public bool DeactivateUser(long profileId)
        {
            return UserProfileRepository.DeactivateUser(profileId);
        }

        #endregion Profile

        #region Authorized Emails for Automatic Approval


        public bool IsEmailAuthorizedForAutomaticApproval(string email)
        {
            return AuthorizedEmailRepository.IsEmailAuthorizedForAutomaticApproval(email);
        }

        public bool UpdateEmailAuthorizedForAutomaticApproval(string[] emailPatterns)
        {
            return AuthorizedEmailRepository.UpdateEmailAuthorizedForAutomaticApproval(emailPatterns);
        }

        public bool UpdateEmailAuthorizedForAutomaticApproval(string userId, string[] emailPatterns)
        {
            return AuthorizedEmailRepository.UpdateEmailAuthorizedForAutomaticApproval(userId, emailPatterns);
        }

        public List<string> GetAuthorizedEmailsForAutomaticApproval()
        {
            return AuthorizedEmailRepository.GetAuthorizedEmailsForAutomaticApproval().Select(i => i.Email).ToList();
        }

        public List<string> GetAuthorizedEmailsForAutomaticApproval(string userId)
        {
            return AuthorizedEmailRepository.GetAuthorizedEmailsForAutomaticApproval(userId).Select(i => i.Email).ToList();
        }

        #endregion Authorized Emails for Automatic Approval

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

                ChatDTO chatDTO = AutoMapper.Mapper.Map<ChatDTO>(ChatRepository.GetChat(context, userId, userId2));
                chatDTO.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chat.ChatMember.Select(i => i.AspNetUsers.Profile.FirstOrDefault()).ToList());

                return chatDTO;
            }
        }

        public ChatDTO GetChat(long chatId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Chat chat = ChatRepository.GetChat(context, chatId);

                ChatDTO chatDTO = AutoMapper.Mapper.Map<ChatDTO>(chat);
                chatDTO.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chat.ChatMember.Select(i => i.AspNetUsers.Profile.FirstOrDefault()).ToList());

                return chatDTO;
            }
        }

        public List<ChatDTO> GetChats(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Chat> chats = ChatRepository.GetChats(context, userId);

                List<ChatDTO> result = AutoMapper.Mapper.Map<List<ChatDTO>>(chats);

                result.ForEach(i => i.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chats.FirstOrDefault(c => c.ID == i.ID).ChatMember.Select(m => m.AspNetUsers.Profile.FirstOrDefault()).ToList()));

                return result;
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

        public int GetTotalUnreadMessages(string Id)
        {
            return ChatRepository.GetTotalUnreadMessages(Id);
        }

        #endregion Messages / Chats

        #region Notifications

        public List<NotificationDTO> GetUserNotifications(string Id)
        {
            List<Notification> list = NotificationRepository.GetUserNotifications(Id);
            List<NotificationDTO> result = AutoMapper.Mapper.Map<List<NotificationDTO>>(list);

            //result.usersNames = UserProfileRepository.GetNamesForUsers(list.Select(i=>i.FromUserID).Distinct().ToList());

            return result;
        }

        public List<ListItemString> GetNamesForUsers(List<string> usersIds)
        {
            return UserProfileRepository.GetNamesForUsers(usersIds);
        }

        public List<ListItemObject> GetPhotosForUsers(List<string> usersIds)
        {
            return UserProfileRepository.GetPhotosForUsers(usersIds);
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

        public bool CreateNotificationForAdministrators(string fromUserId, NotificationTypeEnum type)
        {
            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                //ToUserID = toUserId,
                FromUserID = string.IsNullOrEmpty(fromUserId) ? null : fromUserId
            };

            List<AspNetUsers> admins = UserProfileRepository.GetAdministratorsList();

            foreach(AspNetUsers admin in admins)
            {
                notification.ToUserID = admin.Id;
                NotificationRepository.CreateNotification(notification);
            }

            return true;
        }

        public bool CreateNotificationForPost(long postId, string fromUserId, NotificationTypeEnum type)
        {
            string postOwnerUserId = PostRepository.GetPostOwnerUserId(postId);

            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                ToUserID = postOwnerUserId,
                FromUserID = string.IsNullOrEmpty(fromUserId) ? null : fromUserId,
                ID_Post = postId
            };

            return NotificationRepository.CreateNotification(notification);
        }

        public bool CreateNotificationForChat(string chatId, string fromUserId, NotificationTypeEnum type)
        {
            List<string> chatMembersUserIds = ChatRepository.GetChatMembersUserIds(chatId, fromUserId);

            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                //ToUserID = postOwnerUserId,
                FromUserID = string.IsNullOrEmpty(fromUserId) ? null : fromUserId
            };

            foreach(string userId in chatMembersUserIds)
            {
                notification.ToUserID = userId;
                NotificationRepository.CreateNotification(notification);
            }

            return true;
        }

        public bool CreateNotificationForNewPost(NotificationTypeEnum type, long postId)
        {
            List<string> usersToNotify = UserProfileRepository.GetUsersToNotify();

            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                ID_Post = postId
                //ToUserID = postOwnerUserId,
                //FromUserID = string.IsNullOrEmpty(fromUserId) ? null : fromUserId
            };

            foreach (string userId in usersToNotify)
            {
                notification.ToUserID = userId;
                NotificationRepository.CreateNotification(notification);
            }

            return true;
        }

        public bool MarkNotificationAsRead(long id)
        {
            return NotificationRepository.MarkNotificationAsRead(id);
        }

        public bool MarkAllNotificationsAsRead(string id)
        {
            return NotificationRepository.MarkNotificationsAsRead(id);
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

        public List<PostDTO> GetUserPostsOnly(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetUserPostsOnly(context, Id);

                //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
                //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public List<PostDTO> GetCurrentDiscussionPosts(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetPosts(context, Id, PostSubjectEnum.CURRENT_DISCUSSION_POST);

                //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
                //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public List<PostDTO> GetASFPosts()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetPostsBySubject(context, PostSubjectEnum.ASF_POST);

                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public List<PostDTO> GetAPROSEPosts()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetPostsBySubject(context, PostSubjectEnum.APROSE_POST);

                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public List<PostDTO> GetAPSPosts()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.GetPostsBySubject(context, PostSubjectEnum.APS_POST);

                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public PostDTO GetPost(long postId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Post post = PostRepository.GetPost(context, postId);

                //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
                //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


                return AutoMapper.Mapper.Map<PostDTO>(post);
            }
        }

        public long CreatePost(PostDTO item)
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
                    Image = item.Image,
                    FileName = item.FileName,
                    FileExtension = item.FileExtension
                });
            }

            return PostRepository.CreatePost(post);
        }

        public long EditPost(PostDTO item)
        {
            Post post = AutoMapper.Mapper.Map<Post>(item);

            post.ID_PostType = PostRepository.GetPostType(item.Type.ToString()).ID;
            post.PostType = null;
            post.ID_PostSubject = PostRepository.GetPostSubject(item.Subject.ToString()).ID;
            post.PostSubject = null;

            if (null != item.Image)
            {
                post.PostImage = new List<PostImage>();
                post.PostImage.Add(new PostImage()
                {
                    Active = true,
                    ID_Post = post.ID,
                    //AspNetUsers = post.AspNetUsers,
                    Date = DateTime.Now,
                    //ID_User = post.ID_User,
                    Image = item.Image,
                    FileName = item.FileName,
                    FileExtension = item.FileExtension
                });
            }
            else
            {
                post.PostImage = null;
            }

            return PostRepository.EditPost(post);
        }

        public bool LikePost(long postId, string userId)
        {
            return PostRepository.LikePost(postId, userId);
        }

        public bool UnlikePost(long postId, string userId)
        {
            return PostRepository.UnlikePost(postId, userId);
        }

        public bool IsOwnPost(long postId, string userId)
        {
            return PostRepository.IsOwnPost(postId, userId);
        }

        public bool DeletePost(long postId, string userId)
        {
            return PostRepository.DeletePost(postId, userId);
        }

        public bool HidePost(long postId, string userId)
        {
            return PostRepository.HidePost(postId, userId);
        }

        public int GetTotalLikes(string userId)
        {
            return PostRepository.GetTotalLikes(userId);
        }

        #endregion Posts

        #region Comments

        public long CreateComment(PostCommentDTO item)
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

        public bool AcceptFriendRequest(string currentUserId, string newFriendId)
        {
            return FriendsRepository.AcceptFriendRequest(currentUserId, newFriendId);
        }

        public bool IgnoreFriendRequest(string currentUserId, string newFriendId)
        {
            return FriendsRepository.IgnoreFriendRequest(currentUserId, newFriendId);
        }

        public bool AreFriends(string currentUserId, string otherUserId)
        {
            return FriendsRepository.AreFriends(currentUserId, otherUserId);
        }

        public bool HasPendingFriendRequest(string currentUserId, string otherUserId)
        {
            return FriendsRepository.HasPendingFriendRequest(currentUserId, otherUserId);
        }

        public int GetTotalFriends(string userId)
        {
            return FriendsRepository.GetTotalFriends(userId);
        }

        #endregion Friends

        #region Garages

        public CompanyDTO GetGarage(long id)
        {
            Garage item = GarageRepository.GetGarage(id);
            return AutoMapper.Mapper.Map<CompanyDTO>(item);
        }

        public List<CompanyDTO> GetGarages()
        {
            List<Garage> items = GarageRepository.GetGarages();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(items);
        }

        public long CreateGarage(CompanyDTO item)
        {
            Garage garage = AutoMapper.Mapper.Map<Garage>(item);
            return GarageRepository.CreateGarage(garage);
        }

        public bool EditGarage(CompanyDTO item)
        {
            Garage garage = AutoMapper.Mapper.Map<Garage>(item);
            return GarageRepository.EditGarage(garage);
        }

        public bool DeleteGarage(long id)
        {
            return GarageRepository.DeleteGarage(id);
        }

        public bool ActivateGarage(long id)
        {
            return GarageRepository.ActivateGarage(id);
        }

        public bool DeactivateGarage(long id)
        {
            return GarageRepository.DeactivateGarage(id);
        }

        public List<ListItem> GetCompanyServices(CompanyTypeEnum type)
        {
            return CompanyRepository.GetCompanyServices(type);
        }

        #endregion Garages

        #region Medical Clinics

        public CompanyDTO GetMedicalClinic(long id)
        {
            MedicalClinic item = MedicalClinicRepository.GetMedicalClinic(id);
            return AutoMapper.Mapper.Map<CompanyDTO>(item);
        }

        public List<CompanyDTO> GetMedicalClinics()
        {
            List<MedicalClinic> items = MedicalClinicRepository.GetMedicalClinics();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(items);
        }

        public long CreateMedicalClinic(CompanyDTO item)
        {
            MedicalClinic clinic = AutoMapper.Mapper.Map<MedicalClinic>(item);
            return MedicalClinicRepository.CreateMedicalClinic(clinic);
        }

        public bool EditMedicalClinic(CompanyDTO item)
        {
            MedicalClinic clinic = AutoMapper.Mapper.Map<MedicalClinic>(item);
            return MedicalClinicRepository.EditMedicalClinic(clinic);
        }

        public bool DeleteMedicalClinic(long id)
        {
            return MedicalClinicRepository.DeleteMedicalClinic(id);
        }

        public bool ActivateMedicalClinic(long id)
        {
            return MedicalClinicRepository.ActivateMedicalClinic(id);
        }

        public bool DeactivateMedicalClinic(long id)
        {
            return MedicalClinicRepository.DeactivateMedicalClinic(id);
        }

        #endregion Medical Clinics

        #region Construction Companies

        public CompanyDTO GetConstructionCompany(long id)
        {
            ConstructionCompany item = ConstructionCompanyRepository.GetConstructionCompany(id);
            return AutoMapper.Mapper.Map<CompanyDTO>(item);
        }

        public List<CompanyDTO> GetConstructionCompanies()
        {
            List<ConstructionCompany> items = ConstructionCompanyRepository.GetConstructionCompanies();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(items);
        }

        public long CreateConstructionCompany(CompanyDTO item)
        {
            ConstructionCompany constructionCompany = AutoMapper.Mapper.Map<ConstructionCompany>(item);
            return ConstructionCompanyRepository.CreateConstructionCompany(constructionCompany);
        }

        public bool EditConstructionCompany(CompanyDTO item)
        {
            ConstructionCompany constructionCompany = AutoMapper.Mapper.Map<ConstructionCompany>(item);
            return ConstructionCompanyRepository.EditConstructionCompany(constructionCompany);
        }

        public bool DeleteConstructionCompany(long id)
        {
            return ConstructionCompanyRepository.DeleteConstructionCompany(id);
        }

        public bool ActivateConstructionCompany(long id)
        {
            return ConstructionCompanyRepository.ActivateConstructionCompany(id);
        }

        public bool DeactivateConstructionCompany(long id)
        {
            return ConstructionCompanyRepository.DeactivateConstructionCompany(id);
        }

        #endregion Construction Companies

        #region Home Appliances Repairs

        public CompanyDTO GetHomeApplianceRepair(long id)
        {
            HomeApplianceRepair item = HomeApplianceRepairRepository.GetHomeApplianceRepair(id);
            return AutoMapper.Mapper.Map<CompanyDTO>(item);
        }

        public List<CompanyDTO> GetHomeApplianceRepairs()
        {
            List<HomeApplianceRepair> items = HomeApplianceRepairRepository.GetHomeApplianceRepairs();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(items);
        }

        public long CreateHomeApplianceRepair(CompanyDTO item)
        {
            HomeApplianceRepair homeApplianceRepair = AutoMapper.Mapper.Map<HomeApplianceRepair>(item);
            return HomeApplianceRepairRepository.CreateHomeApplianceRepair(homeApplianceRepair);
        }

        public bool EditHomeApplianceRepair(CompanyDTO item)
        {
            HomeApplianceRepair homeApplianceRepair = AutoMapper.Mapper.Map<HomeApplianceRepair>(item);
            return HomeApplianceRepairRepository.EditHomeApplianceRepair(homeApplianceRepair);
        }

        public bool DeleteHomeApplianceRepair(long id)
        {
            return HomeApplianceRepairRepository.DeleteHomeApplianceRepair(id);
        }

        public bool ActivateHomeApplianceRepair(long id)
        {
            return HomeApplianceRepairRepository.ActivateHomeApplianceRepair(id);
        }

        public bool DeactivateHomeApplianceRepair(long id)
        {
            return HomeApplianceRepairRepository.DeactivateHomeApplianceRepair(id);
        }

        #endregion Home Appliances Repairs

        #region Insurance Company Contacts

        public CompanyDTO GetInsuranceCompanyContact(long id)
        {
            InsuranceCompanyContact item = InsuranceCompanyContactRepository.GetInsuranceCompanyContact(id);
            return AutoMapper.Mapper.Map<CompanyDTO>(item);
        }

        public List<CompanyDTO> GetInsuranceCompanyContacts()
        {
            List<InsuranceCompanyContact> items = InsuranceCompanyContactRepository.GetInsuranceCompanyContacts();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(items);
        }

        public long CreateInsuranceCompanyContact(CompanyDTO item)
        {
            InsuranceCompanyContact insuranceCompanyContact = AutoMapper.Mapper.Map<InsuranceCompanyContact>(item);
            return InsuranceCompanyContactRepository.CreateInsuranceCompanyContact(insuranceCompanyContact);
        }

        public bool EditInsuranceCompanyContact(CompanyDTO item)
        {
            InsuranceCompanyContact insuranceCompanyContact = AutoMapper.Mapper.Map<InsuranceCompanyContact>(item);
            return InsuranceCompanyContactRepository.EditInsuranceCompanyContact(insuranceCompanyContact);
        }

        public bool DeleteInsuranceCompanyContact(long id)
        {
            return InsuranceCompanyContactRepository.DeleteInsuranceCompanyContact(id);
        }

        public bool ActivateInsuranceCompanyContact(long id)
        {
            return InsuranceCompanyContactRepository.ActivateInsuranceCompanyContact(id);
        }

        public bool DeactivateInsuranceCompanyContact(long id)
        {
            return InsuranceCompanyContactRepository.DeactivateInsuranceCompanyContact(id);
        }

        #endregion Insurance Company Contacts

        #region Postal Code

        //public PostalCodeDTO GetPostalCodeInformation(string postalCode)
        //{
        //    int postalCodeValue = -1;
        //    int postalSubCodeValue = -1;
        //    string[] postalCodeSplitted = postalCode.Trim().Split(new char[] { '-' });

        //    bool postalCodeParsedCorrectly = true;
        //    if (postalCodeSplitted.Length > 1)
        //    {
        //        postalCodeParsedCorrectly = postalCodeParsedCorrectly && Int32.TryParse(postalCodeSplitted[0], out postalCodeValue);
        //        postalCodeParsedCorrectly = postalCodeParsedCorrectly && Int32.TryParse(postalCodeSplitted[1], out postalSubCodeValue);

        //        if(postalCodeParsedCorrectly)
        //        {
        //            PostalCode itemInfo = PostalCodeRepository.GetInformation(postalCodeValue, postalSubCodeValue);
        //            return AutoMapper.Mapper.Map<PostalCodeDTO>(itemInfo);
        //        }
        //    }
        //    else if (postalCodeSplitted.Length == 1)
        //    {
        //        postalCodeParsedCorrectly = postalCodeParsedCorrectly && Int32.TryParse(postalCodeSplitted[0], out postalCodeValue);

        //        if (postalCodeParsedCorrectly)
        //        {
        //            PostalCode itemInfo = PostalCodeRepository.GetInformation(postalCodeValue, null);
        //            return AutoMapper.Mapper.Map<PostalCodeDTO>(itemInfo);
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return null;
        //}

        public List<ListItem> GetDistricts()
        {
            return PostalCodeRepository.GetDistrictList();
        }

        public List<ListItem> GetCountiesByDistrict(long districtId)
        {
            return PostalCodeRepository.GetCountyListByDistrict(districtId);
        }

        #endregion Postal Code

        #region Search Operations
        
        public List<CompanyDTO> SearchGarages(CompanySearchFilterDTO searchFilter)
        {
            List<Garage> itemsList = CompanyRepository.SearchGarages(searchFilter);
            itemsList.ForEach(i => i.GarageFavorite = i.GarageFavorite.Where(j => j.ID_User == searchFilter.UserId).ToList());
            itemsList = itemsList.OrderBy(i => (null != i.GarageFavorite && i.GarageFavorite.Count > 0) ? 0 : 1).ToList();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(itemsList);
        }

        public List<CompanyDTO> SearchMedicalClinis(CompanySearchFilterDTO searchFilter)
        {

            List<MedicalClinic> itemsList = CompanyRepository.SearchMedicalClinics(searchFilter);
            itemsList.ForEach(i => i.MedicalClinicFavorite = i.MedicalClinicFavorite.Where(j => j.ID_User == searchFilter.UserId).ToList());
            itemsList = itemsList.OrderBy(i => (null != i.MedicalClinicFavorite && i.MedicalClinicFavorite.Count > 0) ? 0 : 1).ToList();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(itemsList);
        }

        public List<CompanyDTO> SearchConstructionCompanies(CompanySearchFilterDTO searchFilter)
        {
            List<ConstructionCompany> itemsList = CompanyRepository.SearchConstructionCompanies(searchFilter);
            itemsList.ForEach(i => i.ConstructionCompanyFavorite = i.ConstructionCompanyFavorite.Where(j => j.ID_User == searchFilter.UserId).ToList());
            itemsList = itemsList.OrderBy(i => (null != i.ConstructionCompanyFavorite && i.ConstructionCompanyFavorite.Count > 0) ? 0 : 1).ToList();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(itemsList);
        }

        public List<CompanyDTO> SearchHomeApplianceRepairs(CompanySearchFilterDTO searchFilter)
        {
            List<HomeApplianceRepair> itemsList = CompanyRepository.SearchHomeApplianceRepairs(searchFilter);
            itemsList.ForEach(i => i.HomeApplianceRepairFavorite = i.HomeApplianceRepairFavorite.Where(j => j.ID_User == searchFilter.UserId).ToList());
            itemsList = itemsList.OrderBy(i => (null != i.HomeApplianceRepairFavorite && i.HomeApplianceRepairFavorite.Count > 0) ? 0 : 1).ToList();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(itemsList);
        }

        public List<CompanyDTO> SearchInsuranceCompanyContacts(CompanySearchFilterDTO searchFilter)
        {
            List<InsuranceCompanyContact> itemsList = CompanyRepository.SearchInsuranceCompanyContacts(searchFilter);
            itemsList.ForEach(i => i.InsuranceCompanyContactFavorite = i.InsuranceCompanyContactFavorite.Where(j => j.ID_User == searchFilter.UserId).ToList());
            itemsList = itemsList.OrderBy(i => (null != i.InsuranceCompanyContactFavorite && i.InsuranceCompanyContactFavorite.Count > 0) ? 0 : 1).ToList();
            return AutoMapper.Mapper.Map<List<CompanyDTO>>(itemsList);
        }

        public bool AddFavoriteGarage(long companyId, string userId)
        {
            return CompanyRepository.AddFavoriteGarage(companyId, userId);
        }

        public bool AddFavoriteConstructionCompany(long companyId, string userId)
        {
            return CompanyRepository.AddFavoriteConstructionCompany(companyId, userId);
        }

        public bool AddFavoriteHomeAppliancesRepair(long companyId, string userId)
        {
            return CompanyRepository.AddFavoriteHomeApplianceRepair(companyId, userId);
        }

        public bool AddFavoriteInsuranceCompanyContact(long companyId, string userId)
        {
            return CompanyRepository.AddFavoriteInsuranceCompanyContact(companyId, userId);
        }

        public bool AddFavoriteMedicalClinic(long companyId, string userId)
        {
            return CompanyRepository.AddFavoriteMedicalClinic(companyId, userId);
        }

        public bool RemoveFavoriteGarage(long companyId, string userId)
        {
            return CompanyRepository.RemoveFavoriteGarage(companyId, userId);
        }

        public bool RemoveFavoriteConstructionCompany(long companyId, string userId)
        {
            return CompanyRepository.RemoveFavoriteConstructionCompany(companyId, userId);
        }

        public bool RemoveFavoriteHomeAppliancesRepair(long companyId, string userId)
        {
            return CompanyRepository.RemoveFavoriteHomeApplianceRepair(companyId, userId);
        }

        public bool RemoveFavoriteInsuranceCompanyContact(long companyId, string userId)
        {
            return CompanyRepository.RemoveFavoriteInsuranceCompanyContact(companyId, userId);
        }

        public bool RemoveFavoriteMedicalClinic(long companyId, string userId)
        {
            return CompanyRepository.RemoveFavoriteMedicalClinic(companyId, userId);
        }

        #endregion Search Operations
    }
}
