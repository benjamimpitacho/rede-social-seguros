using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Utils;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.SystemSettings;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static UserProfileDTO CurrentUser;

        public HomeController()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-PT");
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

        [Authorize]
        public async Task<ActionResult> PostDetail(long idPost, long? idNotification)
        {
            HomeViewModel model = new HomeViewModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            if(idNotification.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            }

            model.Posts = new List<PostDTO>() { InsuranceBusiness.BusinessLayer.GetPost(idPost) };

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

        //[AllowAnonymous]
        //public ActionResult PrivacyPolicy()
        //{
        //    ViewBag.Message = "Your Privacy Policy page.";

        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult TermsConditions()
        {
            ViewBag.Message = "Your Terms & Conditions page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult CookieUsagePolicy()
        {
            ViewBag.Message = "Your Cookie Usage Policy page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Your Privacy Policy page.";

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

        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public async Task<ActionResult> NewMessage()
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
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            model.IsNewMessage = true;

            return View("Messages", model);
        }

        /// <summary>
        /// Receives profile ID and returns view to send messages (with old messages loaded if exists
        /// </summary>
        /// <param name="id">Profile ID</param>
        /// <returns>Messages view</returns>
        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public async Task<ActionResult> SendMessage(long? id, long? ntId, string userId)
        {
            try
            {
                if (ntId.HasValue)
                {
                    InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
                }

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

                if(string.IsNullOrEmpty(userId) && id.HasValue)
                {
                    userId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id.Value);
                }
                model.ActiveChat = InsuranceBusiness.BusinessLayer.GetChat(model.Profile.User.Id, userId);
                model.Chats = InsuranceBusiness.BusinessLayer.GetChats(CurrentUser.ID_User);

                return View("Messages", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [FunctionalityAutorizeAttribute("MESSAGES_FUNCTIONALITY")]
        public ActionResult LoadChat(long id)
        {
            ChatViewModel model = new ChatViewModel();

            model.Profile = CurrentUser;
            model.Chat = InsuranceBusiness.BusinessLayer.GetChat(id, CurrentUser.ID_User);

            InsuranceBusiness.BusinessLayer.MarkAllChatMessagesRead(id, CurrentUser.ID_User);

            return PartialView("Partial/ChatSectionView", model);
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult DeleteChat(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeleteChat(id, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Messages");
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult MarkChatAsUnread(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.MarkChatAsUnread(id, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Messages");
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult DisableChatNotifications(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.ChangeChatNotificationsState(id, CurrentUser.ID_User, false);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Messages");
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult EnableChatNotifications(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.ChangeChatNotificationsState(id, CurrentUser.ID_User, true);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Messages");
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
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [FunctionalityAutorizeAttribute("NOTIFICATIONS_FUNCTIONALITY")]
        public async Task<ActionResult> MarkAllNotificationsAsRead(string id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.MarkAllNotificationsAsRead(id);

                return Redirect("../Notifications");

                //return View("Partial/NotificationsControl", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                model.UsersNames = InsuranceBusiness.BusinessLayer.GetNamesForUsers(model.Items.Select(i => i.FromUserID).Distinct().ToList());
                model.UsersPhotos = InsuranceBusiness.BusinessLayer.GetPhotosForUsers(model.Items.Select(i => i.FromUserID).Distinct().ToList());

                return PartialView("Partial/NotificationsControl", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
        [ValidateInput(false)]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult NewPost(HomeViewModel model, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload, string livePreviewUrlHidden, string livePreviewImgHidden, string livePreviewTitleHidden, string livePreviewDescriptionHidden, string livePreviewDomainHidden)
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

                if(!string.IsNullOrEmpty(livePreviewImgHidden))
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.LINK_POST;
                    var webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(livePreviewImgHidden);
                    MemoryStream imgStream = new MemoryStream(imageBytes);
                    Image img = Image.FromStream(imgStream);
                    if (img.Width > 1024 || img.Height > 1024)
                    {
                        newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(imageBytes, 1024, 1024);
                    }
                    else
                    {
                        newPost.Image = imageBytes;
                    }
                    newPost.URL_Image_Address = livePreviewImgHidden;
                    newPost.URL = livePreviewUrlHidden;
                    if (!string.IsNullOrEmpty(livePreviewDomainHidden))
                    {
                        Uri myUri = new Uri(livePreviewDomainHidden);
                        newPost.URL_Domain = myUri.Host;
                    }
                }

                if (!string.IsNullOrEmpty(livePreviewTitleHidden)
                    || !string.IsNullOrEmpty(livePreviewDescriptionHidden)
                    || !string.IsNullOrEmpty(livePreviewDomainHidden))
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.LINK_POST;
                    newPost.URL_Title = livePreviewTitleHidden;
                    newPost.URL_Description = livePreviewDescriptionHidden;
                    newPost.URL = livePreviewUrlHidden;
                    if (!string.IsNullOrEmpty(livePreviewDomainHidden))
                    {
                        Uri myUri = new Uri(livePreviewDomainHidden);
                        newPost.URL_Domain = myUri.Host;
                    }
                }

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                if (null != fileUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileUpload.InputStream);
                    newPost.FileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(fileUpload.FileName);
                }

                InsuranceBusiness.BusinessLayer.CreatePost(newPost);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult EditPost(PostViewModel model, string editPostContentTextarea, HttpPostedFileBase imgEditUpload, HttpPostedFileBase fileEditUpload, string editPostLivePreviewUrlHidden, string editPostLivePreviewImgHidden, string editPostLivePreviewTitleHidden, string editPostLivePreviewDescriptionHidden, string editPostLivePreviewDomainHidden)
        {
            try
            {
                PostDTO editedPost = InsuranceBusiness.BusinessLayer.GetPost(model.Post.ID);

                editedPost.LastChangeDate = DateTime.Now;
                editedPost.Text = editPostContentTextarea;
                editedPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST;
                editedPost.Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.PERSONAL_POST;

                editedPost.Image = null;
                editedPost.FileName = null;
                editedPost.FileExtension = null;

                if (model.RemoveImage && null != imgEditUpload)
                {
                    editedPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    editedPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgEditUpload.InputStream), 1024, 1024);
                    editedPost.FileName = Path.GetFileNameWithoutExtension(imgEditUpload.FileName);
                    editedPost.FileExtension = Path.GetExtension(imgEditUpload.FileName);
                }

                if (model.RemoveFile && null != fileEditUpload)
                {
                    editedPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    editedPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileEditUpload.InputStream);
                    editedPost.FileName = Path.GetFileNameWithoutExtension(fileEditUpload.FileName);
                    editedPost.FileExtension = Path.GetExtension(fileEditUpload.FileName);
                }

                if (!string.IsNullOrEmpty(editPostLivePreviewImgHidden))
                {
                    editedPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.LINK_POST;
                    var webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(editPostLivePreviewImgHidden);
                    MemoryStream imgStream = new MemoryStream(imageBytes);
                    Image img = Image.FromStream(imgStream);
                    if (img.Width > 1024 || img.Height > 1024)
                    {
                        editedPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(imageBytes, 1024, 1024);
                    }
                    else
                    {
                        editedPost.Image = imageBytes;
                    }
                    editedPost.URL_Image_Address = editPostLivePreviewImgHidden;
                    editedPost.URL = editPostLivePreviewUrlHidden;
                    if (!string.IsNullOrEmpty(editPostLivePreviewDomainHidden))
                    {
                        Uri myUri = new Uri(editPostLivePreviewDomainHidden);
                        editedPost.URL_Domain = myUri.Host;
                    }
                }

                if (!string.IsNullOrEmpty(editPostLivePreviewTitleHidden)
                    || !string.IsNullOrEmpty(editPostLivePreviewDescriptionHidden)
                    || !string.IsNullOrEmpty(editPostLivePreviewDomainHidden))
                {
                    editedPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.LINK_POST;
                    editedPost.URL_Title = editPostLivePreviewTitleHidden;
                    editedPost.URL_Description = editPostLivePreviewDescriptionHidden;
                    editedPost.URL = editPostLivePreviewUrlHidden;
                    if (!string.IsNullOrEmpty(editPostLivePreviewDomainHidden))
                    {
                        Uri myUri = new Uri(editPostLivePreviewDomainHidden);
                        editedPost.URL_Domain = myUri.Host;
                    }
                }

                InsuranceBusiness.BusinessLayer.EditPost(editedPost);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult SharePost(PostViewModel model, string editPostContentTextarea)
        {
            try
            {
                PostDTO sharedPost = InsuranceBusiness.BusinessLayer.GetPost(model.Post.ID);

                sharedPost.IsRepost = true;
                sharedPost.Repost_PostID = sharedPost.ID;
                sharedPost.Repost_ProfileID = InsuranceBusiness.BusinessLayer.GetUserProfileIdFromId(sharedPost.ID_User);
                sharedPost.Repost_Text = editPostContentTextarea;
                sharedPost.Text = editPostContentTextarea;
                sharedPost.Type = PostTypeEnum.TEXT_POST;
                sharedPost.Subject = PostSubjectEnum.PERSONAL_POST;

                sharedPost.ID_User = CurrentUser.ID_User;

                sharedPost.CreateDate = DateTime.Now;
                sharedPost.LastChangeDate = DateTime.Now;

                InsuranceBusiness.BusinessLayer.CreatePost(sharedPost);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public JsonResult GetPost(long id)
        {
            var result = InsuranceBusiness.BusinessLayer.GetPost(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult DeletePost(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeletePost(id, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return RedirectToAction("Error");
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult HidePost(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.HidePost(id, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateNewHRPost(PostSubjectEnum id)
        {
            try
            {
                NewPostViewModel model = new NewPostViewModel();

                model.Post = new PostDTO();
                model.Post.Subject = id;
                model.Post.PostOwner = CurrentUser;

                return PartialView("Partial/_CreateHRPost", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return PartialView("Error");
            }
        }

        [HttpPost]
        //[FunctionalityAutorizeAttribute("NEW_CURRENT_DISCUSSION_FUNCTIONALITY")]
        public ActionResult CreateNewHRPost(NewPostViewModel model, string postContentTextarea, HttpPostedFileBase imgUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = CurrentUser.ID_User,
                    ID_District = model.ID_District,
                    ID_County = model.ID_County,
                    Text = model.Post.Text,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = model.Post.Subject
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Post.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("HRInsurances", new { id = model.Post.Subject });
        }

        public ActionResult CreateNewInsuranceBusinessPost(PostSubjectEnum id)
        {
            try
            {
                NewPostViewModel model = new NewPostViewModel();

                model.Post = new PostDTO();
                model.Post.Subject = id;
                model.Post.PostOwner = CurrentUser;

                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectBusinessType } };
                model.SubjectTypeList = initList.Concat(InsuranceBusiness.BusinessLayer.GetSubjectTypes().Where(i => i.Value.StartsWith("INSURANCE_BUSINESS_")).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList()).ToList();                

                return PartialView("Partial/_CreateInsuranceBusinessPost", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return PartialView("Error");
            }
        }

        [HttpPost]
        //[FunctionalityAutorizeAttribute("NEW_CURRENT_DISCUSSION_FUNCTIONALITY")]
        public ActionResult CreateNewInsuranceBusinessPost(NewPostViewModel model, string postContentTextarea, HttpPostedFileBase imgUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = CurrentUser.ID_User,
                    ID_District = model.ID_District,
                    ID_County = model.ID_County,
                    Text = model.Post.Text,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    ID_PostSubject = model.Post.ID_PostSubject,
                    Subject = model.Post.Subject
                };

                string subjectTypeToken = InsuranceBusiness.BusinessLayer.GetSubjectTypes().FirstOrDefault(i => i.Key == model.Post.ID_PostSubject).Value;
                switch(subjectTypeToken)
                {
                    case "INSURANCE_BUSINESS_BUY_SELL_POST":
                        newPost.Subject = PostSubjectEnum.INSURANCE_BUSINESS_BUY_SELL_POST;
                        break;
                    case "INSURANCE_BUSINESS_PARTNERSHIP_POST":
                        newPost.Subject = PostSubjectEnum.INSURANCE_BUSINESS_PARTNERSHIP_POST;
                        break;
                }

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Post.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("InsuranceBusinesses", new { id = model.Post.Subject });
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_CURRENT_DISCUSSION_FUNCTIONALITY")]
        public ActionResult NewCurrentDiscussionPost(HomeViewModel model, string postTitleTextarea, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = model.Profile.ID_User,
                    Title = postTitleTextarea,
                    Text = postContentTextarea,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.CURRENT_DISCUSSION_POST
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                if (null != fileUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileUpload.InputStream);
                    newPost.FileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(fileUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);

                InsuranceBusiness.BusinessLayer.CreateNotificationForNewPost(NotificationTypeEnum.NEW_CURRENT_DISCUSSION_POST, postId);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("CurrentDiscussions");
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_ASF_FUNCTIONALITY")]
        public ActionResult NewAsfPost(HomeViewModel model, string postTitleTextarea, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = model.Profile.ID_User,
                    Title = postTitleTextarea,
                    Text = postContentTextarea,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.ASF_POST
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                if (null != fileUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileUpload.InputStream);
                    newPost.FileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(fileUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);

                InsuranceBusiness.BusinessLayer.CreateNotificationForNewPost(NotificationTypeEnum.NEW_ASF_POST, postId);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("AsfPage");
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_APS_FUNCTIONALITY")]
        public ActionResult NewApsPost(HomeViewModel model, string postTitleTextarea, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = model.Profile.ID_User,
                    Title = postTitleTextarea,
                    Text = postContentTextarea,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.APS_POST
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                if (null != fileUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileUpload.InputStream);
                    newPost.FileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(fileUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);

                InsuranceBusiness.BusinessLayer.CreateNotificationForNewPost(NotificationTypeEnum.NEW_APS_POST, postId);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("ApsPage");
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_APROSE_FUNCTIONALITY")]
        public ActionResult NewAprosePost(HomeViewModel model, string postTitleTextarea, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload)
        {
            try
            {
                PostDTO newPost = new PostDTO()
                {
                    Active = true,
                    CreateDate = DateTime.Now,
                    LastChangeDate = DateTime.Now,
                    ID_User = model.Profile.ID_User,
                    Title = postTitleTextarea,
                    Text = postContentTextarea,
                    Type = null == imgUpload ? InsuranceSocialNetworkCore.Enums.PostTypeEnum.TEXT_POST : InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST,
                    Subject = InsuranceSocialNetworkCore.Enums.PostSubjectEnum.APROSE_POST
                };

                if (null != imgUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.IMAGE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 1024, 1024);
                    newPost.FileName = Path.GetFileNameWithoutExtension(imgUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(imgUpload.FileName);
                }

                if (null != fileUpload)
                {
                    newPost.Type = InsuranceSocialNetworkCore.Enums.PostTypeEnum.FILE_POST;
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(fileUpload.InputStream);
                    newPost.FileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    newPost.FileExtension = Path.GetExtension(fileUpload.FileName);
                }

                long postId = InsuranceBusiness.BusinessLayer.CreatePost(newPost);

                InsuranceBusiness.BusinessLayer.CreateNotificationForNewPost(NotificationTypeEnum.NEW_APROSE_POST, postId);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("AprosePage");
        }

        public FileResult Download(long id)
        {
            PostDTO post = InsuranceBusiness.BusinessLayer.GetPost(id);
            if (null != post.PostImage[0])
            {
                if (!string.IsNullOrEmpty(post.PostImage[0].FileName) && !string.IsNullOrEmpty(post.PostImage[0].FileExtension))
                {
                    return File(post.PostImage[0].Image, System.Net.Mime.MediaTypeNames.Application.Octet, post.PostImage[0].FileName + "." + post.PostImage[0].FileExtension);
                }
                return File(post.PostImage[0].Image, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            return null;
        }

        [FunctionalityAutorizeAttribute("LIKE_POST_FUNCTIONALITY")]
        public JsonResult LikePost(long postId)
        {
            if (InsuranceBusiness.BusinessLayer.LikePost(postId, CurrentUser.ID_User) && !InsuranceBusiness.BusinessLayer.IsOwnPost(postId, CurrentUser.ID_User))
            {
                InsuranceBusiness.BusinessLayer.CreateNotificationForPost(postId, CurrentUser.ID_User, NotificationTypeEnum.NEW_POST_LIKE);
            }

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
        public ActionResult Posts(long? id)
        {
            PostItemsViewModel model = new PostItemsViewModel();
            model.Profile = CurrentUser;

            model.Items = InsuranceBusiness.BusinessLayer.GetUserRelatedPosts(CurrentUser.ID_User);
            
            return PartialView("Partial/PostsControl", model);
        }

        //[HttpPost]
        //[FunctionalityAutorizeAttribute("COMMENT_POST_FUNCTIONALITY")]
        //public ActionResult NewComment(long postId, string postNewComment)
        //{
        //    try
        //    {
        //        PostCommentDTO newComment = new PostCommentDTO()
        //        {
        //            ID_Post = postId,
        //            ID_User = CurrentUser.ID_User,
        //            Text = postNewComment
        //        };

        //        InsuranceBusiness.BusinessLayer.CreateComment(newComment);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return RedirectToAction("Index");
        //}

        [FunctionalityAutorizeAttribute("COMMENT_POST_FUNCTIONALITY")]
        public JsonResult NewComment(long postId, string postNewComment, bool hasNotification)
        {
            long commentId = -1;
            try
            {
                PostCommentDTO newComment = new PostCommentDTO()
                {
                    ID_Post = postId,
                    ID_User = CurrentUser.ID_User,
                    Text = postNewComment
                };

                commentId = InsuranceBusiness.BusinessLayer.CreateComment(newComment);

                if (hasNotification && commentId > 0 && !InsuranceBusiness.BusinessLayer.IsOwnPost(postId, CurrentUser.ID_User))
                {
                    InsuranceBusiness.BusinessLayer.CreateNotificationForPost(postId, CurrentUser.ID_User, NotificationTypeEnum.NEW_POST_COMMENT);
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, postId), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return Json(new { ok = false, message = commentId });
            }

            return Json(new { ok = true, message = commentId });
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileInfo(long? id, long? idNotification)
        {
            try
            {
                HomeViewModel model = new HomeViewModel();

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id, false);
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

                if (idNotification.HasValue)
                {
                    InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
                }

                model.IsProfileTimeline = true;
                model.IsOwnProfile = (!id.HasValue || (id.HasValue && id.Value == model.Profile.ID));
                model.CompaniesWorkingWith = InsuranceBusiness.BusinessLayer.GetInsuranceCompaniesWorkingWith(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList();
                model.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

                if (!model.IsOwnProfile && id.HasValue)
                {
                    model.ProfileInfo = InsuranceBusiness.BusinessLayer.GetUserProfile(id.Value);
                    model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, model.ProfileInfo.ID_User);
                    model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, model.ProfileInfo.ID_User);
                }
                else
                {
                    model.ProfileInfo = model.Profile;
                }

                model.ProfileInfo.TotalFriends = InsuranceBusiness.BusinessLayer.GetTotalFriends(model.ProfileInfo.ID_User);
                model.ProfileInfo.TotalLikes = InsuranceBusiness.BusinessLayer.GetTotalLikes(model.ProfileInfo.ID_User);

                return View("Index", model);

                //return View(model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, (id.HasValue ? id.Value.ToString() : "")), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
            //var model = new ProfileViewModel();
            //if (null != this.User && this.User.Identity.IsAuthenticated)
            //{
            //    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
            //    if (null != user)
            //    {
            //        FillModel(model, user.Id);
            //    }
            //    else
            //    {
            //        return RedirectToAction("LogOff", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            //if(idNotification.HasValue)
            //{
            //    InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            //}

            //model.IsOwnProfile = id.HasValue ? model.Profile.ID == id.Value : true;
            //if(!model.IsOwnProfile)
            //{
            //    model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, model.Profile.ID_User);
            //    model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, model.Profile.ID_User);
            //}

            //model.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

            //return View(model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileConfigurations(string userId)
        {
            try
            {
                HomeViewModel model = new HomeViewModel();

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != user)
                    {
                        FillModel(model, user.Id, false);
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

                model.IsPostsView = false;
                model.IsProfileTimeline = false;
                model.IsOwnProfile = (userId == model.Profile.ID_User);
                model.IsProfileConfigurations = true;
                model.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

                if (!model.IsOwnProfile)
                {
                    model.ProfileInfo = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);
                    model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, model.ProfileInfo.ID_User);
                    model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, model.ProfileInfo.ID_User);
                }
                else
                {
                    model.ProfileInfo = model.Profile;
                }

                model.ProfileInfo.TotalFriends = InsuranceBusiness.BusinessLayer.GetTotalFriends(model.ProfileInfo.ID_User);
                model.ProfileInfo.TotalLikes = InsuranceBusiness.BusinessLayer.GetTotalLikes(model.ProfileInfo.ID_User);

                return View("Index", model);

                //return View(model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, userId), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        //public async Task<ActionResult> ProfileTimeline(long id)
        //{
        //    try
        //    {
        //        HomeViewModel model = new HomeViewModel();

        //        if (null != this.User && this.User.Identity.IsAuthenticated)
        //        {
        //            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //            var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
        //            if (null != user)
        //            {
        //                FillModel(model, user.Id, false);
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

        //        string targetUserId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);

        //        model.SearchModel.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchModel.SearchTerm, CurrentUser.ID);
        //        model.SearchModel.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);
        //        model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, targetUserId);
        //        model.IsProfileTimeline = true;
        //        model.TimelineProfileId = id;

        //        if (model.IsFriend)
        //        {
        //            model.Posts = InsuranceBusiness.BusinessLayer.GetUserPostsOnly(targetUserId);
        //        }

        //        return View("Index", model);

        //        //return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [FunctionalityAutorizeAttribute("ADD_FRIEND_FUNCTIONALITY")]
        public async Task<ActionResult> FriendRequest(string id, long? ntId)
        {
            if (ntId.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
            }

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

            model.IsOwnProfile = false;
            model.IsProfileTimeline = true;
            model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, id);
            model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, id);
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

            if (!model.IsOwnProfile)
            {
                model.ProfileInfo = InsuranceBusiness.BusinessLayer.GetUserProfile(id);
                model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, model.ProfileInfo.ID_User);
                model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, model.ProfileInfo.ID_User);
            }
            else
            {
                model.ProfileInfo = model.Profile;
            }

            model.ProfileInfo.TotalFriends = InsuranceBusiness.BusinessLayer.GetTotalFriends(model.ProfileInfo.ID_User);
            model.ProfileInfo.TotalLikes = InsuranceBusiness.BusinessLayer.GetTotalLikes(model.ProfileInfo.ID_User);

            return View("Index", model);
        }

        [FunctionalityAutorizeAttribute("ADD_FRIEND_FUNCTIONALITY")]
        public async Task<ActionResult> AcceptFriendRequest(string id)
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

            if(InsuranceBusiness.BusinessLayer.AcceptFriendRequest(CurrentUser.ID_User, id))
            {
                model.IsFriendRequest = false;
            }
            else
            {
                model.IsFriendRequest = true;
            }
            model.IsOwnProfile = false;
            model.IsFriend = true;
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

            return View("ProfileInfo", model);
        }

        [FunctionalityAutorizeAttribute("ADD_FRIEND_FUNCTIONALITY")]
        public async Task<ActionResult> IgnoreFriendRequest(string id)
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

            InsuranceBusiness.BusinessLayer.IgnoreFriendRequest(CurrentUser.ID_User, id);
            model.IsOwnProfile = false;
            model.IsFriendRequest = false;
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

            return View("ProfileInfo", model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileEdit(string userId, long? ntId)
        {
            if (ntId.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
            }

            //ProfileEditModel model = new ProfileEditModel();
            HomeViewModel model = new HomeViewModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            //UserProfileDTO userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            model.IsPostsView = false;
            model.IsOwnProfile = userId == CurrentUser.ID_User;
            model.IsProfileEdit = true;

            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            model.ProfileEditModel = new ProfileEditModel();
            model.ProfileEditModel.ID = model.Profile.ID;
            model.ProfileEditModel.ID_User = model.Profile.ID_User;
            model.ProfileEditModel.FirstName =  model.Profile.FirstName;
            model.ProfileEditModel.LastName =  model.Profile.LastName;
            model.ProfileEditModel.CompanyName = model.Profile.CompanyName;
            model.ProfileEditModel.MobilePhone_1 =  model.Profile.MobilePhone_1;
            model.ProfileEditModel.MobilePhone_2 =  model.Profile.MobilePhone_2;
            model.ProfileEditModel.Telephone_1 =  model.Profile.Telephone_1;
            model.ProfileEditModel.Telephone_2 =  model.Profile.Telephone_2;
            model.ProfileEditModel.Fax = model.Profile.Fax;
            model.ProfileEditModel.Address =  model.Profile.Address;
            model.ProfileEditModel.PostalCode = model.Profile.PostalCode;
            model.ProfileEditModel.ID_Parish = model.Profile.ID_Parish;
            model.ProfileEditModel.ID_County = model.Profile.ID_County;
            model.ProfileEditModel.ID_District = model.Profile.ID_District;
            model.ProfileEditModel.SameInformationForInvoice = model.ProfileEditModel.SameInformationForInvoice;
            model.ProfileEditModel.Invoice_Address = model.ProfileEditModel.Invoice_Address;
            model.ProfileEditModel.Invoice_PostalCode = model.ProfileEditModel.Invoice_PostalCode;
            model.ProfileEditModel.Invoice_ID_Parish = model.ProfileEditModel.Invoice_ID_Parish;
            model.ProfileEditModel.Invoice_ID_County = model.ProfileEditModel.Invoice_ID_County;
            model.ProfileEditModel.Invoice_ID_District = model.ProfileEditModel.Invoice_ID_District;
            model.ProfileEditModel.Birthdate =  model.Profile.Birthdate;
            model.ProfileEditModel.ContactEmail =  model.Profile.ContactEmail;
            model.ProfileEditModel.ProfilePhoto =  model.Profile.ProfilePhoto;
            model.ProfileEditModel.Website = string.IsNullOrEmpty( model.Profile.Website) ?  model.Profile.Website :  model.Profile.Website.ToLower();
            model.ProfileEditModel.AboutMe =  model.Profile.AboutMe;
            model.ProfileEditModel.Facebook = model.Profile.Facebook;
            model.ProfileEditModel.Twitter = model.Profile.Twitter;
            model.ProfileEditModel.GooglePlus = model.Profile.GooglePlus;
            model.ProfileEditModel.Skype =  model.Profile.Skype;
            model.ProfileEditModel.Whatsapp =  model.Profile.Whatsapp;
            model.ProfileEditModel.CreateDate = model.Profile.CreateDate;

            var companiesWorkingWith = InsuranceBusiness.BusinessLayer.GetInsuranceCompaniesWorkingWith(model.Profile.ID_User);
            model.ProfileEditModel.CompaniesWorkingWith = companiesWorkingWith.Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList();
            model.ProfileEditModel.AvailableCompaniesToWorkWith = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContacts().Where(i => !companiesWorkingWith.Exists(j => j.Key == i.ID)).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.BusinessName }).ToList();

            model.ProfileEditModel.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

            if(model.ProfileEditModel.ID_County.HasValue)
            {
                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } };
                model.ProfileEditModel.CountyList = initList.Concat(InsuranceBusiness.BusinessLayer.GetCountiesByDistrict(model.ProfileEditModel.ID_District.Value).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            }

            if (model.ProfileEditModel.ID_Parish.HasValue)
            {
                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectParish } };
                model.ProfileEditModel.ParishList = initList.Concat(InsuranceBusiness.BusinessLayer.GetParishesByCounties(model.ProfileEditModel.ID_County.Value).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            }

            model.Profile.TotalFriends = InsuranceBusiness.BusinessLayer.GetTotalFriends(model.Profile.ID_User);
            model.Profile.TotalLikes = InsuranceBusiness.BusinessLayer.GetTotalLikes(model.Profile.ID_User);

            return View("Index", model);
            //return View(model);
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileEditConfigurations(string userId)
        {
            //ProfileEditModel model = new ProfileEditModel();
            HomeViewModel model = new HomeViewModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            //UserProfileDTO userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            model.IsPostsView = false;
            model.IsProfileTimeline = false;
            model.IsOwnProfile = (userId == model.Profile.ID_User);
            model.IsProfileEditConfiguations = true;

            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            FillProfileEditModel(model);

            return View("Index", model);
            //return View(model);
        }

        private static void FillProfileEditModel(HomeViewModel model)
        {
            model.ProfileEditModel = new ProfileEditModel();
            model.ProfileEditModel.ID = model.Profile.ID;
            model.ProfileEditModel.ID_User = model.Profile.ID_User;
            model.ProfileEditModel.FirstName = model.Profile.FirstName;
            model.ProfileEditModel.LastName = model.Profile.LastName;
            model.ProfileEditModel.CompanyName = model.Profile.CompanyName;
            model.ProfileEditModel.MobilePhone_1 = model.Profile.MobilePhone_1;
            model.ProfileEditModel.MobilePhone_2 = model.Profile.MobilePhone_2;
            model.ProfileEditModel.Telephone_1 = model.Profile.Telephone_1;
            model.ProfileEditModel.Telephone_2 = model.Profile.Telephone_2;
            model.ProfileEditModel.Fax = model.Profile.Fax;
            model.ProfileEditModel.Address = model.Profile.Address;
            model.ProfileEditModel.PostalCode = model.Profile.PostalCode;
            model.ProfileEditModel.ID_Parish = model.Profile.ID_Parish;
            model.ProfileEditModel.ID_County = model.Profile.ID_County;
            model.ProfileEditModel.ID_District = model.Profile.ID_District;
            model.ProfileEditModel.SameInformationForInvoice = model.ProfileEditModel.SameInformationForInvoice;
            model.ProfileEditModel.Invoice_Address = model.ProfileEditModel.Invoice_Address;
            model.ProfileEditModel.Invoice_PostalCode = model.ProfileEditModel.Invoice_PostalCode;
            model.ProfileEditModel.Invoice_ID_Parish = model.ProfileEditModel.Invoice_ID_Parish;
            model.ProfileEditModel.Invoice_ID_County = model.ProfileEditModel.Invoice_ID_County;
            model.ProfileEditModel.Invoice_ID_District = model.ProfileEditModel.Invoice_ID_District;
            model.ProfileEditModel.Birthdate = model.Profile.Birthdate;
            model.ProfileEditModel.ContactEmail = model.Profile.ContactEmail;
            model.ProfileEditModel.ProfilePhoto = model.Profile.ProfilePhoto;
            model.ProfileEditModel.Website = string.IsNullOrEmpty(model.Profile.Website) ? model.Profile.Website : model.Profile.Website.ToLower();
            model.ProfileEditModel.AboutMe = model.Profile.AboutMe;
            model.ProfileEditModel.Facebook = model.Profile.Facebook;
            model.ProfileEditModel.Twitter = model.Profile.Twitter;
            model.ProfileEditModel.GooglePlus = model.Profile.GooglePlus;
            model.ProfileEditModel.Skype = model.Profile.Skype;
            model.ProfileEditModel.Whatsapp = model.Profile.Whatsapp;
            //model.ProfileEditModel.CompaniesWorkingWith = model.Profile.CompaniesWorkingWith;
            model.ProfileEditModel.CreateDate = model.Profile.CreateDate;

            var companiesWorkingWith = InsuranceBusiness.BusinessLayer.GetInsuranceCompaniesWorkingWith(model.Profile.ID_User);
            model.ProfileEditModel.CompaniesWorkingWith = companiesWorkingWith.Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList();
            model.ProfileEditModel.AvailableCompaniesToWorkWith = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContacts().Where(i => companiesWorkingWith.Exists(j => j.Key == i.ID)).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.BusinessName }).ToList();

            model.ProfileEditModel.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

            model.ProfileEditModel.ProfileSettings = new ProfileSettingsModel();
            model.ProfileEditModel.ProfileSettings.ShowDisplayName = model.Profile.ProfileSettings.FirstOrDefault().ShowDisplayName;
            model.ProfileEditModel.ProfileSettings.ShowBirthDate = model.Profile.ProfileSettings.FirstOrDefault().ShowBirthDate;
            model.ProfileEditModel.ProfileSettings.ShowContactInformation = model.Profile.ProfileSettings.FirstOrDefault().ShowContactInformation;
            model.ProfileEditModel.ProfileSettings.ShowSocialNetworks = model.Profile.ProfileSettings.FirstOrDefault().ShowSocialNetworks;

            model.ProfileEditModel.ProfileSettings.LikesOnYourPosts = model.Profile.ProfileSettings.FirstOrDefault().LikesOnYourPosts;
            model.ProfileEditModel.ProfileSettings.CommentsOnYourPosts = model.Profile.ProfileSettings.FirstOrDefault().CommentsOnYourPosts;
            model.ProfileEditModel.ProfileSettings.ReceiveNotificationsByEmail = model.Profile.ProfileSettings.FirstOrDefault().ReceiveNotificationsByEmail;
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileSave(HomeViewModel model, HttpPostedFileBase fileUploaderControl)
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

            if (CurrentUser.ID != model.ProfileEditModel.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            model.IsOwnProfile = CurrentUser.ID == model.ProfileEditModel.ID;

            UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ProfileEditModel.ID);

            profile.AboutMe = model.ProfileEditModel.AboutMe;
            profile.Address = model.ProfileEditModel.Address;
            profile.PostalCode = model.ProfileEditModel.PostalCode;
            profile.ID_Parish = model.ProfileEditModel.ID_Parish;
            profile.ID_County = model.ProfileEditModel.ID_County;
            profile.ID_District = model.ProfileEditModel.ID_District;
            profile.SameInformationForInvoice = model.ProfileEditModel.SameInformationForInvoice;
            profile.Invoice_Address = model.ProfileEditModel.Invoice_Address;
            profile.Invoice_PostalCode = model.ProfileEditModel.Invoice_PostalCode;
            profile.Invoice_ID_Parish = model.ProfileEditModel.Invoice_ID_Parish;
            profile.Invoice_ID_County = model.ProfileEditModel.Invoice_ID_County;
            profile.Invoice_ID_District = model.ProfileEditModel.Invoice_ID_District;
            profile.Birthdate = model.ProfileEditModel.Birthdate;
            profile.CompanyName = model.ProfileEditModel.CompanyName;
            profile.ContactEmail = model.ProfileEditModel.ContactEmail;
            profile.FirstName = model.ProfileEditModel.FirstName;
            profile.LastName = model.ProfileEditModel.LastName;
            profile.MobilePhone_1 = model.ProfileEditModel.MobilePhone_1;
            profile.MobilePhone_2 = model.ProfileEditModel.MobilePhone_2;
            profile.Facebook = model.ProfileEditModel.Facebook;
            profile.Twitter = model.ProfileEditModel.Twitter;
            profile.GooglePlus = model.ProfileEditModel.GooglePlus;
            profile.Skype = model.ProfileEditModel.Skype;
            profile.Whatsapp = model.ProfileEditModel.Whatsapp;
            //profile.CompaniesWorkingWith = model.ProfileEditModel.CompaniesWorkingWith;
            if (null != fileUploaderControl)
            {
                Bitmap resizedImage = ImageUtils.ResizeImage(Bitmap.FromStream(fileUploaderControl.InputStream), 250, 250);
                profile.ProfilePhoto = ImageUtils.ImageToByte(resizedImage);
                model.ProfileEditModel.ProfilePhoto = profile.ProfilePhoto;
                model.ProfileEditModel.Profile.ProfilePhoto = profile.ProfilePhoto;
                //using (var binaryReader = new BinaryReader(fileUploaderControl.InputStream))
                //{
                //    profile.ProfilePhoto = binaryReader.ReadBytes(fileUploaderControl.ContentLength);
                //    model.ProfilePhoto = profile.ProfilePhoto;
                //}
            }
            profile.Telephone_1 = model.ProfileEditModel.Telephone_1;
            profile.Telephone_2 = model.ProfileEditModel.Telephone_2;
            profile.Fax = model.ProfileEditModel.Fax;
            profile.Website = model.ProfileEditModel.Website;
            
            InsuranceBusiness.BusinessLayer.UpdateProfile(profile);

            try
            {
                InsuranceBusiness.BusinessLayer.UpdateEmailAuthorizedForAutomaticApproval(model.Profile.ID_User, model.ProfileEditModel.SelectedAllowedEmails);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            try
            {
                InsuranceBusiness.BusinessLayer.UpdateCompaniesWorkingWith(model.Profile.ID_User, model.ProfileEditModel.SelectedCompaniesWorkingWith, CompanyTypeEnum.INSURANCE_COMPANY_CONTACT);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            //return View("ProfileInfo", model);
            return RedirectToAction("ProfileInfo", new { id = model.ProfileEditModel.ID });
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileSaveConfigurations(HomeViewModel model)
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

            if (CurrentUser.ID != model.ProfileEditModel.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            //model.IsOwnProfile = CurrentUser.ID == model.ProfileEditModel.ID;

            ProfileSettingsDTO profileSettings = InsuranceBusiness.BusinessLayer.GetUserProfileSettings(model.ProfileEditModel.ID);

            profileSettings.ShowDisplayName = model.ProfileEditModel.ProfileSettings.ShowDisplayName;
            profileSettings.ShowBirthDate = model.ProfileEditModel.ProfileSettings.ShowBirthDate;
            profileSettings.ShowContactInformation = model.ProfileEditModel.ProfileSettings.ShowContactInformation;
            profileSettings.ShowSocialNetworks = model.ProfileEditModel.ProfileSettings.ShowSocialNetworks;

            profileSettings.LikesOnYourPosts = model.ProfileEditModel.ProfileSettings.LikesOnYourPosts;
            profileSettings.CommentsOnYourPosts = model.ProfileEditModel.ProfileSettings.CommentsOnYourPosts;
            profileSettings.ReceiveNotificationsByEmail = model.ProfileEditModel.ProfileSettings.ReceiveNotificationsByEmail;

            InsuranceBusiness.BusinessLayer.UpdateProfileSetting(profileSettings);

            return RedirectToAction("ProfileConfigurations", new { userId = model.Profile.ID_User });
        }

        [FunctionalityAutorizeAttribute("PROFILE_INFO_FUNCTIONALITY")]
        public async Task<ActionResult> ProfileChangePassword(HomeViewModel model)
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

            if (CurrentUser.ID != model.ProfileEditModel.ID)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var result = await UserManager.PasswordValidator.ValidateAsync(model.ProfileEditModel.ProfileSettings.NewPassword);

            if(result.Succeeded)
            {
                result = await UserManager.ChangePasswordAsync(model.Profile.ID_User, model.ProfileEditModel.ProfileSettings.OldPassword, model.ProfileEditModel.ProfileSettings.NewPassword);

                if(result.Succeeded)
                {
                    return RedirectToAction("ProfileConfigurations", new { userId = model.Profile.ID_User });
                }
            }

            model.IsPostsView = false;
            model.IsProfileTimeline = false;
            model.IsOwnProfile = true;
            model.IsProfileEditConfiguations = true;

            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.Profile.ID_User);

            FillProfileEditModel(model);

            return View("Index", model);
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

            model.IsOwnProfile = CurrentUser.ID == model.ID;

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

        private void FillModel(ProfileViewModel model, string userId, bool getUserPosts = true)
        {
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            if (null == model.Profile.ProfilePhoto || model.Profile.ProfilePhoto.Length == 0)
            {
                Image img = Image.FromFile(Server.MapPath("/Content/images/no_photo_img.jpg"));
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    model.Profile.ProfilePhoto = ms.ToArray();
                }
            }

            CurrentUser = model.Profile;
            model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);
            model.TotalUnreadMessages = InsuranceBusiness.BusinessLayer.GetTotalUnreadMessages(userId);
            if (model is HomeViewModel)
            {
                if (((HomeViewModel)model).IsPostsView && getUserPosts)
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

        [HttpGet]
        public ActionResult GetCountiesByDistrict(string districtId)
        {
            try
            {
                if (string.IsNullOrEmpty(districtId))
                {
                    return Json(new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } }, JsonRequestBehavior.AllowGet);
                }

                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectCounty } };

                List<SelectListItem> items = initList.Concat(InsuranceBusiness.BusinessLayer.GetCountiesByDistrict(Int64.Parse(districtId)).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, districtId), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult GetParishesByCounty(string countyId)
        {
            try
            {
                if (string.IsNullOrEmpty(countyId))
                {
                    return Json(new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectParish } }, JsonRequestBehavior.AllowGet);
                }

                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectParish } };

                List<SelectListItem> items = initList.Concat(InsuranceBusiness.BusinessLayer.GetParishesByCounties(Int64.Parse(countyId)).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, countyId), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    GarageName = model.SearchCompaniesModel.GarageName,
                    GarageDistrictID = model.SearchCompaniesModel.GarageDistrict,
                    GarageCountyID = model.SearchCompaniesModel.GarageCounty,
                    GarageServiceID = model.SearchCompaniesModel.GarageServiceID,
                    GaragePartnership = model.SearchCompaniesModel.GaragePartnership,
                    GarageOfficialAgent = model.SearchCompaniesModel.GarageOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchGarages(companySearchFilter);
                
                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    ClinicName = model.SearchCompaniesModel.ClinicName,
                    ClinicDistrictID = model.SearchCompaniesModel.ClinicDistrict,
                    ClinicCountyID = model.SearchCompaniesModel.ClinicCounty,
                    ClinicServiceID = model.SearchCompaniesModel.ClinicServiceID,
                    ClinicPartnership = model.SearchCompaniesModel.ClinicPartnership,
                    ClinicOfficialAgent = model.SearchCompaniesModel.ClinicOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchMedicalClinis(companySearchFilter);

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    ConstructionCompanyName = model.SearchCompaniesModel.ConstructionCompanyName,
                    ConstructionCompanyDistrictID = model.SearchCompaniesModel.ConstructionCompanyDistrict,
                    ConstructionCompanyCountyID = model.SearchCompaniesModel.ConstructionCompanyCounty,
                    ConstructionCompanyServiceID = model.SearchCompaniesModel.ConstructionCompanyServiceID,
                    ConstructionCompanyPartnership = model.SearchCompaniesModel.ConstructionCompanyPartnership,
                    ConstructionCompanyOfficialAgent = model.SearchCompaniesModel.ConstructionCompanyOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchConstructionCompanies(companySearchFilter);

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    HomeApplianceRepairName = model.SearchCompaniesModel.HomeApplianceRepairName,
                    HomeApplianceRepairDistrictID = model.SearchCompaniesModel.HomeApplianceRepairDistrict,
                    HomeApplianceRepairCountyID = model.SearchCompaniesModel.HomeApplianceRepairCounty,
                    HomeApplianceRepairServiceID = model.SearchCompaniesModel.HomeApplianceRepairServiceID,
                    HomeApplianceRepairPartnership = model.SearchCompaniesModel.HomeApplianceRepairPartnership,
                    HomeApplianceRepairOfficialAgent = model.SearchCompaniesModel.HomeApplianceRepairOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchHomeApplianceRepairs(companySearchFilter);

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    InsuranceContactName = model.SearchCompaniesModel.InsuranceContactName,
                    InsuranceContactDistrictID = model.SearchCompaniesModel.InsuranceContactDistrict,
                    InsuranceContactCountyID = model.SearchCompaniesModel.InsuranceContactCounty,
                    InsuranceContactServiceID = model.SearchCompaniesModel.InsuranceContactServiceID,
                    InsuranceContactPartnership = model.SearchCompaniesModel.InsuranceContactPartnership,
                    InsuranceContactOfficialAgent = model.SearchCompaniesModel.InsuranceContactOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchInsuranceCompanyContacts(companySearchFilter);

                FillModel(model, CurrentUser.ID_User);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
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

            model.IsOwnProfile = true;

            return View(model);
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> SystemManagement()
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

            model.IsOwnProfile = true;

            return View(model);
        }

        [FunctionalityAutorizeAttribute("COMPANY_FAVORITE_FUNCTIONALITY")]
        public JsonResult AddFavorite(long companyId, int companyTypeId)
        {
            switch (companyTypeId)
            {
                case (int)CompanyTypeEnum.GARAGE:
                    InsuranceBusiness.BusinessLayer.AddFavoriteGarage(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.CONSTRUCTION_COMPANY:
                    InsuranceBusiness.BusinessLayer.AddFavoriteConstructionCompany(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                    InsuranceBusiness.BusinessLayer.AddFavoriteHomeAppliancesRepair(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                    InsuranceBusiness.BusinessLayer.AddFavoriteInsuranceCompanyContact(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.MEDICAL_CLINIC:
                    InsuranceBusiness.BusinessLayer.AddFavoriteMedicalClinic(companyId, CurrentUser.ID_User);
                    break;
            }

            //return RedirectToAction("Index");
            return Json(new { ok = true, message = "" });
        }

        [FunctionalityAutorizeAttribute("COMPANY_FAVORITE_FUNCTIONALITY")]
        public JsonResult RemoveFavorite(long companyId, int companyTypeId)
        {
            switch (companyTypeId)
            {
                case (int)CompanyTypeEnum.GARAGE:
                    InsuranceBusiness.BusinessLayer.RemoveFavoriteGarage(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.CONSTRUCTION_COMPANY:
                    InsuranceBusiness.BusinessLayer.RemoveFavoriteConstructionCompany(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                    InsuranceBusiness.BusinessLayer.RemoveFavoriteHomeAppliancesRepair(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                    InsuranceBusiness.BusinessLayer.RemoveFavoriteInsuranceCompanyContact(companyId, CurrentUser.ID_User);
                    break;
                case (int)CompanyTypeEnum.MEDICAL_CLINIC:
                    InsuranceBusiness.BusinessLayer.RemoveFavoriteMedicalClinic(companyId, CurrentUser.ID_User);
                    break;
            }

            //return RedirectToAction("Index");
            return Json(new { ok = true, message = "" });
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("APS_FUNCTIONALITY")]
        public async Task<ActionResult> ApsPage()
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetAPSPosts();

            model.QuickLinks = new List<InsuranceSocialNetworkCore.Types.ListItemString>();
            for (int i = 1; i < 7; i++)
            {
                SystemSettingsDTO title_setting = InsuranceBusiness.BusinessLayer.GetSystemSetting(string.Format("APS_QUICK_LINK_{0}_TITLE", i));
                if (!string.IsNullOrEmpty(title_setting.Value))
                {
                    SystemSettingsDTO url_setting = InsuranceBusiness.BusinessLayer.GetSystemSetting(string.Format("APS_QUICK_LINK_{0}_URL", i));
                    if (!string.IsNullOrEmpty(url_setting.Value))
                    {
                        model.QuickLinks.Add(new InsuranceSocialNetworkCore.Types.ListItemString() { Key = title_setting.Value, Value = url_setting.Value });
                    }
                    else
                    {
                        model.QuickLinks.Add(new InsuranceSocialNetworkCore.Types.ListItemString() { Key = title_setting.Value });
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("APS_FUNCTIONALITY")]
        public async Task<ActionResult> ApsDetails(long id, long? idNotification)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            if (idNotification.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            }

            model.Posts = new List<PostDTO>() { InsuranceBusiness.BusinessLayer.GetPost(id) };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchApsPosts(HomeViewModel model)
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

                if (null == model.SearchModel.SearchTerm)
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.GetAPSPosts();
                }
                else
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.SearchAPSPosts(model.SearchModel.SearchTerm);
                }

                return View("ApsPage", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("ASF_FUNCTIONALITY")]
        public async Task<ActionResult> AsfPage()
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetASFPosts();

            model.QuickLinks = new List<InsuranceSocialNetworkCore.Types.ListItemString>();
            for(int i=1;i<7;i++)
            {
                SystemSettingsDTO title_setting = InsuranceBusiness.BusinessLayer.GetSystemSetting(string.Format("ASF_QUICK_LINK_{0}_TITLE", i));
                if(!string.IsNullOrEmpty(title_setting.Value))
                {
                    SystemSettingsDTO url_setting = InsuranceBusiness.BusinessLayer.GetSystemSetting(string.Format("ASF_QUICK_LINK_{0}_URL", i));
                    if(!string.IsNullOrEmpty(url_setting.Value))
                    {
                        model.QuickLinks.Add(new InsuranceSocialNetworkCore.Types.ListItemString() { Key = title_setting.Value, Value = url_setting.Value });
                    }
                    else
                    {
                        model.QuickLinks.Add(new InsuranceSocialNetworkCore.Types.ListItemString() { Key = title_setting.Value });
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("ASF_FUNCTIONALITY")]
        public async Task<ActionResult> AsfDetails(long id, long? idNotification)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            if (idNotification.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            }

            model.Posts = new List<PostDTO>() { InsuranceBusiness.BusinessLayer.GetPost(id) };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchAsfPosts(HomeViewModel model)
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

                if (null == model.SearchModel.SearchTerm)
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.GetASFPosts();
                }
                else
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.SearchASFPosts(model.SearchModel.SearchTerm);
                }

                return View("AsfPage", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("APROSE_FUNCTIONALITY")]
        public async Task<ActionResult> AprosePage()
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetAPROSEPosts();

            return View(model);
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("APROSE_FUNCTIONALITY")]
        public async Task<ActionResult> AproseDetails(long id, long? idNotification)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            if (idNotification.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            }

            model.Posts = new List<PostDTO>() { InsuranceBusiness.BusinessLayer.GetPost(id) };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchAprosePosts(HomeViewModel model)
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

                if (null == model.SearchModel.SearchTerm)
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.GetAPROSEPosts();
                }
                else
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.SearchAPROSEPosts(model.SearchModel.SearchTerm);
                }

                return View("AprosePage", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("CURRENT_DISCUSSIONS_FUNCTIONALITY")]
        public async Task<ActionResult> CurrentDiscussions()
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetCurrentDiscussionPosts(CurrentUser.ID_User);

            return View(model);
        }

        [Authorize]
        [FunctionalityAutorizeAttribute("CURRENT_DISCUSSIONS_FUNCTIONALITY")]
        public async Task<ActionResult> CurrentDiscussionDetails(long id, long? idNotification)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            if(idNotification.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(idNotification.Value);
            }

            model.Posts = new List<PostDTO>() { InsuranceBusiness.BusinessLayer.GetPost(id) };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchCurrentDiscussionsPosts(HomeViewModel model)
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

                if (null == model.SearchModel.SearchTerm)
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.GetCurrentDiscussionPosts(CurrentUser.ID_User);
                }
                else
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.SearchCurrentDiscussionPosts(CurrentUser.ID_User, model.SearchModel.SearchTerm);
                }

                return View("CurrentDiscussions", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        [Authorize]
        public async Task<ActionResult> HRInsurances(PostSubjectEnum? id)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetHumanResourcesPosts(CurrentUser.ID_User, id);

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> InsuranceBusinesses(PostSubjectEnum? id)
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
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

            model.Posts = InsuranceBusiness.BusinessLayer.GetInsuranceBusinessesPosts(CurrentUser.ID_User, id);

            return View(model);
        }

        public ActionResult ConfirmPostDelete(long id)
        {
            ViewBag.postId = id;
            return PartialView("Partial/_ConfirmPostDelete");
        }

        public ActionResult ConfirmPostHide(long id)
        {
            ViewBag.postId = id;
            return PartialView("Partial/_ConfirmPostHide");
        }

        public ActionResult ConfirmChatDelete(long id)
        {
            ViewBag.chatId = id;
            return PartialView("Partial/_ConfirmChatDelete");
        }

        public ActionResult EditPostDialog(long id)
        {
            PostViewModel model = new PostViewModel();

            FillModel(model, CurrentUser.ID_User, false);
            model.Post = InsuranceBusiness.BusinessLayer.GetPost(id);

            return PartialView("Partial/_EditPost", model);
        }

        public ActionResult SharePostDialog(long id)
        {
            PostViewModel model = new PostViewModel();

            FillModel(model, CurrentUser.ID_User, false);
            model.Post = InsuranceBusiness.BusinessLayer.GetPost(id);

            return PartialView("Partial/_SharePost", model);
        }

        [HttpPost]
        public JsonResult Preview(string url)
        {
            try
            {
                Scrap scrap = new Scrap();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    scrap = LivePreviewUtils.GetScrap(url);
                }
                return Json(scrap, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                //InsuranceBusiness.BusinessLayer.LogException("", "HomeController::Preview", ex);
                throw ex;
            }
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}