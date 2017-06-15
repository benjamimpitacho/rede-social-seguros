using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ActionResult> Messages()
        {
            var model = new HomeViewModel();
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
                    model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(user.Id);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

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

            return RedirectToAction("Index");
        }

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

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Search(SearchViewModel model)
        {
            try
            {
                if (null == model)
                {
                    model = new SearchViewModel();
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

            return RedirectToAction("Index");
        }

        //[HttpPost]
        public async Task<ActionResult> SearchUsers(SearchViewModel model)
        {
            try
            {
                if (null == model)
                {
                    model = new SearchViewModel();
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

                model.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchTerm, CurrentUser.ID);
                model.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);

                return View("Search", model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddFriend(long id, string searchTerm)
        {
            try
            {
                SearchViewModel model = new SearchViewModel();
                model.SearchTerm = searchTerm;

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

                if (InsuranceBusiness.BusinessLayer.AddFriend(CurrentUser.ID, id))
                {
                    string friendId = InsuranceBusiness.BusinessLayer.GetUserIdFromProfileId(id);
                    InsuranceBusiness.BusinessLayer.CreateNotification(friendId, NotificationTypeEnum.FRIEND_REQUEST_RECEIVED);
                }

                model.Users = InsuranceBusiness.BusinessLayer.SearchUsers(model.SearchTerm, CurrentUser.ID);
                model.AlreadyFriends = InsuranceBusiness.BusinessLayer.GetFriendsIDs(CurrentUser.ID);

                return View("Search", model);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
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

        public ActionResult LikePost(HomeViewModel model, long postId, string userId)
        {
            InsuranceBusiness.BusinessLayer.LikePost(postId, userId);

            return RedirectToAction("Index");
        }

        public ActionResult UnlikePost(HomeViewModel model, long postId, string userId)
        {
            InsuranceBusiness.BusinessLayer.UnlikePost(postId, userId);

            return RedirectToAction("Index");
        }

        public ActionResult Posts()
        {
            PostItemsViewModel model = new PostItemsViewModel();
            model.Profile = CurrentUser;
            model.Items = InsuranceBusiness.BusinessLayer.GetUserPosts(CurrentUser.ID_User);
            return PartialView("Partial/PostsControl", model);
        }

        [HttpPost]
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

        public async Task<ActionResult> ProfileInfo()
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

            return View(model);
        }

        public async Task<ActionResult> Friends()
        {
            var model = new FriendsViewModel();
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

        private void FillModel(ProfileViewModel model, string userId)
        {
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);
            CurrentUser = model.Profile;
            model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);
            if (model is HomeViewModel)
            {
                if (((HomeViewModel)model).IsPostsView)
                {
                    ((HomeViewModel)model).Posts = InsuranceBusiness.BusinessLayer.GetUserPosts(userId);
                }
                ((HomeViewModel)model).Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);
            }
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();

            return View(model);
        }

        #region Search Operations

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

        public ActionResult Settings()
        {
            HomeViewModel model = new HomeViewModel();
            FillModel(model, CurrentUser.ID_User);
            return View(model);
        }
    }
}