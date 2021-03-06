﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using InsuranceWebsite.Models;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using System.Collections.Generic;
using InsuranceSocialNetworkCore.Enums;
using System.Resources;
using InsuranceSocialNetworkDTO.Role;
using System.IO;
using System.Net.Mail;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                //return View("ForgotPasswordConfirmation");
                ModelState.AddModelError("Email", Resources.Resources.InvalidLoginAttempt);
                return View(model);
            }

            if(!(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
                return View("RegisterToConfirm", new RegisterViewModel() { Name = profile.FirstName, Email = model.Email });
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("{0}, {1}, {2}", provider, returnUrl, rememberMe));
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //SelectList rolesList = new SelectList(InsuranceBusiness.BusinessLayer.GetRegisterRoles().ToList(), "Id", "Name");

            //ResourceManager rm = Resources.Resources.ResourceManager;
            //foreach (RoleDTO role in rolesList.Items)
            //{
            //    role.Name = rm.GetString(role.Name);
            //}

            //ViewBag.Name = rolesList;
            //ViewBag.UserRole = rolesList;

            RegisterViewModel model = new RegisterViewModel();

            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectRole } };
            model.UserRoles = initList.Concat(InsuranceBusiness.BusinessLayer.GetRegisterRoles().Select(i => new SelectListItem() { Value = i.Id, Text = Resources.Resources.ResourceManager.GetString(i.Name) })).ToList();

            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterToConfirm(RegisterViewModel model)
        {
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                user.EmailConfirmed = false;
                
                //user.LockoutEnabled = true;
                //await SendActivationEmail(user, model.Name);
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    try
                    {
                        RoleDTO role = InsuranceBusiness.BusinessLayer.GetRoles().FirstOrDefault(i => i.Id == model.UserRole);
                        result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                    }
                    catch (Exception ex)
                    {
                        InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);

                        await UserManager.DeleteAsync(user);

                        return View("Error");
                    }
                }
                
                long userId = -1;
                if (result.Succeeded)
                {
                    try
                    {
                        // Register default profile information!
                        userId = InsuranceBusiness.BusinessLayer.CreateDefaultUserProfile(user.Id, user.UserName, user.Email, model.Name, null, model.ProfessionalNumber);
                    }
                    catch (Exception ex)
                    {
                        InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);

                        await UserManager.DeleteAsync(user);

                        return View("Error");
                    }
                }

                if (result.Succeeded)
                {
                    try
                    {
                        InsuranceBusiness.BusinessLayer.CreateNotification(user.Id, null, NotificationTypeEnum.COMPLETE_PROFILE_INFO);

                        if (!InsuranceBusiness.BusinessLayer.IsEmailAuthorizedForAutomaticApproval(user.UserName) && !InsuranceBusiness.BusinessLayer.IsUserInRoleByUserID(user.Id, RoleEnum.ASSOCIATED_PREMIUM.ToString()))
                        {
                            await LockUserAccount(user.Id, 5000);
                        }

                        try
                        {
                            await SendConfirmationEmail(user, model.Name);
                        }
                        catch (Exception ex)
                        {
                            InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);

                            InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                            await UserManager.DeleteAsync(user);

                            return View("Error");
                        }

                        //if(InsuranceBusiness.BusinessLayer.IsEmailAuthorizedForAutomaticApproval(user.UserName))
                        //{
                        //    // Authorized
                        //    try
                        //    {
                        //        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        //        var activationResult = await UserManager.ConfirmEmailAsync(user.Id, code);

                        //        if (activationResult.Succeeded)
                        //        {
                        //            await SendActivationEmail(user, model.Name);
                        //        }
                        //    }
                        //    catch(Exception ex)
                        //    {
                        //        InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                        //        await UserManager.DeleteAsync(user);

                        //        return View(model);
                        //    }
                        //}
                        //else
                        //{
                        //    // Not authorized - pending activation
                        //    await SendConfirmationEmail(user, model.Name);
                        //}

                    }
                    catch (Exception ex)
                    {
                        InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);

                        InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                        await UserManager.DeleteAsync(user);

                        return View("Error");
                    }
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return View("RegisterToConfirm", model);
                }

                AddErrors(result);

                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Join(",", result.Errors));
            }

            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectRole } };
            model.UserRoles = initList.Concat(InsuranceBusiness.BusinessLayer.GetRegisterRoles().Select(i => new SelectListItem() { Value = i.Id, Text = Resources.Resources.ResourceManager.GetString(i.Name) })).ToList();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task<bool> SendConfirmationEmail(ApplicationUser user, string name)
        {
            var newToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            newToken = System.Web.HttpUtility.UrlEncode(newToken);

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(user.Email));
            m.Subject = Resources.Resources.EmailRegisterConfirmation;
            //m.Body = string.Format(Resources.Resources.RegisterConfirmationMessage, name, Url.Action("ConfirmEmail", "Account", new { token = code, code = user.Id }, Request.Url.Scheme));

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Account/EmailTemplates/RegisterConfirmationTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", name); //replacing the required things  

            m.Body = m.Body.Replace("{URL}", Url.Action("ConfirmEmail", "Account", new { token = newToken, code = user.Id }, Request.Url.Scheme));

            m.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };
            smtp.Send(m);

            return true;
        }

        //private async Task<bool> SendActivationEmail(ApplicationUser user, string name)
        //{
        //    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
        //        new System.Net.Mail.MailAddress(ConfigurationSettings.AppEmailAddress, Resources.Resources.ApplicationNAme),
        //        new System.Net.Mail.MailAddress(user.Email));
        //    m.Subject = Resources.Resources.EmailRegisterConfirmation;
        //    //m.Body = string.Format(Resources.Resources.RegisterConfirmationMessage, name, Url.Action("ConfirmEmail", "Account", new { token = code, code = user.Id }, Request.Url.Scheme));

        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Account/EmailTemplates/RegisterActivationTemplate.html")))
        //    {
        //        m.Body = reader.ReadToEnd();
        //    }

        //    m.Body = m.Body.Replace("{NAME}", name); //replacing the required things
        //    m.Body = m.Body.Replace("{URL}", ConfigurationSettings.ApplicationSiteUrl);
        //    m.IsBodyHtml = true;

        //    SmtpClient smtp = new SmtpClient(ConfigurationSettings.SmtpHost, ConfigurationSettings.SmtpPort)
        //    {
        //        Credentials = new System.Net.NetworkCredential(ConfigurationSettings.SmtpUsername, ConfigurationSettings.SmtpPassword),
        //        EnableSsl = false
        //    };
        //    smtp.Send(m);

        //    return true;
        //}

        private async Task<bool> SendRecoverPasswordEmail(ApplicationUser user, string name, string callbackUrl)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(user.Email));

            m.Subject = Resources.Resources.EmailPasswordRecover;
            //m.Body = string.Format(Resources.Resources.RegisterConfirmationMessage, name, Url.Action("ConfirmEmail", "Account", new { token = code, code = user.Id }, Request.Url.Scheme));

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Account/EmailTemplates/RecoverPAsswordTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", name); //replacing the required things
            m.Body = m.Body.Replace("{URL}", callbackUrl);
            m.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };

            smtp.Send(m);

            return true;
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string token, string code)
        {
            try
            {
                token = HttpUtility.UrlDecode(token);

                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, code, "AccountController::ConfirmEmail", string.Format("Received token={0} | code={1}", token, code));

                if (code == null || token == null)
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("{0}, {1}", token, code));
                    return View("Error");
                }

                var result = await UserManager.ConfirmEmailAsync(code, token);
                if(!result.Succeeded && result.Errors.Count() > 0 && result.Errors.First().ToLower().StartsWith("invalid token"))
                {
                    token = HttpUtility.UrlDecode(token);
                    result = await UserManager.ConfirmEmailAsync(code, token);
                }

                if (result.Succeeded)
                {
                    var user = UserManager.FindById(code);
                    var userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(code);

                    if (UserManager.IsLockedOut(user.Id))
                    {
                        RegisterViewModel model = new RegisterViewModel() { Name = userProfile.FirstName };
                        InsuranceBusiness.BusinessLayer.CreateNotificationForAdministrators(userProfile.ID_User, NotificationTypeEnum.USER_APPROVAL_PENDING);
                        return View("PendingApproval", model);
                    }
                    else
                    {
                        return View("Login");
                    }
                }

                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("{0}, {1}, {2}", token, code, string.Concat(result.Errors)));
                return View("Error");
            }
            catch(Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                await SendRecoverPasswordEmail(user, userProfile.FirstName, callbackUrl);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("{0}, {1}", returnUrl, rememberMe));
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("{0}, {1}, {2}", model.SelectedProvider, model.ReturnUrl, model.RememberMe));
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        public virtual async Task<IdentityResult> LockUserAccount(string userId, int? forDays)
        {
            var result = await UserManager.SetLockoutEnabledAsync(userId, true);
            if (result.Succeeded)
            {
                if (forDays.HasValue)
                {
                    result = await UserManager.SetLockoutEndDateAsync(userId, DateTimeOffset.UtcNow.AddDays(forDays.Value));
                }
                else
                {
                    result = await UserManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MaxValue);
                }
            }
            return result;
        }
        #endregion
    }
}