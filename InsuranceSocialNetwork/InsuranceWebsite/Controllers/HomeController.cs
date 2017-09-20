using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Utils;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static UserProfileDTO CurrentUser;

        public HomeController()
        {
        }

        public async Task<ActionResult> TestView(HomeViewModel model)
        {
            if (null == model)
            {
                model = new HomeViewModel();
            }

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public async Task<ActionResult> Index(HomeViewModel model)
        {
            if (null == model)
            {
                model = new HomeViewModel();
            }

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Your Privacy Policy page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult TermsConditions()
        {
            ViewBag.Message = "Your Terms & Conditions page.";

            return View();
        }

        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public async Task<ActionResult> Messages()
        {
            var model = new MessagesViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                    //model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
                    //model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(user.Id);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            //model.Chats = InsuranceBusiness.BusinessLayer.GetChats(CurrentUser.ID_User);

            return View(model);
        }

        /// <summary>
        /// Receives profile ID and returns view to send messages (with old messages loaded if exists
        /// </summary>
        /// <param name="id">Profile ID</param>
        /// <returns>Messages view</returns>
        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public async Task<ActionResult> SendMessage(long id)
        {
            try
            {
                var model = new MessagesViewModel();
                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id);
                    }
                    else
                    {
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                string secondUserId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);
                model.Chats = InsuranceBusiness.BusinessLayer.GetChats(CurrentUser.ID_User);
                model.ActiveChat = InsuranceBusiness.BusinessLayer.GetChat(model.Profile.User.Id, secondUserId);

                return View("Messages", model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public ActionResult LoadChat(long id)
        {
            ChatViewModel model = new ChatViewModel();

            model.Profile = CurrentUser;
            model.Chat = InsuranceBusiness.BusinessLayer.GetChat(id);

            return PartialView("Partial/ChatSectionView", model);
        }

        [FunctionalityAutorizeAttribute("NOTIFICATIONS_FUNCTIONALITY")]
        public async Task<ActionResult> Notifications(HomeViewModel model)
        {
            try
            {
                if (null == model)
                {
                    model = new HomeViewModel();
                }

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id);
                    }
                    else
                    {
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        [FunctionalityAutorizeAttribute("NOTIFICATIONS_FUNCTIONALITY")]
        public ActionResult LoadNotifications()
        {
            try
            {
                NotificationItemsViewModel model = new NotificationItemsViewModel();
                model.Profile = CurrentUser;
                model.Items = InsuranceBusiness.BusinessLayer.GetUserNotifications(CurrentUser.ID_User);

                return PartialView("Partial/NotificationsControl", model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ActionResult> Search(HomeViewModel model)
        {
            try
            {
                if (null == model)
                {
                    model = new HomeViewModel();
                }

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id);
                    }
                    else
                    {
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                model.SearchModel.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchModel.SearchTerm, CurrentUser.ID);
                model.SearchModel.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);
                model.IsProfileSearchView = true;

                return View("Index", model);

                //return View(model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        [HttpPost]
        public async Task<ActionResult> SearchUsers(HomeViewModel model)
        {
            try
            {
                if (null == model)
                {
                    model = new HomeViewModel();
                }

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id);
                    }
                    else
                    {
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                model.SearchModel.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchModel.SearchTerm, CurrentUser.ID);
                model.SearchModel.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);
                model.IsProfileSearchView = true;

                //return View("Search", model);
                return View("Index", model);
                //return PartialView("~/Views/Home/Partial/SearchResult.cshtml", model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        //[FunctionalityAutorizeAttribute("ADD_FRIEND_FUNCTIONALITY")]
        //public async Task<ActionResult> AddFriend(long id, string searchTerm)
        //{
        //    try
        //    {
        //        SearchViewModel model = new SearchViewModel();
        //        model.SearchTerm = searchTerm;

        //        if (null != this.User && this.User.Identity.IsAuthenticated)
        //        {
        //            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //            var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
        //            if (null != user)
        //            {
        //                FillModel(model, user.Id);
        //            }
        //            else
        //            {
        //                return RedirectToAction("LogOff", "Account");
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }

        //        if (InsuranceBusiness.BusinessLayer.AddFriend(CurrentUser.ID, id))
        //        {
        //            string friendId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);
        //            InsuranceBusiness.BusinessLayer.CreateNotification(friendId, NotificationTypeEnum.FRIEND_REQUEST_RECEIVED);
        //        }

        //        model.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchTerm, CurrentUser.ID);
        //        model.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);

        //        return View("Search", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public JsonResult AddFriend()
        //{
        //    return Json(new { ok = false, message = "" });
        //}

        [HttpPost]
        public JsonResult AddFriend(long id)
        {
            try
            {
                //SearchViewModel model = new SearchViewModel();
                //model.SearchTerm = searchTerm;

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = UserManager.FindByNameAsync(this.User.Identity.Name);
                    //if (null != user)
                    //{
                    //    FillModel(model, user.Id);
                    //}
                    //else
                    //{
                    //    return RedirectToAction("LogOff", "Account");
                    //}
                }
                else
                {
                    //return RedirectToAction("Login", "Account");
                    return Json(new { ok = false, message = "" });
                }

                if (InsuranceBusiness.BusinessLayer.AddFriend(CurrentUser.ID, id))
                {
                    string friendId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);
                    InsuranceBusiness.BusinessLayer.CreateNotification(friendId, CurrentUser.ID_User, NotificationTypeEnum.FRIEND_REQUEST_RECEIVED);
                    return Json(new { ok = true, mydata = friendId, message = "" });
                }

                //model.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchTerm, CurrentUser.ID);
                //model.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);

                //return View("Search", model);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message });
            }

            return Json(new { ok = false, message = "" });
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult NewPost(HomeViewModel model, string postContentTextarea, HttpPostedFileBase imgUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = model.Profile.ID_User,
                    Text = postContentTextarea,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.PERSONAL_POST
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                }

                InsuranceBusiness.BusinessLayer.CreatePost(newPost);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("LIKE_POST_FUNCTIONALITY")]
        public JsonResult LikePost(/*HomeViewModel model, */long postId/*, string userId*/)
        {
            InsuranceBusiness.BusinessLayer.LikePost(postId, CurrentUser.ID_User);

            //return RedirectToAction("Index");
            return Json(new { ok = true, message = "" });
        }

        [FunctionalityAutorizeAttribute("LIKE_POST_FUNCTIONALITY")]
        public JsonResult UnlikePost(/*HomeViewModel model,*/ long postId/*, string userId*/)
        {
            InsuranceBusiness.BusinessLayer.UnlikePost(postId, CurrentUser.ID_User);

            //return RedirectToAction("Index");
            return Json(new { ok = true, message = "" });
        }

        [FunctionalityAutorizeAttribute("TIMELINE_FUNCTIONALITY")]
        public ActionResult Posts()
        {
            PostItemsViewModel model = new PostItemsViewModel();
            model.Profile = CurrentUser;
            model.Items = InsuranceBusiness.BusinessLayer.GetUserRelatedPosts(CurrentUser.ID_User);
            return PartialView("Partial/PostsControl", model);
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("COMMENT_POST_FUNCTIONALITY")]
        public ActionResult NewComment(long postId, string postNewComment)
        {
            try
            {
                PostCommentDTO newComment = new PostCommentDTO()
                {
                    ID_Post = postId,
                    ID_User = CurrentUser.ID_User,
                    Text = postNewComment
                };

                InsuranceBusiness.BusinessLayer.CreateComment(newComment);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileInfo(long? id)
        {
            var model = new ProfileViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            model.OwnProfile = id.HasValue ? model.Profile.ID == id.Value : true;

            return View(model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileEdit(long id)
        {
            ProfileEditModel model = new ProfileEditModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            if(id != model.Profile.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            model.OwnProfile = id == model.Profile.ID;

            model.ID = model.Profile.ID;
            model.FirstName = model.Profile.FirstName;
            model.LastName = model.Profile.LastName;
            model.MobilePhone_1 = model.Profile.MobilePhone_1;
            model.MobilePhone_2 = model.Profile.MobilePhone_2;
            model.Telephone_1 = model.Profile.Telephone_1;
            model.Telephone_2 = model.Profile.Telephone_2;
            model.Address = model.Profile.Address;
            model.Birthdate = model.Profile.Birthdate;
            model.ContactEmail = model.Profile.ContactEmail;
            model.ProfilePhoto = model.Profile.ProfilePhoto;
            model.Website = string.IsNullOrEmpty(model.Profile.Website) ? model.Profile.Website : model.Profile.Website.ToLower();
            model.AboutMe = model.Profile.AboutMe;

            return View(model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileSave(ProfileEditModel model, HttpPostedFileBase fileUploaderControl)
        {
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            if (CurrentUser.ID != model.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            model.OwnProfile = CurrentUser.ID == model.ID;

            UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID);

            profile.AboutMe = model.AboutMe;
            profile.Address = model.Address;
            profile.Birthdate = model.Birthdate;
            profile.ContactEmail = model.ContactEmail;
            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.MobilePhone_1 = model.MobilePhone_1;
            profile.MobilePhone_2 = model.MobilePhone_2;
            if(null!= fileUploaderControl)
            {
                Bitmap resizedImage = ImageUtils.ResizeImage(Bitmap.FromStream(fileUploaderControl.InputStream), 250, 250);
                profile.ProfilePhoto = ImageUtils.ImageToByte(resizedImage);
                model.ProfilePhoto = profile.ProfilePhoto;
                model.Profile.ProfilePhoto = profile.ProfilePhoto;
                //using (var binaryReader = new BinaryReader(fileUploaderControl.InputStream))
                //{
                //    profile.ProfilePhoto = binaryReader.ReadBytes(fileUploaderControl.ContentLength);
                //    model.ProfilePhoto = profile.ProfilePhoto;
                //}
            }
            profile.Telephone_1 = model.Telephone_1;
            profile.Telephone_2 = model.Telephone_2;
            profile.Website = model.Website;

            InsuranceBusiness.BusinessLayer.UpdateProfile(profile);

            return View("ProfileInfo", model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileSettingsSave(ProfileEditModel model)
        {
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            if (CurrentUser.ID != model.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            model.OwnProfile = CurrentUser.ID == model.ID;

            //UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID);

            //profile.AboutMe = model.AboutMe;
            //profile.Address = model.Address;
            //profile.Birthdate = model.Birthdate;
            //profile.ContactEmail = model.ContactEmail;
            //profile.FirstName = model.FirstName;
            //profile.LastName = model.LastName;
            //profile.MobilePhone_1 = model.MobilePhone_1;
            //profile.MobilePhone_2 = model.MobilePhone_2;
            ////profile.ProfilePhoto = model.ProfilePhoto;
            //profile.Telephone_1 = model.Telephone_1;
            //profile.Telephone_2 = model.Telephone_2;
            //profile.Website = model.Website;

            //InsuranceBusiness.BusinessLayer.UpdateProfile(profile);

            return View("ProfileInfo", model);
        }

        //[FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        //public async Task<ActionResult> ProfileInfo(long userId)
        //{
        //    var model = new ProfileViewModel();
        //    model.OwnProfile = false;
        //    if (null != this.User && this.User.Identity.IsAuthenticated)
        //    {
        //        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
        //        if (null != user)
        //        {
        //            FillModel(model, user.Id);
        //        }
        //        else
        //        {
        //            return RedirectToAction("LogOff", "Account");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    return View(model);
        //}

        [FunctionalityAutorizeAttribute("FRIENDS_FUNCTIONALITY")]
        public async Task<ActionResult> Friends()
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            model.Friends = InsuranceBusiness.BusinessLayer.GetFriends(CurrentUser.ID_User);

            return View(model);
        }

        private void FillModel(ProfileViewModel model, string userId)
        {
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);
            CurrentUser = model.Profile;
            model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);
            if (model is HomeViewModel)
            {
                if (((HomeViewModel)model).IsPostsView)
                {
                    ((HomeViewModel)model).Posts = InsuranceBusiness.BusinessLayer.GetUserRelatedPosts(userId);
                }
                ((HomeViewModel)model).TopBanners = InsuranceBusiness.BusinessLayer.GetActiveBanners(BannerTypeEnum.WEB_PRINCIPAL_BANNER);
                ((HomeViewModel)model).SideBanners = InsuranceBusiness.BusinessLayer.GetActiveBanners(BannerTypeEnum.WEB_SECONDARY_BANNER);
            }
            //else if (model is HomeViewModel)
            //{
            //    ((HomeViewModel)model).Friends = InsuranceBusiness.BusinessLayer.GetFriends(userId);
            //}
            else if (model is MessagesViewModel)
            {
                ((MessagesViewModel)model).Chats = InsuranceBusiness.BusinessLayer.GetChats(userId);
            }
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();

            return View(model);
        }

        #region Search Operations

        [FunctionalityAutorizeAttribute("SEARCH_GARAGES_FUNCTIONALITY")]
        public ActionResult SearchGarages(HomeViewModel model)
        {
            try
            {
                model.IsPostsView = model.IsMessagesView = model.IsNotificationsView = false;
                model.IsSearchView = true;
                model.IsSearchGarages = true;

                // Perform Search

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            return View("Index", model);
        }

        [FunctionalityAutorizeAttribute("SEARCH_CLINICS_FUNCTIONALITY")]
        public ActionResult SearchClinics(HomeViewModel model)
        {
            try
            {
                model.IsPostsView = model.IsMessagesView = model.IsNotificationsView = false;
                model.IsSearchView = true;
                model.IsSearchClinics = true;

                // Perform Search

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            return View("Index", model);
        }

        [FunctionalityAutorizeAttribute("SEARCH_CONSTRUCTIONCOMPANIES_FUNCTIONALITY")]
        public ActionResult SearchConstructionCompanies(HomeViewModel model)
        {
            try
            {
                model.IsPostsView = model.IsMessagesView = model.IsNotificationsView = false;
                model.IsSearchView = true;
                model.IsSearchConstructionCompanies = true;

                // Perform Search

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            return View("Index", model);
        }

        [FunctionalityAutorizeAttribute("SEARCH_APPLIANCEREPAIR_FUNCTIONALITY")]
        public ActionResult SearchHomeApplianceRepair(HomeViewModel model)
        {
            try
            {
                model.IsPostsView = model.IsMessagesView = model.IsNotificationsView = false;
                model.IsSearchView = true;
                model.IsSearchHomeApplianceRepair = true;

                // Perform Search

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            return View("Index", model);
        }

        [FunctionalityAutorizeAttribute("SEARCH_INSURANCECONTACTS_FUNCTIONALITY")]
        public ActionResult SearchInsuranceContacts(HomeViewModel model)
        {
            try
            {
                model.IsPostsView = model.IsMessagesView = model.IsNotificationsView = false;
                model.IsSearchView = true;
                model.IsSearchInsuranceContacts = true;

                // Perform Search

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            return View("Index", model);
        }

        #endregion Search Operations

        [FunctionalityAutorizeAttribute("SETTINGS_FUNCTIONALITY")]
        public async Task<ActionResult> Settings()
        {
            var model = new ProfileEditModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id);
                }
                else
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            model.OwnProfile = true;

            return View(model);
        }
    }
}