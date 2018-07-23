using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Utils;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using InsuranceWebsite.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVCGrid.Models;
using MVCGrid.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
//using System.Web.UI.WebControls;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class UsersManagementController : Controller
    {
        public static UserProfileDTO CurrentUser;

        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserProfileDTO, UserProfileModelObject>();
            cfg.CreateMap<UserProfileModelObject, UserProfileDTO>();

            cfg.CreateMap<UserDTO, UserModelObject>();
            cfg.CreateMap<UserModelObject, UserDTO>();
        });

        public UsersManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> Index(long? ntId)
        {
            if (ntId.HasValue)
            {
                InsuranceBusiness.BusinessLayer.MarkNotificationAsRead(ntId.Value);
            }

            UsersManagementViewModel model = new UsersManagementViewModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    FillModel(model, user.Id, false);
                    //model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
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

            try
            {
                var grid = MVCGridDefinitionTable.GetDefinition<UserProfileModelObject>("UsersManagementGrid");
            }
            catch (Exception)
            {
                MVCGridDefinitionTable.Add("UsersManagementGrid", new MVCGridBuilder<UserProfileModelObject>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText(Resources.Resources.FirstName)
                        .WithValueExpression(i => i.FirstName)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText(Resources.Resources.LastName)
                        .WithValueExpression(i => i.LastName)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("Username")
                        .WithHeaderText(Resources.Resources.Email)
                        .WithValueExpression(i => i.User.UserName)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("ProfessionalNumber")
                        .WithHeaderText(Resources.Resources.ProfessionalNumber)
                        .WithValueExpression(i => i.ProfessionalNumber)
                        .WithAllowChangeVisibility(true)
                        .WithFiltering(true)
                        .WithSorting(true)
                        .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                    cols.Add("CreateDate")
                        .WithSorting(true)
                        .WithHeaderText(Resources.Resources.RegisterDate)
                        .WithCellCssClassExpression(p => "dateCell")
                        .WithValueExpression(p => p.CreateDate.ToString("dd-MM-yyyy"))
                        .WithVisibility(true, true);
                    cols.Add("Active").WithHtmlEncoding(false)
                        .WithSorting(true)
                        .WithHeaderText(Resources.Resources.Active)
                        .WithValueExpression((p, c) => !p.User.LockoutEndDateUtc.HasValue ? "checked" : "")
                        .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                        .WithCellCssClassExpression(p => "mvcgrid-cell")
                        .WithVisibility(true, true);
                    cols.Add("Edit").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText(" ")
                        .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "UsersManagement", new { id = p.ID }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit'>" + Resources.Resources.Edit + "</a>")
                        .WithVisibility(true, false);
                    cols.Add("Delete").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText(" ")
                        .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "UsersManagement", new { id = p.ID }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                        .WithVisibility(true, false);
                })
                .WithAdditionalQueryOptionNames("Search")
                .WithSorting(true, "FirstName")
                .WithPaging(paging: true, itemsPerPage: 100, allowChangePageSize: true, maxItemsPerPage: 100)
                .WithProcessingMessage(Resources.Resources.Loading)
                .WithNextButtonCaption(Resources.Resources.Next)
                .WithPreviousButtonCaption(Resources.Resources.Previous)
                .WithSummaryMessage(Resources.Resources.ShowingGridEntries)
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>

                    var options = context.QueryOptions;
                    var result = new QueryResult<UserProfileModelObject>();
                    var query = BusinessItemsLists.GetUsers();
                    if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                    {
                        switch (options.SortColumnName.ToLower())
                        {
                            case "firstname":
                                if (options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.FirstName).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.FirstName).ToList();
                                }

                                break;
                            case "lastname":
                                if (options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.LastName).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.LastName).ToList();
                                }

                                break;
                            case "username":
                                if (options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.User.UserName).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.User.UserName).ToList();
                                }

                                break;
                            case "active":
                                if (options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.Active).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.Active).ToList();
                                }

                                break;
                            case "createdate":
                                if (options.SortDirection == SortDirection.Asc
                                    || options.SortDirection == SortDirection.Unspecified)
                                {
                                    query = query.OrderBy(p => p.CreateDate).ToList();
                                }
                                else
                                {
                                    query = query.OrderByDescending(p => p.CreateDate).ToList();
                                }

                                break;
                        }
                    }

                    string globalSearch = options.GetAdditionalQueryOptionString("Search");
                    if (null != globalSearch)
                    {
                        query = query.Where(i =>
                            (null != i.FirstName && i.FirstName.ToLower().Contains(globalSearch.ToLower()))
                            || (null != i.LastName && i.LastName.ToLower().Contains(globalSearch.ToLower()))
                            || (null != i.User && null != i.User.UserName && i.User.UserName.ToLower().Contains(globalSearch.ToLower()))
                            //|| (null != i.ContactEmail && i.ContactEmail.Contains(globalSearch))
                        ).ToList();
                    }

                    result.TotalRecords = query.Count();

                    if (options.GetLimitOffset().HasValue)
                    {
                        query = query
                            .Skip(options.GetLimitOffset().Value)
                            .Take(options.GetLimitRowcount().Value)
                            .ToList();
                    }

                    result.Items = query;

                    return result;

                    //return new QueryResult<UserProfileModelObject>()
                    //{
                    //    Items = query,
                    //    TotalRecords = query.Count // if paging is enabled, return the total number of records of all pages
                    //};
                })
            );
            }

            return View(model);
        }

        //[FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public JsonResult Get()
        {
            var data = BusinessItemsLists.GetUsers();
            data.ForEach(i => i.ProfilePhoto = null);
            string editUrl = this.HttpContext.Request.Path.Replace("Get", "Edit") + "/";
            string deleteUrl = this.HttpContext.Request.Path.Replace("Get", "Delete") + "/";

            //data.ForEach(i => i.Controlo = "<button  onclick=\"$('#urlField').val('" + editUrl + i.ID.ToString() + "');$('.ui-dialog-title').html('Edit');$('#dialog-edit').dialog('open');return false;\" >" + Resources.Resources.Edit + "</button><button  onclick=\"$('#urlField').val('" + deleteUrl + i.ID.ToString() + "');$('#dialog-confirm').dialog('open');$('.date-picker').datepicker();return false;\" >" + Resources.Resources.Delete + "</button>");
            data.ForEach(i => i.Controlo = "<input class=\"jsgrid-button jsgrid-edit-button\" type=\"button\" title=\"Edit\" onclick=\"$('#urlField').val('" + editUrl + i.ID.ToString() + "');$('.ui-dialog-title').html('Edit');$('#dialog-edit').dialog('open');return false;\"><input class=\"jsgrid-button jsgrid-delete-button\" type=\"button\" title=\"Delete\" onclick=\"openConfirmUserDelete(" + i.ID.ToString() + ")\">");
            data.ForEach(i => i.Active = !i.User.LockoutEndDateUtc.HasValue);
            //data.ForEach(i => i.CreateDateString = string.Format("{0}-{1}-{2}", i.CreateDate.Day.ToString("00"), i.CreateDate.Month.ToString("00"), i.CreateDate.Year));
            data.ForEach(i => i.CreateDateString = string.Format("{2}-{1}-{0}T{3}:{4}:{5}", i.CreateDate.Day.ToString("00"), i.CreateDate.Month.ToString("00"), i.CreateDate.Year, i.CreateDate.Hour.ToString("00"), i.CreateDate.Minute.ToString("00"), i.CreateDate.Second.ToString("00")));

            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);

            //return new JsonResult()
            //{
            //    Data = BusinessItemsLists.GetUsers().ToArray(),
            //    ContentType = "application/json",
            //    //ContentEncoding = contentEncoding,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            //    MaxJsonLength = Int32.MaxValue
            //};

            //return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }

        //public IEnumerable<object> Get()
        //{
        //    var result = BusinessItemsLists.GetUsers();

        //    var x = result.ToArray();

        //    return x;
        //}

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Create()
        {
            UserProfileModelObject model = new UserProfileModelObject()
            {
                //StartDate = DateTime.Now
            };

            return PartialView("Create", model);

            //return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> Create(UserProfileModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                UserProfileDTO newUser = new UserProfileDTO()
                {
                    Active = true,
                    ContactEmail = model.User.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    User = new UserDTO()
                    {
                        Email = model.User.Email,
                        UserName = model.User.Email
                    }
                };

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.User.Email, Email = model.User.Email };
                    user.EmailConfirmed = false;
                    string password = GeneratePassword();
                    var result = await this.UserManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        try
                        {
                            RoleDTO role = InsuranceBusiness.BusinessLayer.GetRoles().FirstOrDefault(i => i.Id == model.Role.Id);
                            result = await this.UserManager.AddToRoleAsync(user.Id, role.Name);
                        }
                        catch (Exception ex)
                        {
                            await this.UserManager.DeleteAsync(user);
                            return View(model);
                        }
                    }

                    long userId = -1;
                    if (result.Succeeded)
                    {
                        try
                        {
                            // Register default profile information!
                            userId = InsuranceBusiness.BusinessLayer.CreateDefaultUserProfile(user.Id, user.UserName, user.Email, model.FirstName, model.LastName, model.ProfessionalNumber);
                        }
                        catch (Exception ex)
                        {
                            await this.UserManager.DeleteAsync(user);
                            return View(model);
                        }
                    }

                    if (result.Succeeded)
                    {
                        try
                        {
                            InsuranceBusiness.BusinessLayer.CreateNotification(user.Id, null, NotificationTypeEnum.COMPLETE_PROFILE_INFO);

                            string token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            token = System.Web.HttpUtility.UrlEncode(token);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { code = user.Id, token = token }, protocol: Request.Url.Scheme);
                            await SendNewRegisterEmail(user, model.FirstName + " " + model.LastName, callbackUrl, password);

                        }
                        catch (Exception ex)
                        {
                            InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                            await this.UserManager.DeleteAsync(user);

                            return View(model);
                        }

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        //return View("RegisterToConfirm", model);
                    }
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        private static string GeneratePassword()
        {
            string password = Membership.GeneratePassword(8, 2);
            while (!PasswordUtils.ValidatePassword(password))
            {
                password = Membership.GeneratePassword(8, 2);
            }
            Random rnd = new Random();
            password = password + rnd.Next(1, 99);
            return password;
        }

        //[FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        //public async Task<ActionResult> GenerateNewConfirmationToken(string id)
        //{
        //    try
        //    {
        //        string password = GeneratePassword();

        //        var user = UserManager.FindById(id);
        //        var userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

        //        var result = await this.UserManager.(user, password);

        //        string token = await UserManager.GenerateEmailConfirmationTokenAsync(id);
        //        //token = System.Web.HttpUtility.UrlEncode(token);
        //        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { code = id, token = token }, protocol: Request.Url.Scheme);
        //        await SendNewRegisterEmail(user, userProfile.FirstName + " " + userProfile.LastName, callbackUrl, password);

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
        //        return View("Error");
        //    }
        //}

        private async Task<bool> SendNewRegisterEmail(ApplicationUser user, string name, string callbackUrl, string password)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(user.Email));

            m.Subject = Resources.Resources.EmailRegisterConfirmation;
            //m.Body = string.Format(Resources.Resources.RegisterConfirmationMessage, name, Url.Action("ConfirmEmail", "Account", new { token = code, code = user.Id }, Request.Url.Scheme));

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Account/EmailTemplates/RegisterConfirmationWithPasswordTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", name); //replacing the required things
            //m.Body = m.Body.Replace("{URL}", ConfigurationSettings.ApplicationSiteUrl);
            m.Body = m.Body.Replace("{URL}", callbackUrl);
            m.Body = m.Body.Replace("{PASSWORD}", password);
            m.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };

            smtp.Send(m);

            return true;
        }

        private async Task<bool> SendAccountActivation(ApplicationUser user, string name)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(user.Email));

            m.Subject = Resources.Resources.EmailRegisterConfirmation;
            //m.Body = string.Format(Resources.Resources.RegisterConfirmationMessage, name, Url.Action("ConfirmEmail", "Account", new { token = code, code = user.Id }, Request.Url.Scheme));

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Account/EmailTemplates/ActivationApprovedTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", name); //replacing the required things
            //m.Body = m.Body.Replace("{URL}", ConfigurationSettings.ApplicationSiteUrl);
            m.Body = m.Body.Replace("{URL}", ConfigurationSettings.ApplicationSiteUrl);
            m.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };

            smtp.Send(m);

            return true;
        }

        // GET: /User/Edit/5
        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Edit(long id = 0)
        {
            UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);

            UserProfileModelObject model = new UserProfileModelObject()
            {
                Active = profile.Active,
                Address = profile.Address,
                Birthdate = profile.Birthdate,
                ContactEmail = profile.ContactEmail,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ID = profile.ID,
                ID_User = profile.ID_User,
                Role = new RoleModelObject()
                {
                    Id = profile.Role.Id
                },
                User = new UserModelObject()
                {
                    Email = profile.User.Email,
                    EmailConfirmed = profile.User.EmailConfirmed,
                    LockoutEnabled = profile.User.LockoutEnabled,
                    LockoutEndDateUtc = profile.User.LockoutEndDateUtc,
                    Id = profile.User.Id,
                    UserName = profile.User.UserName
                }
            };

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> Edit(UserProfileModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID);

                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                //profile.User.EmailConfirmed = model.User.EmailConfirmed;

                InsuranceBusiness.BusinessLayer.UpdateProfile(profile);

                UserProfileDTO user = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID_User);

                if(user.Role.Id != model.Role.Id)
                {
                    RoleDTO roleToRemove = InsuranceBusiness.BusinessLayer.GetRoles().FirstOrDefault(i => i.Id == user.Role.Id);
                    var result = await this.UserManager.RemoveFromRoleAsync(model.ID_User, roleToRemove.Name);
                    RoleDTO roleToAssign = InsuranceBusiness.BusinessLayer.GetRoles().FirstOrDefault(i => i.Id == model.Role.Id);
                    result = await this.UserManager.AddToRoleAsync(model.ID_User, roleToAssign.Name);
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ConfirmUserDelete(long id)
        {
            ViewBag.profileId = id;
            return PartialView("Partial/_ConfirmUserDelete");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Delete(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeleteUserProfile(id);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> Activate(long id)
        {
            try
            {
                var userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);
                var user = UserManager.FindById(userProfile.ID_User);

                if (UserManager.IsLockedOut(userProfile.ID_User))
                {
                    InsuranceBusiness.BusinessLayer.ActivateUser(userProfile.ID);
                    //await UnlockUserAccount(userProfile.ID_User);
                    await SendAccountActivation(user, userProfile.FirstName);
                }

                //InsuranceBusiness.BusinessLayer.ActivateUser(id);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public async Task<ActionResult> Deactivate(long id)
        {
            try
            {
                //InsuranceBusiness.BusinessLayer.DeactivateUser(id);
                UserProfileDTO userProfile = InsuranceBusiness.BusinessLayer.GetUserProfile(id);
                InsuranceBusiness.BusinessLayer.DeactivateUser(userProfile.ID);
                //await LockUserAccount(userProfile.ID_User, 5000);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveAllowedEmailsDomains(UsersManagementViewModel model)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.UpdateEmailAuthorizedForAutomaticApproval(model.SelectedAllowedEmails);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, model.Profile.ID_User), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult DownloadConsultantStatistics()
        {
            try
            {
                UsersManagementViewModel model = new UsersManagementViewModel();

                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = UserManager.FindByName(this.User.Identity.Name);
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

                var gv = new System.Web.UI.WebControls.GridView();
                gv.DataSource = InsuranceBusiness.BusinessLayer.GetConsultantsStatistics();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Estatisticas_Colaboradores.xlsx");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();

                return View("Index", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0}", Request.UserHostAddress), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }
        }

        private ApplicationUserManager _userManager;
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

        public virtual async Task<IdentityResult> UnlockUserAccount(string userId)
        {
            var result = await UserManager.SetLockoutEnabledAsync(userId, false);
            if (result.Succeeded)
            {
                result = await UserManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MaxValue);
            }
            return result;
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
            else if (model is MessagesViewModel)
            {
                ((MessagesViewModel)model).Chats = InsuranceBusiness.BusinessLayer.GetChats(userId);
            }
        }
    }
}