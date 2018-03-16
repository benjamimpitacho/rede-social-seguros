using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.Role;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using InsuranceWebsite.Utils;
using Microsoft.AspNet.Identity.Owin;
using MVCGrid.Models;
using MVCGrid.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class NotificationsManagementController : Controller
    {
        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoleDTO, RoleModelObject>();
            cfg.CreateMap<RoleModelObject, RoleDTO>();
        });

        public NotificationsManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("ALERTS_MANAGEMENT")]
        public async Task<ActionResult> Index()
        {
            RolesManagementViewModel model = new RolesManagementViewModel();

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
                var grid = MVCGridDefinitionTable.GetDefinition<UserProfileModelObject>("RolesManagementGrid");
            }
            catch(Exception)
            {
                //InsuranceBusiness.BusinessLayer.GetRoles

                MVCGridDefinitionTable.Add("RolesManagementGrid", new MVCGridBuilder<UserProfileModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Username")
                                .WithHeaderText(Resources.Resources.Username)
                                .WithValueExpression(i => i.User.UserName)
                                .WithAllowChangeVisibility(true)
                                .WithFiltering(true)
                                .WithSorting(true)
                                .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("FirstName")
                                .WithHeaderText(Resources.Resources.Name)
                                .WithValueExpression(i => i.FirstName)
                                .WithAllowChangeVisibility(true)
                                .WithFiltering(true)
                                .WithSorting(true)
                                .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("CreateDate")
                                .WithHeaderText(Resources.Resources.RegisterDate)
                                .WithCellCssClassExpression(p => "dateCell")
                                .WithValueExpression(p => p.CreateDate.ToString("dd-MM-yyyy"));
                        cols.Add("Activate")
                            .WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithCellCssClassExpression(p => "controlCell")
                            //.WithPlainTextValueExpression((p, c) => p.User.EmailConfirmed ? "deactivate" : "activate")
                            .WithValueExpression((p, c) => c.UrlHelper.Action(p.User.EmailConfirmed ? "Deactivate" : "Activate", "UsersManagement", new { id = p.ID }))
                            .WithValueTemplate("<a href='{Value}' class='' role='button' style='margin-right:5px;color:limegreen;'>" + MVCGridConfig.GetActivateCommandCode("{Model.User.EmailConfirmed}") + "</a>" + "<a href='UsersManagement/BlockUser/{Model.ID}' class='' role='button' style='margin-right:5px;color:red'>" + MVCGridConfig.GetBlockCommandCode("{Model.User.EmailConfirmed}") + "</a>");
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "FirstName")
                    .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
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
                                    //query = query.OrderBy(p => p.LastName, options.SortDirection);
                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                i.FirstName.Contains(globalSearch)
                                || i.LastName.Contains(globalSearch)
                                || i.User.UserName.Contains(globalSearch)
                                || i.ContactEmail.Contains(globalSearch)
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
                    })
                );
            }

            return View(model);
        }

        //[FunctionalityAutorizeAttribute("ROLES_MANAGEMENT")]
        //public ActionResult RolesManagement()
        //{
        //    RolesManagementViewModel model = new RolesManagementViewModel();

        //    return View(model);
        //}

        [FunctionalityAutorizeAttribute("ALERTS_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public void EasypayPaymentNotification(string ep_cin, string ep_user, string ep_doc, string ep_key)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "", string.Format("Easypay payment notification received"), string.Format("Easypay infromation received is ep_cin {0}, ep_user {1}, ep_doc {2}, ep_key {3},", ep_cin , ep_user, ep_doc, ep_key));

                PaymentDTO payment = InsuranceBusiness.BusinessLayer.GetPayment(ep_key);
                if(null == payment || payment.ep_cin != ep_cin || payment.ep_user != ep_user)
                {
                    throw new Exception("INVALID_PAYMENT_INFORMATION");
                }
                payment.ep_key = ep_key;
                payment.ep_doc = ep_doc;
                if(InsuranceBusiness.BusinessLayer.ConfirmEasypayPayment(payment))
                {
                    if (payment.ID_Profile.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateUser(payment.ID_Profile.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(false, payment.ID_Profile.Value);

                    }
                    else if (payment.ID_Garage.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateGarage(payment.ID_Garage.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(true, payment.ID_Garage.Value, CompanyTypeEnum.GARAGE);
                    }
                    else if (payment.ID_ConstructionCompany.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateConstructionCompany(payment.ID_ConstructionCompany.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(true, payment.ID_ConstructionCompany.Value, CompanyTypeEnum.GARAGE);
                    }
                    else if (payment.ID_HomeApplianceRepair.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateHomeApplianceRepair(payment.ID_HomeApplianceRepair.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(true, payment.ID_HomeApplianceRepair.Value, CompanyTypeEnum.GARAGE);
                    }
                    else if (payment.ID_InsuranceCompanyContact.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateInsuranceCompanyContact(payment.ID_InsuranceCompanyContact.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(true, payment.ID_InsuranceCompanyContact.Value, CompanyTypeEnum.GARAGE);
                    }
                    else if (payment.ID_MedicalClinic.HasValue)
                    {
                        // Activate user
                        InsuranceBusiness.BusinessLayer.ActivateMedicalClinic(payment.ID_MedicalClinic.Value);
                        // Send user notification
                        InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(true, payment.ID_MedicalClinic.Value, CompanyTypeEnum.GARAGE);
                    }
                }
            }
            catch(Exception ex)
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "", string.Format("Easypay payment notification ERROR"), ex.Message + "\r\n" + ex.StackTrace);
                throw new NotImplementedException();
            }
        }
    }
}