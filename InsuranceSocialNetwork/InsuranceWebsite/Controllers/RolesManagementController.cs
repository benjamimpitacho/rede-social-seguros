using AutoMapper;
using InsuranceSocialNetworkBusiness;
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
    public class RolesManagementController : Controller
    {
        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoleDTO, RoleModelObject>();
            cfg.CreateMap<RoleModelObject, RoleDTO>();
        });

        public RolesManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("ROLES_MANAGEMENT")]
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
                                i.FirstName.ToLower().Contains(globalSearch.ToLower())
                                || i.LastName.ToLower().Contains(globalSearch.ToLower())
                                || i.User.UserName.ToLower().Contains(globalSearch.ToLower())
                                || i.ContactEmail.ToLower().Contains(globalSearch.ToLower())
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

        [FunctionalityAutorizeAttribute("ROLES_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}