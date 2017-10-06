using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Post;
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
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class UsersManagementController : Controller
    {
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
        public async Task<ActionResult> Index()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();

            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                if (null != user)
                {
                    model.Profile = InsuranceBusiness.BusinessLayer.GetUserProfile(user.Id);
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
                    cols.Add("CreateDate")
                        .WithHeaderText(Resources.Resources.RegisterDate)
                        .WithCellCssClassExpression(p => "dateCell")
                        .WithValueExpression(p => p.CreateDate.ToString("dd-MM-yyyy"))
                        .WithVisibility(true, true);
                    cols.Add("Active").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText(Resources.Resources.Active)
                        .WithValueExpression((p, c) => p.User.EmailConfirmed ? "checked" : "")
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
                .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
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
                        }
                    }

                    string globalSearch = options.GetAdditionalQueryOptionString("Search");
                    if (null != globalSearch)
                    {
                        query = query.Where(i =>
                            (null != i.FirstName && i.FirstName.Contains(globalSearch))
                            || (null != i.LastName && i.LastName.Contains(globalSearch))
                            || (null != i.User && null != i.User.UserName && i.User.UserName.Contains(globalSearch))
                            //|| (null != i.ContactEmail && i.ContactEmail.Contains(globalSearch))
                        ).ToList();
                    }

                    return new QueryResult<UserProfileModelObject>()
                    {
                        Items = query,
                        TotalRecords = query.Count // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
            }

            return View(model);
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Create()
        {
            UserProfileModelObject model = new UserProfileModelObject()
            {
                //StartDate = DateTime.Now
            };

            return PartialView(model);
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
                    string password = Membership.GeneratePassword(8, 2);
                    Random rnd = new Random();
                    password = password + rnd.Next(1, 99);
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
                            userId = InsuranceBusiness.BusinessLayer.CreateDefaultUserProfile(user.Id, user.UserName, user.Email, model.FirstName, model.LastName);
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

                            //await SendConfirmationEmail(user, model.Name);
                        }
                        catch (Exception ex)
                        {
                            InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                            await this.UserManager.DeleteAsync(user);

                            return View(model);
                        }
                        
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        //return View("RegisterToConfirm", model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
        public ActionResult Edit(UserProfileModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(model.ID);

                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                //profile.User.EmailConfirmed = model.User.EmailConfirmed;

                InsuranceBusiness.BusinessLayer.UpdateProfile(profile);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Activate(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.ActivateUser(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("USERS_MANAGEMENT")]
        public ActionResult Deactivate(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeactivateUser(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
    }
}