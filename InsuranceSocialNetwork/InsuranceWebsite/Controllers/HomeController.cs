using InsuranceSocialNetworkBusiness;
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

        //public async Task<ActionResult> Notifications()
        //{
        //    var model = new HomeViewModel();
        //    if (null != this.User && this.User.Identity.IsAuthenticated)
        //    {
        //        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
        //        if (null != user)
        //        {
        //            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
        //            model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(user.Id);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    return View(model);
        //}

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
                    newPost.Image = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream);
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

        private void FillModel(ProfileViewModel model, string userId)
        {
            model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(userId);
            CurrentUser = model.Profile;
            model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);
            if (model is HomeViewModel)
            {
                ((HomeViewModel)model).Posts = InsuranceBusiness.BusinessLayer.GetUserPosts(userId);
                ((HomeViewModel)model).Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(userId);

            }
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();

            return View(model);
        }
    }
}