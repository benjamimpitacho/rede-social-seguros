//using AutoMapper;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDAL;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Chat;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceSocialNetworkDTO.SystemSettings;
using InsuranceSocialNetworkDTO.PostalCode;
using System.Net;
using System.Net.Sockets;
using System.Xml;

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

                cfg.CreateMap<ProfileSettings, ProfileSettingsDTO>();
                cfg.CreateMap<ProfileSettingsDTO, ProfileSettings>();

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

                cfg.CreateMap<ChatNote, ChatNoteDTO>();
                cfg.CreateMap<ChatNoteDTO, ChatNote>();

                cfg.CreateMap<Banner, BannerDTO>();
                cfg.CreateMap<BannerDTO, Banner>();
                cfg.CreateMap<BannerType, BannerTypeDTO>();
                cfg.CreateMap<BannerTypeDTO, BannerType>();

                cfg.CreateMap<Payment, PaymentDTO>();
                cfg.CreateMap<PaymentDTO, Payment>();
                cfg.CreateMap<PaymentNotification, PaymentNotificationDTO>();
                cfg.CreateMap<PaymentNotificationDTO, PaymentNotification>();

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

                cfg.CreateMap<SystemSettings, SystemSettingsDTO>();
                cfg.CreateMap<SystemSettingsDTO, SystemSettings>();

                cfg.CreateMap<Parish, ParishDTO>();
                cfg.CreateMap<ParishDTO, Parish>();
                cfg.CreateMap<County, CountyDTO>();
                cfg.CreateMap<CountyDTO, County>();
                cfg.CreateMap<District, DistrictDTO>();
                cfg.CreateMap<DistrictDTO, District>();
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
            //return UserProfileRepository.GetProfile(profileId).ID_User;
            return UserProfileRepository.GetProfileId(profileId);
        }

        public long GetUserProfileIdFromId(string id)
        {
            //return UserProfileRepository.GetProfile(id).ID;
            return UserProfileRepository.GetProfileId(id);
        }

        public bool IsUserAuthorizedToFunctionality(string username, string functionality)
        {
            return UserProfileRepository.IsUserAuthorizedToFunctionality(username, functionality);
        }

        public bool IsUserInRoleByUsername(string username, string role)
        {
            return UserProfileRepository.IsUserInRoleByUsername(username, role);
        }

        public bool IsUserInRoleByUserID(string userId, string role)
        {
            return UserProfileRepository.IsUserInRoleByUserID(userId, role);
        }

        public List<UserProfileDTO> GetUsers()
        {
            List<Profile> users = UserProfileRepository.GetProfiles();
            //if (users.Count > 0)
            //    users[0].AspNetUsers.EmailConfirmed = false;
            return AutoMapper.Mapper.Map<List<UserProfileDTO>>(users);
        }

        public long CreateDefaultUserProfile(string Id, string UserName, string Email, string fName, string lName, string professionalNumber)
        {
            string trimmedName = fName.Trim();
            string firstName = !string.IsNullOrEmpty(fName) ? trimmedName : string.Empty;
            string lastName = !string.IsNullOrEmpty(lName) ? lName.Trim() : string.Empty;
            if (trimmedName.Contains(" ") && string.IsNullOrEmpty(lName))
            {
                firstName = trimmedName.Substring(0, trimmedName.IndexOf(" "));
                lastName = trimmedName.Substring(trimmedName.IndexOf(" ") + 1);
            }
            return UserProfileRepository.CreateDefaultProfile(AutoMapper.Mapper.Map<Profile>(new UserProfileDTO() { ID_User = Id, ContactEmail = Email, FirstName = firstName, LastName = lastName, ProfessionalNumber = professionalNumber }));
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

        public bool ActivateUser(string userId)
        {
            return UserProfileRepository.ActivateUser(userId);
        }

        public bool DeactivateUser(long profileId)
        {
            return UserProfileRepository.DeactivateUser(profileId);
        }

        public ProfileSettingsDTO GetUserProfileSettings(long Id)
        {
            ProfileSettings item = UserProfileRepository.GetProfileSettings(Id);
            return AutoMapper.Mapper.Map<ProfileSettingsDTO>(item);
        }

        public bool UpdateProfileSetting(ProfileSettingsDTO profileSettings)
        {
            return UserProfileRepository.UpdateUserProfileSettings(AutoMapper.Mapper.Map<ProfileSettings>(profileSettings));
        }

        #endregion Profile

        #region Payment

        public List<ListItem> GetPaymentTypes()
        {
            return PaymentRepository.GetPaymentTypes();
        }

        public long GetPaymentTypeID(PaymentTypeEnum type)
        {
            return PaymentRepository.GetPaymentType(type.ToString()).ID;
        }

        public long GetPaymentStatusID(PaymentStatusEnum status)
        {
            return PaymentRepository.GetPaymentStatus(status.ToString()).ID;
        }

        public PaymentDTO GetPayment(string paymentId)
        {
            return AutoMapper.Mapper.Map<PaymentDTO>(PaymentRepository.GetPayment(paymentId));
        }

        public PaymentDTO GetPayment(string entity, string reference)
        {
            return AutoMapper.Mapper.Map<PaymentDTO>(PaymentRepository.GetPayment(entity, reference));
        }

        public PaymentDTO GetPayment(long id)
        {
            return AutoMapper.Mapper.Map<PaymentDTO>(PaymentRepository.GetPayment(id));
        }

        public PaymentDTO GetPaymentByUserAndType(long id, PaymentTypeEnum paymentType, CompanyTypeEnum companyType = CompanyTypeEnum.NONE)
        {
            return AutoMapper.Mapper.Map<PaymentDTO>(PaymentRepository.GetPayment(id, paymentType.ToString(), companyType));
        }

        public long CreatePayment(PaymentDTO payment)
        {
            return PaymentRepository.CreatePayment(AutoMapper.Mapper.Map<Payment>(payment));
        }

        public bool UpdatePayment(PaymentDTO payment)
        {
            return PaymentRepository.EditPayment(AutoMapper.Mapper.Map<Payment>(payment));
        }

        public PaymentNotificationDTO GetPaymentNotification(long id)
        {
            return AutoMapper.Mapper.Map<PaymentNotificationDTO>(PaymentRepository.GetPaymentNotification(id));
        }

        public long CreatePaymentNotification(string ep_cin, string ep_user, string ep_doc, string ep_type)
        {
            return PaymentRepository.CreatePaymentNotification(new PaymentNotification()
            {
                ep_cin = ep_cin,
                ep_user = ep_user,
                ep_doc = ep_doc,
                ep_type = ep_type
            });
        }

        public bool UpdatePaymentNotification(PaymentNotificationDTO paymentNotification)
        {
            return PaymentRepository.EditPaymentNotification(AutoMapper.Mapper.Map<PaymentNotification>(paymentNotification));
        }

        //public bool ConfirmEasypayPayment(PaymentDTO paymentDto)
        //{
        //    Payment payment = PaymentRepository.GetPayment(paymentDto.ID);
        //    PaymentStatus status = PaymentRepository.GetPaymentStatus(PaymentStatusEnum.PAYED.ToString());

        //    payment.PaymentDate = DateTime.Now;
        //    payment.PaymentStatus = status;
        //    payment.ID_PaymentStatus = status.ID;
        //    payment.ep_key = paymentDto.ep_key;
        //    payment.ep_doc = paymentDto.ep_doc;

        //    return PaymentRepository.EditPayment(payment);
        //}

        #endregion

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

        public bool UpdateCompaniesWorkingWith(string userId, long[] companies, CompanyTypeEnum type)
        {
            return CompanyRepository.UpdateCompaniesWorkingWith(userId, companies, type);
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

                    chat = ChatRepository.CreateChat(context, chat);
                }

                //ChatDTO chatDTO = AutoMapper.Mapper.Map<ChatDTO>(ChatRepository.GetChat(context, userId, userId2));
                ChatDTO chatDTO = AutoMapper.Mapper.Map<ChatDTO>(chat);
                chatDTO.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chat.ChatMember.Select(i => i.AspNetUsers.Profile.FirstOrDefault()).ToList());
                chatDTO.HasNotebook = (null != chat.ChatNote && null != chat.ChatNote.FirstOrDefault(i => i.ID_User == userId));
                chatDTO.NotificationsEnabled = (null == chat.ChatNotification || null == chat.ChatNotification.FirstOrDefault(i => i.ID_User == userId) || chat.ChatNotification.FirstOrDefault(i => i.ID_User == userId).ReceiveNotifications);

                return chatDTO;
            }
        }

        public ChatDTO GetChat(long chatId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Chat chat = ChatRepository.GetChat(context, chatId, userId);

                ChatDTO chatDTO = AutoMapper.Mapper.Map<ChatDTO>(chat);
                chatDTO.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chat.ChatMember.Select(i => i.AspNetUsers.Profile.FirstOrDefault()).ToList());
                chatDTO.HasNotebook = (null != chat.ChatNote && null != chat.ChatNote.FirstOrDefault(i => i.ID_User == userId));
                chatDTO.NotificationsEnabled = (null == chat.ChatNotification || null == chat.ChatNotification.FirstOrDefault(i => i.ID_User == userId) || chat.ChatNotification.FirstOrDefault(i => i.ID_User == userId).ReceiveNotifications);

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
                result.ForEach(i => i.HasUnreadMessages = i.ChatMessage.Exists(j => j.ID_User != userId && null == j.ReadDate));

                return result;
            }
        }

        public List<ChatDTO> SearchChats(string userId, string searchTerm)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Chat> chats = ChatRepository.SearchChats(context, userId, searchTerm);

                List<ChatDTO> result = AutoMapper.Mapper.Map<List<ChatDTO>>(chats);

                result.ForEach(i => i.ChatMemberProfile = AutoMapper.Mapper.Map<List<UserProfileDTO>>(chats.FirstOrDefault(c => c.ID == i.ID).ChatMember.Select(m => m.AspNetUsers.Profile.FirstOrDefault()).ToList()));
                result.ForEach(i => i.HasUnreadMessages = i.ChatMessage.Exists(j => j.ID_User != userId && null == j.ReadDate));

                return result;
            }
        }

        public void SaveMessage(string userId, string chatId, string message, bool isImage, bool isFile)
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
                    Text = message,
                    IsImage = isImage,
                    IsFile = isFile,
                    FileStream = (isImage || isFile) ? message : string.Empty
                };

                context.ChatMessage.Create(chatMessage);
                context.Save();
            }
        }

        public int GetTotalUnreadMessages(string Id)
        {
            return ChatRepository.GetTotalUnreadMessages(Id);
        }

        public void MarkAllChatMessagesRead(long chatId, string userId)
        {
            ChatRepository.MarkAllChatMessagesRead(chatId, userId);
        }
        
        public bool DeleteChat(long postId, string userId)
        {
            return ChatRepository.DeleteChat(postId, userId);
        }

        public bool MarkChatAsUnread(long postId, string userId)
        {
            return ChatRepository.MarkChatAsUnread(postId, userId);
        }

        public bool ChangeChatNotificationsState(long postId, string userId, bool state)
        {
            return ChatRepository.ChangeChatNotificationsState(postId, userId, state);
        }

        public bool CreateNote(long ID_Chat, string ID_User, string text)
        {
            ChatNote note = new ChatNote();
            note.ID_Chat = ID_Chat;
            note.ID_User = ID_User;
            note.Note = text;

            return ChatRepository.CreateNote(note).ID > 0;
        }

        public bool DeleteNote(long noteId, string ID_User)
        {
            return ChatRepository.DeleteNote(noteId, ID_User);
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

            List<ChatNotification> chatNotificationState = ChatRepository.GetChatNotificationsState(chatId);

            foreach(string userId in chatMembersUserIds)
            {
                if(chatNotificationState.Exists(i=>i.ID_User == userId && !i.ReceiveNotifications))
                {
                    continue;
                }
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

        public bool CreateNotificationForPaymentDone(NotificationTypeEnum type, bool isCompany, long profileId, long paymentId, CompanyTypeEnum companyType = CompanyTypeEnum.NONE)
        {
            Notification notification = new Notification()
            {
                Active = true,
                CreateDate = DateTime.Now,
                Read = false,
                ID_NotificationType = NotificationRepository.GetNotificationType(type.ToString()).ID,
                ToUserID = UserProfileRepository.GetProfileId(profileId)
            };

            NotificationRepository.CreateNotification(notification);

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

        //public List<PostDTO> GetUserPosts(string Id)
        //{
        //    List<Post> list = PostRepository.GetUserPosts(Id);

        //    list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
        //    list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


        //    return AutoMapper.Mapper.Map<List<PostDTO>>(list);
        //}

        public List<PostDTO> GetUserRelatedPosts(string Id, int skipInverval = 0, int itemsCount = 10)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                //InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Id, "InsuranceBusiness::GetUserRelatedPosts::GetUserRelatedPosts", string.Format("Get posts - start: {0}", DateTime.Now.ToString()));
                List<Post> list = PostRepository.GetUserRelatedPosts(context, Id, skipInverval, itemsCount);
                //InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Id, "InsuranceBusiness::GetUserRelatedPosts::GetUserRelatedPosts", string.Format("Get posts - start: {0}", DateTime.Now.ToString()));

                //InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Id, "InsuranceBusiness::GetUserRelatedPosts::Map", string.Format("Get posts - start: {0}", DateTime.Now.ToString()));
                List<PostDTO> result = AutoMapper.Mapper.Map<List<PostDTO>>(list);
                //InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Id, "InsuranceBusiness::GetUserRelatedPosts::Map", string.Format("Get posts - start: {0}", DateTime.Now.ToString()));

                //result.Where(i => i.IsRepost && i.Repost_PostID.HasValue).ToList().ForEach(i => i.ImageSource = null);

                return result;
            }
        }

        //public List<PostDTO> GetUserPostsOnly(string Id)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        List<Post> list = PostRepository.GetUserPostsOnly(context, Id);

        //        //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
        //        //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


        //        return AutoMapper.Mapper.Map<List<PostDTO>>(list);
        //    }
        //}

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

        public List<PostDTO> SearchCurrentDiscussionPosts(string Id, string searchTerm)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.SearchPosts(context, Id, searchTerm, PostSubjectEnum.CURRENT_DISCUSSION_POST);

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

        public List<PostDTO> SearchASFPosts(string searchTerm)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.SearchPosts(context, string.Empty, searchTerm, PostSubjectEnum.ASF_POST);

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

        public List<PostDTO> SearchAPROSEPosts(string searchTerm)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.SearchPosts(context, string.Empty, searchTerm, PostSubjectEnum.APROSE_POST);

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

        public List<PostDTO> SearchAPSPosts(string searchTerm)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> list = PostRepository.SearchPosts(context, string.Empty, searchTerm, PostSubjectEnum.APS_POST);

                return AutoMapper.Mapper.Map<List<PostDTO>>(list);
            }
        }

        public List<PostDTO> GetHumanResourcesPosts(string Id, PostSubjectEnum? type)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> posts;
                if (!type.HasValue)
                {
                    posts = PostRepository.GetPosts(context, Id, PostSubjectEnum.HUMAN_RESOURCES_TALENT_POST);
                }
                else
                {
                    posts = PostRepository.GetPosts(context, Id, type.Value);
                }

                return AutoMapper.Mapper.Map<List<PostDTO>>(posts);
            }
        }

        public List<PostDTO> GetInsuranceBusinessesPosts(string Id, PostSubjectEnum? type)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> posts;
                if (!type.HasValue)
                {
                    posts = PostRepository.GetPosts(context, Id, PostSubjectEnum.INSURANCE_BUSINESS_BUY_SELL_POST);
                }
                else
                {
                    posts = PostRepository.GetPosts(context, Id, type.Value);
                }

                return AutoMapper.Mapper.Map<List<PostDTO>>(posts);
            }
        }

        public List<PostDTO> GetInsuranceBusinessePosts(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<Post> buySellList = PostRepository.GetPosts(context, Id, PostSubjectEnum.INSURANCE_BUSINESS_BUY_SELL_POST);
                List<Post> partnershipsList = PostRepository.GetPosts(context, Id, PostSubjectEnum.INSURANCE_BUSINESS_PARTNERSHIP_POST);

                //list.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).ToList());
                //list.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());


                return AutoMapper.Mapper.Map<List<PostDTO>>(buySellList.Concat(partnershipsList).OrderByDescending(i=>i.CreateDate));
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
            if(IsUserInRoleByUserID(userId, RoleEnum.ADMINISTRATOR.ToString()))
            {
                return PostRepository.DeletePostAsAdmin(postId);
            }
            return PostRepository.DeletePost(postId, userId);
        }

        public bool HidePost(long postId, string userId)
        {
            return PostRepository.HidePost(postId, userId);
        }

        public bool ReportPost(long postId, string userId, string reason)
        {
            return PostRepository.ReportPost(postId, userId, reason);
        }

        public int GetTotalLikes(string userId)
        {
            return PostRepository.GetTotalLikes(userId);
        }

        public List<ListItem> GetSubjectTypes()
        {
            return PostRepository.GetSubjectTypeList();
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

        public bool HasPendingFriendRequested(string currentUserId, string otherUserId)
        {
            return FriendsRepository.HasPendingFriendRequested(currentUserId, otherUserId);
        }

        public int GetTotalFriends(string userId)
        {
            return FriendsRepository.GetTotalFriends(userId);
        }

        #endregion Friends

        #region Company

        public bool EditCompany(CompanyDTO item)
        {
            switch(item.CompanyType)
            {
                case CompanyTypeEnum.GARAGE:
                    return EditGarage(item);
                case CompanyTypeEnum.MEDICAL_CLINIC:
                    return EditMedicalClinic(item);
                case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                    return EditConstructionCompany(item);
                case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                    return EditHomeApplianceRepair(item);
                case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                    return EditInsuranceCompanyContact(item);
                default:
                    return false;
            }
        }

        public List<ListItem> GetInsuranceCompaniesWorkingWith(string userId)
        {
            return CompanyRepository.GetInsuranceCompaniesWorkingWith(userId).Select(i => new ListItem() { Key = i.ID, Value = i.BusinessName }).ToList();
        }

        public CompanyDTO GetCompany(string userId)
        {
            var company = GarageRepository.GetGarage(userId);

            if(null != company)
            {
                CompanyDTO companyDTO = AutoMapper.Mapper.Map<CompanyDTO>(company);
                companyDTO.CompanyType = CompanyTypeEnum.GARAGE;
                return companyDTO;
            }

            return null;
        }

        #endregion Company

        #region Garages

        public CompanyDTO GetGarage(long id)
        {
            Garage item = GarageRepository.GetGarage(id);
            CompanyDTO company = AutoMapper.Mapper.Map<CompanyDTO>(item);
            company.CompanyType = CompanyTypeEnum.GARAGE;
            return company;
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
            CompanyDTO company = AutoMapper.Mapper.Map<CompanyDTO>(item);
            company.CompanyType = CompanyTypeEnum.MEDICAL_CLINIC;
            return company;
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
            CompanyDTO company = AutoMapper.Mapper.Map<CompanyDTO>(item);
            company.CompanyType = CompanyTypeEnum.CONSTRUCTION_COMPANY;
            return company;
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
            CompanyDTO company = AutoMapper.Mapper.Map<CompanyDTO>(item);
            company.CompanyType = CompanyTypeEnum.HOME_APPLIANCES_REPAIR;
            return company;
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
            CompanyDTO company = AutoMapper.Mapper.Map<CompanyDTO>(item);
            company.CompanyType = CompanyTypeEnum.INSURANCE_COMPANY_CONTACT;
            return company;
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

        public List<ListItem> GetParishesByCounties(long countyId)
        {
            return PostalCodeRepository.GetParishListByCounty(countyId);
        }

        public ParishDTO GetParish(long id)
        {
            return AutoMapper.Mapper.Map<ParishDTO>(PostalCodeRepository.GetParish(id));
        }

        public CountyDTO GetCounty(long id)
        {
            return AutoMapper.Mapper.Map<CountyDTO>(PostalCodeRepository.GetCounty(id));
        }

        public DistrictDTO GetDistrict(long id)
        {
            return AutoMapper.Mapper.Map<DistrictDTO>(PostalCodeRepository.GetDistrict(id));
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

        #region System Settings

        public SystemSettingsDTO GetSystemSetting(SystemSettingsEnum setting)
        {
            SystemSettings settingObject = SystemSettingsRepository.Get(setting.ToString());
            return AutoMapper.Mapper.Map<SystemSettingsDTO>(settingObject);
        }

        public SystemSettingsDTO GetSystemSetting(string settingToken)
        {
            SystemSettings settingObject = SystemSettingsRepository.Get(settingToken);
            return AutoMapper.Mapper.Map<SystemSettingsDTO>(settingObject);
        }

        public void SetSystemSetting(SystemSettingsEnum setting, string value)
        {
            SystemSettingsRepository.Set(setting.ToString(), value);
        }

        #endregion System Settings

        #region System Logs

        public void Log(SystemLogLevelEnum level, string idUser, string title, string message)
        {
            try
            {
                SystemLogRepository.Log(level.ToString(), string.Format("{0} [app ip:{1}]", idUser, GetLocalIPAddress()), title, message);
            }
            catch (Exception ex) { }
        }

        public void LogException(string idUser, string title, Exception ex)
        {
            try
            {
                string entireMessageWithStack = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                while(null != ex.InnerException)
                {
                    ex = ex.InnerException;
                    entireMessageWithStack += string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                }
                SystemLogRepository.Log(SystemLogLevelEnum.ERROR.ToString(), string.Format("{0} [app ip:{1}]", idUser, GetLocalIPAddress()), title, entireMessageWithStack);
            }
            catch (Exception) { }
        }

        #endregion System Logs

        #region Timer Job Worker Tasks

        

        #endregion Timer Job Worker Tasks

        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "no-ip";
            }
            catch (Exception ex) {
                return "no-ip";
            }
        }
    }
}
