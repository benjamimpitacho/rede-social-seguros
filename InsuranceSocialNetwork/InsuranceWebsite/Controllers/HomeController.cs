using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Utils;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.PostalCode;
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
using System.Threading;
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
                model.ActiveChat = InsuranceBusiness.BusinessLayer.GetChat(model.Profile.User.Id, secondUserId);
                model.Chats = InsuranceBusiness.BusinessLayer.GetChats(CurrentUser.ID_User);

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
                model.UsersNames = InsuranceBusiness.BusinessLayer.GetNamesForUsers(model.Items.Select(i => i.FromUserID).Distinct().ToList());
                model.UsersPhotos = InsuranceBusiness.BusinessLayer.GetPhotosForUsers(model.Items.Select(i => i.FromUserID).Distinct().ToList());

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
        public ActionResult NewPost(HomeViewModel model, string postContentTextarea, HttpPostedFileBase imgUpload, HttpPostedFileBase fileUpload)
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
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [FunctionalityAutorizeAttribute("NEW_POST_FUNCTIONALITY")]
        public ActionResult EditPost(PostViewModel model, string postContentTextarea, HttpPostedFileBase imgEditUpload, HttpPostedFileBase fileEditUpload)
        {
            try
            {
                PostDTO editedPost = InsuranceBusiness.BusinessLayer.GetPost(model.Post.ID);

                editedPost.LastChangeDate = DateTime.Now;
                editedPost.Text = postContentTextarea;
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

                InsuranceBusiness.BusinessLayer.EditPost(editedPost);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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

                model.IsProfileTimeline = true;
                model.IsOwnProfile = (!id.HasValue || (id.HasValue && id.Value == model.Profile.ID));
                model.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

                return View("Index", model);

                //return View(model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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

        public async Task<ActionResult> ProfileTimeline(long id)
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

                string targetUserId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);

                model.SearchModel.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchModel.SearchTerm, CurrentUser.ID);
                model.SearchModel.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);
                model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, targetUserId);
                model.IsProfileTimeline = true;
                model.TimelineProfileId = id;

                if (model.IsFriend)
                {
                    model.Posts = InsuranceBusiness.BusinessLayer.GetUserPostsOnly(targetUserId);
                }

                return View("Index", model);

                //return View(model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        [FunctionalityAutorizeAttribute("ADD_FRIEND_FUNCTIONALITY")]
        public async Task<ActionResult> FriendRequest(string id, long? ntId)
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

            model.IsOwnProfile = false;
            model.IsFriendRequest = InsuranceBusiness.BusinessLayer.HasPendingFriendRequest(CurrentUser.ID_User, id);
            model.IsFriend = InsuranceBusiness.BusinessLayer.AreFriends(CurrentUser.ID_User, id);
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

            if (ntId.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
            }

            return View("ProfileInfo", model);
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

            //if (id != model.Profile.ID)
            //{
            //    return RedirectToAction("LogOff", "Account");
            //}

            UserProfileDTO userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);

            model.IsOwnProfile = userId == CurrentUser.ID_User;

            model.ID = userProfile.ID;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            model.MobilePhone_1 = userProfile.MobilePhone_1;
            model.MobilePhone_2 = userProfile.MobilePhone_2;
            model.Telephone_1 = userProfile.Telephone_1;
            model.Telephone_2 = userProfile.Telephone_2;
            model.Address = userProfile.Address;
            model.Birthdate = userProfile.Birthdate;
            model.ContactEmail = userProfile.ContactEmail;
            model.ProfilePhoto = userProfile.ProfilePhoto;
            model.Website = string.IsNullOrEmpty(userProfile.Website) ? userProfile.Website : userProfile.Website.ToLower();
            model.AboutMe = userProfile.AboutMe;
            model.Skype = userProfile.Skype;
            model.Whatsapp = userProfile.Whatsapp;
            model.CompaniesWorkingWith = userProfile.CompaniesWorkingWith;

            model.AllowedEmails = InsuranceBusiness.BusinessLayer.GetAuthorizedEmailsForAutomaticApproval(model.Profile.ID_User).Select(i => new SelectListItem() { Value = i, Text = i }).ToList();

            if (ntId.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
            }

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

            model.IsOwnProfile = CurrentUser.ID == model.ID;

            UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID);

            profile.AboutMe = model.AboutMe;
            profile.Address = model.Address;
            profile.Birthdate = model.Birthdate;
            profile.ContactEmail = model.ContactEmail;
            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.MobilePhone_1 = model.MobilePhone_1;
            profile.MobilePhone_2 = model.MobilePhone_2;
            profile.Skype = model.Skype;
            profile.Whatsapp = model.Whatsapp;
            profile.CompaniesWorkingWith = model.CompaniesWorkingWith;
            if (null != fileUploaderControl)
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

            try
            {
                InsuranceBusiness.BusinessLayer.UpdateEmailAuthorizedForAutomaticApproval(model.Profile.ID_User, model.SelectedAllowedEmails);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            //return View("ProfileInfo", model);
            return RedirectToAction("ProfileInfo", new { id = model.Profile.ID });
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
                throw new NotImplementedException();
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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    ClinicName = model.SearchCompaniesModel.ClinicName,
                    ClinicDistrict = model.SearchCompaniesModel.ClinicDistrict,
                    ClinicCounty = model.SearchCompaniesModel.ClinicCounty,
                    ClinicServiceID = model.SearchCompaniesModel.ClinicServiceID,
                    ClinicPartnership = model.SearchCompaniesModel.ClinicPartnership,
                    ClinicOfficialAgent = model.SearchCompaniesModel.ClinicOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchMedicalClinis(companySearchFilter);

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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    ConstructionCompanyName = model.SearchCompaniesModel.ConstructionCompanyName,
                    ConstructionCompanyDistrict = model.SearchCompaniesModel.ConstructionCompanyDistrict,
                    ConstructionCompanyCounty = model.SearchCompaniesModel.ConstructionCompanyCounty,
                    ConstructionCompanyServiceID = model.SearchCompaniesModel.ConstructionCompanyServiceID,
                    ConstructionCompanyPartnership = model.SearchCompaniesModel.ConstructionCompanyPartnership,
                    ConstructionCompanyOfficialAgent = model.SearchCompaniesModel.ConstructionCompanyOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchConstructionCompanies(companySearchFilter);

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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    HomeApplianceRepairName = model.SearchCompaniesModel.HomeApplianceRepairName,
                    HomeApplianceRepairDistrict = model.SearchCompaniesModel.HomeApplianceRepairDistrict,
                    HomeApplianceRepairCounty = model.SearchCompaniesModel.HomeApplianceRepairCounty,
                    HomeApplianceRepairServiceID = model.SearchCompaniesModel.HomeApplianceRepairServiceID,
                    HomeApplianceRepairPartnership = model.SearchCompaniesModel.HomeApplianceRepairPartnership,
                    HomeApplianceRepairOfficialAgent = model.SearchCompaniesModel.HomeApplianceRepairOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchHomeApplianceRepairs(companySearchFilter);

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
                CompanySearchFilterDTO companySearchFilter = new CompanySearchFilterDTO()
                {
                    UserId = CurrentUser.ID_User,
                    InsuranceContactName = model.SearchCompaniesModel.InsuranceContactName,
                    InsuranceContactDistrict = model.SearchCompaniesModel.InsuranceContactDistrict,
                    InsuranceContactCounty = model.SearchCompaniesModel.InsuranceContactCounty,
                    InsuranceContactServiceID = model.SearchCompaniesModel.InsuranceContactServiceID,
                    InsuranceContactPartnership = model.SearchCompaniesModel.InsuranceContactPartnership,
                    InsuranceContactOfficialAgent = model.SearchCompaniesModel.InsuranceContactOfficialAgent
                };

                model.SearchCompaniesModel.ResultCompanyItems = InsuranceBusiness.BusinessLayer.SearchInsuranceCompanyContacts(companySearchFilter);

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

        [Authorize]
        public async Task<ActionResult> HRInsurances()
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

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> SafeBusinesses()
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

        public ActionResult EditPostDialog(long id)
        {
            PostViewModel model = new PostViewModel();

            FillModel(model, CurrentUser.ID_User, false);
            model.Post = InsuranceBusiness.BusinessLayer.GetPost(id);

            return PartialView("Partial/_EditPost", model);
        }
    }
}