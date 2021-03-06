﻿using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Utils;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.PostalCode;
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
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class CompaniesManagementController : Controller
    {
        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CompanyDTO, CompanyModelObject>();
            cfg.CreateMap<CompanyModelObject, CompanyDTO>();
        });

        public CompaniesManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public async Task<ActionResult> Index(CompanyTypeEnum id)
        {
            CompaniesManagementViewModel model = new CompaniesManagementViewModel();

            model.CompanyType = id;
            //switch (id)
            //{
            //    case 1:
            //        model.CompanyType = CompanyTypeEnum.GARAGE;
            //        break;
            //    case 2:
            //        model.CompanyType = CompanyTypeEnum.MEDICAL_CLINIC;
            //        break;
            //    case 3:
            //        model.CompanyType = CompanyTypeEnum.CONSTRUCTION_COMPANY;
            //        break;
            //    case 4:
            //        model.CompanyType = CompanyTypeEnum.HOME_APPLIANCES_REPAIR;
            //        break;
            //    case 5:
            //        model.CompanyType = CompanyTypeEnum.INSURANCE_COMPANY_CONTACT;
            //        break;
            //}

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

                model.TotalUnreadMessages = InsuranceBusiness.BusinessLayer.GetTotalUnreadMessages(user.Id); ;
                model.Notifications = InsuranceBusiness.BusinessLayer.GetUserNotifications(user.Id);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("GarageManagementGrid");
                grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("MedicalClinicManagementGrid");
                grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("ConstructionCompanyManagementGrid");
                grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("HomeAppliancesManagementGrid");
                grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("InsuranceManagementGrid");
            }
            catch (Exception)
            {
                MVCGridDefinitionTable.Add("GarageManagementGrid", new MVCGridBuilder<CompanyModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Name")
                            .WithHeaderText(Resources.Resources.Name)
                            .WithValueExpression(i => i.Name)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("NIF")
                            .WithHeaderText(Resources.Resources.NIF)
                            .WithValueExpression(i => i.NIF)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Description")
                            .WithHeaderText(Resources.Resources.Description)
                            .WithValueExpression(i => i.Description)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("ContactEmail")
                            .WithHeaderText(Resources.Resources.ContactEmail)
                            .WithValueExpression(i => i.ContactEmail)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Website")
                            .WithHeaderText(Resources.Resources.Website)
                            .WithValueExpression(i => i.Website)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Active").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(Resources.Resources.Active)
                            .WithValueExpression((p, c) => p.Active ? "checked" : "")
                            .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                            .WithCellCssClassExpression(p => "mvcgrid-cell")
                            .WithVisibility(true, true);
                        cols.Add("Edit").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit' onclick='hello()'>" + Resources.Resources.Edit + "</a>")
                            .WithVisibility(true, false);
                        cols.Add("Delete").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                            .WithVisibility(true, false);
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "Name")
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
                        var result = new QueryResult<CompanyModelObject>();
                        List<CompanyModelObject> query = new List<CompanyModelObject>();

                        query = BusinessItemsLists.GetGarages();

                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "name":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.Name).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.Name).ToList();
                                    }

                                    break;
                                case "nif":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.NIF).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.NIF).ToList();
                                    }

                                    break;
                                case "contactemail":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.ContactEmail).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.ContactEmail).ToList();
                                    }

                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                (null != i.Name && i.Name.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.NIF && i.NIF.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.ContactEmail && i.ContactEmail.ToLower().Contains(globalSearch.ToLower()))
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
                    })
                );

                MVCGridDefinitionTable.Add("MedicalClinicManagementGrid", new MVCGridBuilder<CompanyModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Name")
                            .WithHeaderText(Resources.Resources.Name)
                            .WithValueExpression(i => i.Name)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("NIF")
                            .WithHeaderText(Resources.Resources.NIF)
                            .WithValueExpression(i => i.NIF)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Description")
                            .WithHeaderText(Resources.Resources.Description)
                            .WithValueExpression(i => i.Description)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("ContactEmail")
                            .WithHeaderText(Resources.Resources.ContactEmail)
                            .WithValueExpression(i => i.ContactEmail)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Website")
                            .WithHeaderText(Resources.Resources.Website)
                            .WithValueExpression(i => i.Website)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Active").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(Resources.Resources.Active)
                            .WithValueExpression((p, c) => p.Active ? "checked" : "")
                            .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                            .WithCellCssClassExpression(p => "mvcgrid-cell")
                            .WithVisibility(true, true);
                        cols.Add("Edit").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit' onclick='hello()'>" + Resources.Resources.Edit + "</a>")
                            .WithVisibility(true, false);
                        cols.Add("Delete").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                            .WithVisibility(true, false);
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "Name")
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
                        var result = new QueryResult<CompanyModelObject>();
                        List<CompanyModelObject> query = new List<CompanyModelObject>();

                        query = BusinessItemsLists.GetMedicalClinis();

                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "name":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.Name).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.Name).ToList();
                                    }

                                    break;
                                case "nif":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.NIF).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.NIF).ToList();
                                    }

                                    break;
                                case "contactemail":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.ContactEmail).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.ContactEmail).ToList();
                                    }

                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                (null != i.Name && i.Name.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.NIF && i.NIF.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.ContactEmail && i.ContactEmail.ToLower().Contains(globalSearch.ToLower()))
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
                    })
                );

                MVCGridDefinitionTable.Add("ConstructionCompanyManagementGrid", new MVCGridBuilder<CompanyModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Name")
                            .WithHeaderText(Resources.Resources.Name)
                            .WithValueExpression(i => i.Name)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("NIF")
                            .WithHeaderText(Resources.Resources.NIF)
                            .WithValueExpression(i => i.NIF)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Description")
                            .WithHeaderText(Resources.Resources.Description)
                            .WithValueExpression(i => i.Description)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("ContactEmail")
                            .WithHeaderText(Resources.Resources.ContactEmail)
                            .WithValueExpression(i => i.ContactEmail)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Website")
                            .WithHeaderText(Resources.Resources.Website)
                            .WithValueExpression(i => i.Website)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Active").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(Resources.Resources.Active)
                            .WithValueExpression((p, c) => p.Active ? "checked" : "")
                            .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                            .WithCellCssClassExpression(p => "mvcgrid-cell")
                            .WithVisibility(true, true);
                        cols.Add("Edit").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit' onclick='hello()'>" + Resources.Resources.Edit + "</a>")
                            .WithVisibility(true, false);
                        cols.Add("Delete").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                            .WithVisibility(true, false);
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "Name")
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
                        var result = new QueryResult<CompanyModelObject>();
                        List<CompanyModelObject> query = new List<CompanyModelObject>();

                        query = BusinessItemsLists.GetConstructionCompanies();

                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "name":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.Name).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.Name).ToList();
                                    }

                                    break;
                                case "nif":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.NIF).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.NIF).ToList();
                                    }

                                    break;
                                case "contactemail":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.ContactEmail).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.ContactEmail).ToList();
                                    }

                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                (null != i.Name && i.Name.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.NIF && i.NIF.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.ContactEmail && i.ContactEmail.ToLower().Contains(globalSearch.ToLower()))
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
                    })
                );

                MVCGridDefinitionTable.Add("HomeAppliancesManagementGrid", new MVCGridBuilder<CompanyModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Name")
                            .WithHeaderText(Resources.Resources.Name)
                            .WithValueExpression(i => i.Name)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("NIF")
                            .WithHeaderText(Resources.Resources.NIF)
                            .WithValueExpression(i => i.NIF)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Description")
                            .WithHeaderText(Resources.Resources.Description)
                            .WithValueExpression(i => i.Description)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("ContactEmail")
                            .WithHeaderText(Resources.Resources.ContactEmail)
                            .WithValueExpression(i => i.ContactEmail)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Website")
                            .WithHeaderText(Resources.Resources.Website)
                            .WithValueExpression(i => i.Website)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Active").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(Resources.Resources.Active)
                            .WithValueExpression((p, c) => p.Active ? "checked" : "")
                            .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                            .WithCellCssClassExpression(p => "mvcgrid-cell")
                            .WithVisibility(true, true);
                        cols.Add("Edit").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit' onclick='hello()'>" + Resources.Resources.Edit + "</a>")
                            .WithVisibility(true, false);
                        cols.Add("Delete").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                            .WithVisibility(true, false);
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "Name")
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
                        var result = new QueryResult<CompanyModelObject>();
                        List<CompanyModelObject> query = new List<CompanyModelObject>();

                        query = BusinessItemsLists.GetHomeApplianceRepairs();

                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "name":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.Name).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.Name).ToList();
                                    }

                                    break;
                                case "nif":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.NIF).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.NIF).ToList();
                                    }

                                    break;
                                case "contactemail":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.ContactEmail).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.ContactEmail).ToList();
                                    }

                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                (null != i.Name && i.Name.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.NIF && i.NIF.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.ContactEmail && i.ContactEmail.ToLower().Contains(globalSearch.ToLower()))
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
                    })
                );

                MVCGridDefinitionTable.Add("InsuranceManagementGrid", new MVCGridBuilder<CompanyModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Name")
                            .WithHeaderText(Resources.Resources.Name)
                            .WithValueExpression(i => i.Name)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("NIF")
                            .WithHeaderText(Resources.Resources.NIF)
                            .WithValueExpression(i => i.NIF)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Description")
                            .WithHeaderText(Resources.Resources.Description)
                            .WithValueExpression(i => i.Description)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("ContactEmail")
                            .WithHeaderText(Resources.Resources.ContactEmail)
                            .WithValueExpression(i => i.ContactEmail)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add().WithColumnName("Website")
                            .WithHeaderText(Resources.Resources.Website)
                            .WithValueExpression(i => i.Website)
                            .WithAllowChangeVisibility(true)
                            .WithFiltering(true)
                            .WithSorting(true)
                            .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Active").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(Resources.Resources.Active)
                            .WithValueExpression((p, c) => p.Active ? "checked" : "")
                            .WithValueTemplate("<input type=\"checkbox\" disabled=\"disabled\" {Value}>")
                            .WithCellCssClassExpression(p => "mvcgrid-cell")
                            .WithVisibility(true, true);
                        cols.Add("Edit").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit' onclick='hello()'>" + Resources.Resources.Edit + "</a>")
                            .WithVisibility(true, false);
                        cols.Add("Delete").WithHtmlEncoding(false)
                            .WithSorting(false)
                            .WithHeaderText(" ")
                            .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID, idType = id }))
                            .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>")
                            .WithVisibility(true, false);
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "Name")
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
                        var result = new QueryResult<CompanyModelObject>();
                        List<CompanyModelObject> query = new List<CompanyModelObject>();

                        query = BusinessItemsLists.GetInsuranceCompanyContacts();

                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "name":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.Name).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.Name).ToList();
                                    }

                                    break;
                                case "nif":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.NIF).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.NIF).ToList();
                                    }

                                    break;
                                case "contactemail":
                                    if (options.SortDirection == SortDirection.Asc
                                        || options.SortDirection == SortDirection.Unspecified)
                                    {
                                        query = query.OrderBy(p => p.ContactEmail).ToList();
                                    }
                                    else
                                    {
                                        query = query.OrderByDescending(p => p.ContactEmail).ToList();
                                    }

                                    break;
                            }
                        }

                        string globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (null != globalSearch)
                        {
                            query = query.Where(i =>
                                (null != i.Name && i.Name.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.NIF && i.NIF.ToLower().Contains(globalSearch.ToLower()))
                                || (null != i.ContactEmail && i.ContactEmail.ToLower().Contains(globalSearch.ToLower()))
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
                    })
                );
            }

            return View(model);
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Create(CompanyTypeEnum id)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            try
            {
                CompanyModelObject model = new CompanyModelObject();

                decimal subscriptionValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT).Value, System.Globalization.CultureInfo.InvariantCulture);
                decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value, System.Globalization.CultureInfo.InvariantCulture);
                model.ID_PaymentType = InsuranceBusiness.BusinessLayer.GetPaymentTypeID(PaymentTypeEnum.ATM);
                model.Value = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero);

                List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectService } };
                model.ServiceList = initList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(id).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
                model.PaymentTypeList = initList.Concat(InsuranceBusiness.BusinessLayer.GetPaymentTypes().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList()).ToList();
                model.PaymentPeriodList = InsuranceBusiness.BusinessLayer.GetPaymentPeriods().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList();
                model.CompanyType = id;
                model.CreatePayment = true;

                return PartialView(model);
            }
            catch(Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return PartialView("Error");
            }
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public async Task<ActionResult> Create(CompanyModelObject model, HttpPostedFileBase imgUpload)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            try
            {
                UserProfileDTO profile = null;
                if (null != this.User && this.User.Identity.IsAuthenticated)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var admin_user = await UserManager.FindByNameAsync(this.User.Identity.Name);
                    if (null != admin_user)
                    {
                        profile = InsuranceBusiness.BusinessLayer.GetUserProfile(admin_user.Id);
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

                PaymentDTO payment = null;
                CompanyDTO newCompany = new CompanyDTO()
                {
                    ID = model.ID,
                    Active = model.Active,
                    Address = model.Address,
                    ContactEmail = model.ContactEmail,
                    Description = model.Description,
                    ID_District = model.ID_District,
                    ID_County = model.ID_County,
                    ID_Parish = model.ID_Parish,
                    ID_Service = model.ID_Service,
                    MobilePhone_1 = model.MobilePhone_1,
                    MobilePhone_2 = model.MobilePhone_2,
                    Name = model.Name,
                    NIF = model.NIF,
                    OfficialAgent = model.OfficialAgent,
                    OfficialPartner = model.OfficialPartner,
                    Telephone_1 = model.Telephone_1,
                    Telephone_2 = model.Telephone_2,
                    Website = model.Website,
                    BusinessName = model.BusinessName,
                    IBAN = model.IBAN,
                    ID_User_CreatedBy = profile.ID_User
                };

                if (null != imgUpload)
                {
                    newCompany.LogoPhoto = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 400, 400);
                }

                UserProfileDTO newUser = new UserProfileDTO()
                {
                    Active = true,
                    ContactEmail = model.ContactEmail,
                    FirstName = string.IsNullOrEmpty(model.BusinessName)?model.Name:model.BusinessName,
                    LastName = string.Empty,
                    User = new UserDTO()
                    {
                        Email = model.ContactEmail,
                        UserName = model.ContactEmail
                    }
                };

                if (newUser.FirstName.Length > 32)
                {
                    string[] names = newUser.FirstName.Split(new char[] { ' ' });
                    string tempName = newUser.FirstName;
                    string newFirstName = "";
                    string newLastName = "";

                    int index = 0;
                    foreach(string name in names)
                    {
                        if((newFirstName.Length + name.Length) < 32)
                        {
                            if (index == 0)
                            {
                                newFirstName = name;
                            }
                            else
                            {
                                newFirstName += " " + name;
                            }
                        }
                        else
                        {
                            if(index==0)
                            {
                                newFirstName = name.Substring(0, 32);
                                if(name.Length > 64)
                                {
                                    newLastName = name.Substring(32, 64);
                                }
                                else
                                {
                                    newLastName = name.Substring(32);
                                }
                                break;
                            }
                            else
                            {
                                if ((newLastName.Length + name.Length) < 32)
                                {
                                    if (newLastName.Length == 0)
                                    {
                                        newLastName = name;
                                    }
                                    else
                                    {
                                        newLastName += " " + name;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    newUser.FirstName = newFirstName;
                    newUser.LastName = newLastName;
                }

                
                var user = new ApplicationUser { UserName = model.ContactEmail, Email = model.ContactEmail };
                user.EmailConfirmed = true;
                string password = GeneratePassword();
                var createUserResult = await this.UserManager.CreateAsync(user, password);

                if (createUserResult.Succeeded)
                {
                    try
                    {
                        RoleDTO role = InsuranceBusiness.BusinessLayer.GetRoles().FirstOrDefault(i => i.Name == RoleEnum.DIRECTORY_COMPANY.ToString());
                        createUserResult = await this.UserManager.AddToRoleAsync(user.Id, role.Name);
                    }
                    catch (Exception ex)
                    {
                        await this.UserManager.DeleteAsync(user);
                        return View(model);
                    }
                }
                else
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("Cannot create user. Errors: {0} ", string.Concat(createUserResult.Errors)));
                    return View("Error", new ErrorViewModel(new Exception(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()) { ErrorMessage = string.Concat(createUserResult.Errors) });
                }

                long userId = -1;
                if (createUserResult.Succeeded)
                {
                    try
                    {
                        // Register default profile information!
                        if (newUser.FirstName.Length < 32)
                        {
                            userId = InsuranceBusiness.BusinessLayer.CreateDefaultUserProfile(user.Id, user.UserName, user.Email, newUser.FirstName, newUser.LastName, null);
                        }
                        else
                        {
                            string firstName = newUser.FirstName.Substring(0, 32);
                            string lastName = newUser.FirstName.Substring(32);
                            if(lastName.Length > 31)
                            {
                                lastName = lastName.Substring(0, 32);
                            }
                            userId = InsuranceBusiness.BusinessLayer.CreateDefaultUserProfile(user.Id, user.UserName, user.Email, firstName, lastName, null);
                        }
                        await LockUserAccount(user.Id, 5000);
                    }
                    catch (Exception ex)
                    {
                        await this.UserManager.DeleteAsync(user);
                        return View(model);
                    }
                }
                else
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("Cannot create user. Errors: {0} ", string.Concat(createUserResult.Errors)));
                    return View("Error", new ErrorViewModel(new Exception(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()) { ErrorMessage = string.Concat(createUserResult.Errors) });
                }

                if (createUserResult.Succeeded)
                {
                    try
                    {
                        InsuranceBusiness.BusinessLayer.CreateNotification(user.Id, null, NotificationTypeEnum.COMPLETE_PROFILE_INFO);

                        //string token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { code = user.Id, token = token }, protocol: Request.Url.Scheme);
                        //await SendNewRegisterEmail(user, model.FirstName + " " + model.LastName, callbackUrl, password);

                    }
                    catch (Exception ex)
                    {
                        InsuranceBusiness.BusinessLayer.DeleteUserProfile(userId);
                        await this.UserManager.DeleteAsync(user);

                        return View(model);
                    }
                }
                else
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("Cannot create user. Errors: {0} ", string.Concat(createUserResult.Errors)));
                    return View("Error", new ErrorViewModel(new Exception(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()) { ErrorMessage = string.Concat(createUserResult.Errors) });
                }

                newCompany.ID_User = user.Id;

                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        newCompany.ID = InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        newCompany.ID = InsuranceBusiness.BusinessLayer.CreateMedicalClinic(newCompany);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        newCompany.ID = InsuranceBusiness.BusinessLayer.CreateConstructionCompany(newCompany);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        newCompany.ID = InsuranceBusiness.BusinessLayer.CreateHomeApplianceRepair(newCompany);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        newCompany.ID = InsuranceBusiness.BusinessLayer.CreateInsuranceCompanyContact(newCompany);
                        break;
                    default:
                        break;
                }

                if (model.CreatePayment && model.ID_PaymentType > 0)
                {
                    Guid paymentId = Guid.NewGuid();
                    string baseUrl = string.Format("{0}?ep_cin={1}&ep_user={2}&ep_entity={3}&ep_ref_type={4}&ep_country={5}&ep_language={6}&t_value={7}&t_key={8}"
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_CIN).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_USER).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_ENTITY).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value
                        , model.Value.ToString().Replace(",", ".")
                        , paymentId.ToString());

                    using (var client = new WebClient())
                    {
                        XmlDocument response = new XmlDocument();
                        bool skipProcessing = false;
                        switch (model.ID_PaymentType)
                        {
                            case (int)PaymentTypeEnum.ATM:
                                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "CompaniesManagementController::Create", string.Format("Base URL: {0}", baseUrl));
                                var result = client.DownloadString(baseUrl);
                                response.LoadXml(result);
                                break;
                            case (int)PaymentTypeEnum.DIRECT_DEBIT:
                                skipProcessing = true;
                                break;
                            default:
                                skipProcessing = true;
                                break;
                        }

                        if (!skipProcessing)
                        {
                            decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value, System.Globalization.CultureInfo.InvariantCulture);
                            //decimal totalValue = 0;
                            //if(!decimal.TryParse(model.Value.Replace(",", "."), out totalValue))
                            //{
                            //    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR,);
                            //}

                            decimal liquidValue = model.Value - decimal.Round(((model.Value / (1 + vatValue)) - model.Value) * -1, 2, MidpointRounding.AwayFromZero);
                            payment = new PaymentDTO()
                            {
                                CreateDate = DateTime.Now,
                                LastChangeDate = DateTime.Now,
                                ID_PaymentType = model.ID_PaymentType,
                                Payment_GUID = paymentId,
                                Active = true,
                                t_value = model.Value.ToString().Replace(",", "."),
                                LiquidValue = liquidValue,
                                TotalValue = model.Value,
                                Title = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_TITLE).Value,
                                Description = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_DESCRIPTION).Value,
                                TaxValue = vatValue,
                                //ExpiracyDate = DateTime.Now.AddDays(Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SUBSCRIPTION_PAYMENT_DEADLINE_DAYS).Value))
                                ExpiracyDate = DateTime.Now.AddYears(model.PaymentPeriod)
                            };
                            var node = response.SelectSingleNode("getautoMB/ep_status");
                            if (node.InnerText.Equals("ok0"))
                            {
                                // Sucesso
                                payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                                payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                                payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                                payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                                payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                                payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                                payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                                payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                                payment.ep_rec_url = "";
                                payment.ep_reference = response.SelectSingleNode("getautoMB/ep_reference").InnerText;
                                payment.ep_value = response.SelectSingleNode("getautoMB/ep_value").InnerText;
                                payment.t_key = response.SelectSingleNode("getautoMB/t_key").InnerText;
                                payment.ep_link = response.SelectSingleNode("getautoMB/ep_link").InnerText;
                                payment.ID_PaymentStatus = (int)PaymentStatusEnum.PENDING;
                            }
                            else
                            {
                                // Erro
                                payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                                payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                                payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                                payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                                payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                                payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                                payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                                payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                                payment.ep_rec_url = "";
                                payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;

                                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), string.Format("Cannot create easypay reference. Errors: {0} ", payment.ep_message));
                            }

                            if (null == newCompany.Payment)
                            {
                                newCompany.Payment = new List<PaymentDTO>();
                            }
                            switch (model.CompanyType)
                            {
                                case CompanyTypeEnum.GARAGE:
                                    payment.ID_Garage = newCompany.ID;
                                    break;
                                case CompanyTypeEnum.MEDICAL_CLINIC:
                                    payment.ID_MedicalClinic= newCompany.ID;
                                    break;
                                case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                                    payment.ID_ConstructionCompany = newCompany.ID;
                                    break;
                                case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                                    payment.ID_HomeApplianceRepair = newCompany.ID;
                                    break;
                                case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                                    payment.ID_InsuranceCompanyContact = newCompany.ID;
                                    break;
                                default:
                                    break;
                            }
                            newCompany.Payment.Add(payment);
                        }
                    }
                }
                //else
                //{
                //    // User doesn't have payment created, so won't be activated on payment notification
                //    InsuranceBusiness.BusinessLayer.ActivateUser(newCompany.ID_User);
                //}

                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.EditGarage(newCompany);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.EditMedicalClinic(newCompany);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.EditConstructionCompany(newCompany);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.EditHomeApplianceRepair(newCompany);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.EditInsuranceCompanyContact(newCompany);
                        break;
                    default:
                        break;
                }

                SendRegistWelcomeEmail(newCompany, payment, password);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index", new { id = model.CompanyType });
        }

        private bool SendRegistWelcomeEmail(CompanyDTO company, PaymentDTO payment, string password)
        {
            UserProfileDTO profile = null;
            string cc_mail = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.PLATFORM_EMAIL).Value;
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var admin_user = UserManager.FindByName(this.User.Identity.Name);
                if (null != admin_user)
                {
                    profile = InsuranceBusiness.BusinessLayer.GetUserProfile(admin_user.Id);
                    cc_mail = profile.User.UserName;
                }
            }

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.PLATFORM_EMAIL).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(company.ContactEmail));

            m.CC.Add(new MailAddress(cc_mail));
            m.Subject = Resources.Resources.EmailCompanyRegistWelcome;

            if(null == payment)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/CompaniesManagement/EmailTemplates/RegistWelcomeTemplate.html")))
                {
                    m.Body = reader.ReadToEnd();
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/CompaniesManagement/EmailTemplates/RegistWelcomeTemplateWithPayment.html")))
                {
                    m.Body = reader.ReadToEnd();
                }
            }            

            m.Body = m.Body.Replace("{NAME}", string.IsNullOrEmpty(company.BusinessName) ? company.Name : company.BusinessName); //replacing the required things
            m.Body = m.Body.Replace("{URL}", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.APPLICATION_SITE_URL).Value);

            m.Body = m.Body.Replace("{USERNAME}", company.ContactEmail);
            m.Body = m.Body.Replace("{PASSWORD}", password);

            if (null != payment)
            {
                m.Body = m.Body.Replace("{ENTITY}", payment.ep_entity);
                m.Body = m.Body.Replace("{REFERENCE}", payment.ep_reference);
                m.Body = m.Body.Replace("{AMOUNT}", payment.ep_value);
            }
            //else
            //{
            //    m.Body = m.Body.Replace("{ENTITY}", string.Empty);
            //    m.Body = m.Body.Replace("{REFERENCE}", string.Empty);
            //    m.Body = m.Body.Replace("{AMOUNT}", string.Empty);
            //}
            m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };
            smtp.Send(m);

            return true;
        }

        // GET: /User/Edit/5
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Edit(long id, CompanyTypeEnum idType)
        {
            CompanyDTO company = null;

            switch(idType)
            {
                case CompanyTypeEnum.GARAGE:
                    company = InsuranceBusiness.BusinessLayer.GetGarage(id);
                    break;
                case CompanyTypeEnum.MEDICAL_CLINIC:
                    company = InsuranceBusiness.BusinessLayer.GetMedicalClinic(id);
                    break;
                case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                    company = InsuranceBusiness.BusinessLayer.GetConstructionCompany(id);
                    break;
                case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                    company = InsuranceBusiness.BusinessLayer.GetHomeApplianceRepair(id);
                    break;
                case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                    company = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContact(id);
                    break;
            }

            decimal subscriptionValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT).Value);
            decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value);

            CompanyModelObject model = new CompanyModelObject()
            {
                ID = company.ID,
                Active = company.Active,
                Name = company.Name,
                Description = company.Description,
                Address = company.Address,
                ContactEmail = company.ContactEmail,
                ID_District = company.ID_District,
                ID_County = company.ID_County,
                ID_Parish = company.ID_Parish,
                ID_Service = company.ID_Service,
                LogoPhoto = company.LogoPhoto,
                MobilePhone_1 = company.MobilePhone_1,
                MobilePhone_2 = company.MobilePhone_2,
                NIF = company.NIF,
                OfficialAgent = company.OfficialAgent,
                OfficialPartner = company.OfficialPartner,
                Telephone_1 = company.Telephone_1,
                Telephone_2 = company.Telephone_2,
                Website = company.Website,
                Payments = company.Payment,
                CompanyType = idType,
                ID_PaymentType = InsuranceBusiness.BusinessLayer.GetPaymentTypeID(PaymentTypeEnum.ATM),
                Value = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero),
                CreatePayment = false
            };

            List<SelectListItem> initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectService } };
            model.ServiceList = initList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(idType).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            model.PaymentTypeList = initList.Concat(InsuranceBusiness.BusinessLayer.GetPaymentTypes().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList()).ToList();
            model.PaymentPeriodList = InsuranceBusiness.BusinessLayer.GetPaymentPeriods().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList();

            if (model.ID_District.HasValue)
            {
                model.CountyList = model.CountyList.Concat(InsuranceBusiness.BusinessLayer.GetCountiesByDistrict(model.ID_District.Value).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            }

            return PartialView(model);
        }

        private bool SendPaymentGeneratedEmail(CompanyDTO company, PaymentDTO payment)
        {
            UserProfileDTO profile = null;
            string cc_mail = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.PLATFORM_EMAIL).Value;
            if (null != this.User && this.User.Identity.IsAuthenticated)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var admin_user = UserManager.FindByName(this.User.Identity.Name);
                if (null != admin_user)
                {
                    profile = InsuranceBusiness.BusinessLayer.GetUserProfile(admin_user.Id);
                    cc_mail = profile.User.UserName;
                }
            }

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.PLATFORM_EMAIL).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(company.ContactEmail));

            m.CC.Add(new MailAddress(cc_mail));
            m.Subject = Resources.Resources.EmailPaymentInformation;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/CompaniesManagement/EmailTemplates/PaymentGeneratedTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", string.IsNullOrEmpty(company.BusinessName) ? company.Name : company.BusinessName); //replacing the required things
            m.Body = m.Body.Replace("{URL}", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.APPLICATION_SITE_URL).Value);
            if (null != payment)
            {
                m.Body = m.Body.Replace("{ENTITY}", payment.ep_entity);
                m.Body = m.Body.Replace("{REFERENCE}", payment.ep_reference);
                m.Body = m.Body.Replace("{AMOUNT}", payment.ep_value);
            }
            else
            {
                m.Body = m.Body.Replace("{ENTITY}", string.Empty);
                m.Body = m.Body.Replace("{REFERENCE}", string.Empty);
                m.Body = m.Body.Replace("{AMOUNT}", string.Empty);
            }
            m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };
            smtp.Send(m);

            return true;
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Edit(CompanyModelObject model, HttpPostedFileBase imgUpload)
        {
            try
            {
                PaymentDTO payment = null;
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "", string.Format("Company ID[{0}] will be edited", model.ID), string.Format("Company ID[{0}] will be edited.", model.ID));

                CompanyDTO company = null;
                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        company = InsuranceBusiness.BusinessLayer.GetGarage(model.ID);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        company = InsuranceBusiness.BusinessLayer.GetMedicalClinic(model.ID);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        company = InsuranceBusiness.BusinessLayer.GetConstructionCompany(model.ID);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        company = InsuranceBusiness.BusinessLayer.GetHomeApplianceRepair(model.ID);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        company = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContact(model.ID);
                        break;
                }

                company.Active = model.Active;
                company.Name = model.Name;
                company.Description = model.Description;
                company.Address = model.Address;
                company.ContactEmail = model.ContactEmail;
                company.ID_District = model.ID_District;
                company.ID_County = model.ID_County;
                company.ID_Parish = model.ID_Parish;
                company.ID_Service = model.ID_Service;
                company.LogoPhoto = model.LogoPhoto;
                company.MobilePhone_1 = model.MobilePhone_1;
                company.MobilePhone_2 = model.MobilePhone_2;
                company.NIF = model.NIF;
                company.OfficialAgent = model.OfficialAgent;
                company.OfficialPartner = model.OfficialPartner;
                company.Telephone_1 = model.Telephone_1;
                company.Telephone_2 = model.Telephone_2;
                company.Website = model.Website;
                company.BusinessName = model.BusinessName;
                company.IBAN = model.IBAN;

                if (null != imgUpload)
                {
                    company.LogoPhoto = InsuranceSocialNetworkCore.Utils.ConvertionUtils.ScaleImage(InsuranceSocialNetworkCore.Utils.ConvertionUtils.ReadFully(imgUpload.InputStream), 400, 400);
                }

                bool hasPendingPayment = (null == company.Payment || company.Payment.Count == 0) ? false : company.Payment.Exists(i => i.ID_PaymentStatus == (int)PaymentStatusEnum.PENDING);
                
                if (model.CreatePayment && !hasPendingPayment && model.ID_PaymentType > 0)
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, string.Format("Adding payment to company ID[{0}] will be edited", model.ID), string.Format("Adding payment to company ID[{0}] will be edited", model.ID));

                    Guid paymentId = Guid.NewGuid();
                    string baseUrl = string.Format("{0}?ep_cin={1}&ep_user={2}&ep_entity={3}&ep_ref_type={4}&ep_country={5}&ep_language={6}&t_value={7}&t_key={8}"
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_CIN).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_USER).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_ENTITY).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value
                        , model.Value.ToString().Replace(",", ".")
                        , paymentId.ToString());

                    using (var client = new WebClient())
                    {
                        XmlDocument response = new XmlDocument();
                        bool skipProcessing = false;
                        switch (model.ID_PaymentType)
                        {
                            case (int)PaymentTypeEnum.ATM:
                                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "CompaniesManagementController::Edit", baseUrl);
                                var result = client.DownloadString(baseUrl);
                                response.LoadXml(result);
                                break;
                            case (int)PaymentTypeEnum.DIRECT_DEBIT:
                                skipProcessing = true;
                                break;
                            default:
                                skipProcessing = true;
                                break;
                        }
                        if (!skipProcessing)
                        {
                            decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value, System.Globalization.CultureInfo.InvariantCulture);
                            decimal liquidValue = model.Value - decimal.Round(((model.Value / (1 + vatValue)) - model.Value) * -1, 2, MidpointRounding.AwayFromZero);
                            payment = new PaymentDTO()
                            {
                                CreateDate = DateTime.Now,
                                LastChangeDate = DateTime.Now,
                                ID_PaymentType = model.ID_PaymentType,
                                Payment_GUID = paymentId,
                                Active = true,
                                t_value = model.Value.ToString().Replace(",", "."),
                                LiquidValue = liquidValue,
                                TotalValue = model.Value,
                                Title = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_TITLE).Value,
                                Description = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_DESCRIPTION).Value,
                                TaxValue = vatValue,
                                //ExpiracyDate = DateTime.Now.AddDays(Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SUBSCRIPTION_PAYMENT_DEADLINE_DAYS).Value))
                                ExpiracyDate = DateTime.Now.AddYears(model.PaymentPeriod)
                            };
                            
                            var node = response.SelectSingleNode("getautoMB/ep_status");
                            if (node.InnerText.Equals("ok0"))
                            {
                                // Sucesso
                                payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                                payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                                payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                                payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                                payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                                payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                                payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                                payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                                payment.ep_rec_url = "";
                                payment.ep_reference = response.SelectSingleNode("getautoMB/ep_reference").InnerText;
                                payment.ep_value = response.SelectSingleNode("getautoMB/ep_value").InnerText;
                                payment.t_key = response.SelectSingleNode("getautoMB/t_key").InnerText;
                                payment.ep_link = response.SelectSingleNode("getautoMB/ep_link").InnerText;
                                payment.ID_PaymentStatus = (int)PaymentStatusEnum.PENDING;
                            }
                            else
                            {
                                // Erro
                                payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                                payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                                payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                                payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                                payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                                payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                                payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                                payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                                payment.ep_rec_url = "";
                                payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;
                            }
                            
                            switch (model.CompanyType)
                            {
                                case CompanyTypeEnum.GARAGE:
                                    payment.ID_Garage = company.ID;
                                    break;
                                case CompanyTypeEnum.MEDICAL_CLINIC:
                                    payment.ID_MedicalClinic = company.ID;
                                    break;
                                case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                                    payment.ID_ConstructionCompany = company.ID;
                                    break;
                                case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                                    payment.ID_HomeApplianceRepair = company.ID;
                                    break;
                                case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                                    payment.ID_InsuranceCompanyContact = company.ID;
                                    break;
                                default:
                                    break;
                            }

                            if (null == company.Payment)
                            {
                                company.Payment = new List<PaymentDTO>();
                            }
                            company.Payment.Add(payment);
                        }
                    }
                }

                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.EditMedicalClinic(company);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.EditConstructionCompany(company);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.EditHomeApplianceRepair(company);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.EditInsuranceCompanyContact(company);
                        break;
                    default:
                        break;
                }

                if (null != payment && payment.ID_PaymentStatus == (int)PaymentStatusEnum.PENDING)
                {
                    SendPaymentGeneratedEmail(company, payment);
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index", new { id = model.CompanyType });
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Delete(long id, CompanyTypeEnum idType)
        {
            try
            {
                switch (idType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.DeleteGarage(id);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.DeleteMedicalClinic(id);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.DeleteConstructionCompany(id);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.DeleteHomeApplianceRepair(id);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.DeleteInsuranceCompanyContact(id);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return View("Error");
            }

            return RedirectToAction("Index", new { id = idType });
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public PartialViewResult SetupDirectDebit(long id, CompanyTypeEnum idType)
        {
            try
            {
                CompanyDTO company = null;

                switch (idType)
                {
                    case CompanyTypeEnum.GARAGE:
                        company = InsuranceBusiness.BusinessLayer.GetGarage(id);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        company = InsuranceBusiness.BusinessLayer.GetMedicalClinic(id);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        company = InsuranceBusiness.BusinessLayer.GetConstructionCompany(id);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        company = InsuranceBusiness.BusinessLayer.GetHomeApplianceRepair(id);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        company = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContact(id);
                        break;
                }

                decimal subscriptionValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT).Value);
                decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value);

                Guid paymentId = Guid.NewGuid();
                string baseUrl = string.Format("{0}?ep_cin={1}&ep_user={2}&ep_entity={3}&ep_ref_type={4}&ep_country={5}&ep_language={6}&t_value={7}&t_key={8}&ep_rec=yes&ep_rec_freq=1Y&ep_rec_url={9}"
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_CIN).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_USER).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_ENTITY).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value
                    , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value
                    , decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero)
                    , paymentId.ToString()
                    , string.Format("{0}/NotificationsManagement/DirectDebitSetupResult", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.APPLICATION_SITE_URL).Value));

                using (var client = new WebClient())
                {
                    XmlDocument response = new XmlDocument();
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "CompaniesManagementController::SetupDirectDebit", baseUrl);
                    var result = client.DownloadString(baseUrl);
                    response.LoadXml(result);

                    PaymentDTO payment = new PaymentDTO()
                    {
                        CreateDate = DateTime.Now,
                        LastChangeDate = DateTime.Now,
                        ID_PaymentType = InsuranceBusiness.BusinessLayer.GetPaymentTypeID(PaymentTypeEnum.DIRECT_DEBIT),
                        Payment_GUID = paymentId,
                        Active = true,
                        ep_rec="yes",
                        ep_rec_freq="1Y",
                        t_value = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero).ToString(),
                        LiquidValue = subscriptionValue,
                        TotalValue = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero),
                        Title = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_TITLE).Value,
                        Description = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.ANNUAL_SUBSCRIPTION_DESCRIPTION).Value,
                        TaxValue = vatValue,
                        ExpiracyDate = DateTime.Now.AddDays(Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SUBSCRIPTION_PAYMENT_DEADLINE_DAYS).Value))
                    };

                    var node = response.SelectSingleNode("getautoMB/ep_status");
                    if (node.InnerText.Equals("ok0"))
                    {
                        // Sucesso
                        payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                        payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                        payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                        payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                        payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                        payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                        payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                        payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                        payment.ep_reference = response.SelectSingleNode("getautoMB/ep_reference").InnerText;
                        payment.ep_value = response.SelectSingleNode("getautoMB/ep_value").InnerText;
                        payment.t_key = response.SelectSingleNode("getautoMB/t_key").InnerText;
                        payment.ep_link = response.SelectSingleNode("getautoMB/ep_link").InnerText;
                        payment.ep_link_rp_cc = response.SelectSingleNode("getautoMB/ep_link_rp_cc").InnerText;
                        payment.ep_link_rp_dd = response.SelectSingleNode("getautoMB/ep_link_rp_dd").InnerText;
                        payment.ID_PaymentStatus = (int)PaymentStatusEnum.PENDING;

                        if (string.IsNullOrEmpty(payment.t_key))
                        {
                            //http://test.easypay.pt/_s/c11_rp_dd.php?e=10611&r=698305563&v=44.28&c=PT&l=PT&p=1Y&ep_k1=FFAC0B533DB396E23F76AF083D6041DD698305563
                            int startIndex = payment.ep_link_rp_dd.IndexOf("ep_k1=");
                            if (startIndex > 0)
                            {
                                startIndex += 6;

                                int nextField = payment.ep_link_rp_dd.Substring(startIndex).IndexOf("&");
                                if (nextField > 0)
                                {
                                    payment.t_key = payment.ep_link_rp_dd.Substring(startIndex, nextField);
                                }
                                else
                                {
                                    payment.t_key = payment.ep_link_rp_dd.Substring(startIndex);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Erro

                        InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), response.InnerXml);
                        return PartialView("Error");

                        //payment.ep_status = response.SelectSingleNode("getautoMB/ep_status").InnerText;
                        //payment.ep_message = response.SelectSingleNode("getautoMB/ep_message").InnerText;
                        //payment.ep_cin = response.SelectSingleNode("getautoMB/ep_cin").InnerText;
                        //payment.ep_user = response.SelectSingleNode("getautoMB/ep_user").InnerText;
                        //payment.ep_entity = response.SelectSingleNode("getautoMB/ep_entity").InnerText;
                        //payment.ep_ref_type = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_REF_TYPE).Value;
                        //payment.ep_country = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_COUNTRY).Value;
                        //payment.ep_language = InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_LANGUAGE).Value;
                        //payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;
                    }

                    switch (idType)
                    {
                        case CompanyTypeEnum.GARAGE:
                            payment.ID_Garage = company.ID;
                            break;
                        case CompanyTypeEnum.MEDICAL_CLINIC:
                            payment.ID_MedicalClinic = company.ID;
                            break;
                        case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                            payment.ID_ConstructionCompany = company.ID;
                            break;
                        case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                            payment.ID_HomeApplianceRepair = company.ID;
                            break;
                        case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                            payment.ID_InsuranceCompanyContact = company.ID;
                            break;
                        default:
                            break;
                    }

                    if (null == company.Payment)
                    {
                        company.Payment = new List<PaymentDTO>();
                    }
                    else if (company.Payment.Exists(i => i.Active && i.ID_PaymentType == payment.ID_PaymentType && i.ID_PaymentStatus != (int)PaymentStatusEnum.CANCELED))
                    {
                        // disable existing direct debit payments
                        //company.Payment.Where(i => i.Active && i.ID_PaymentType == payment.ID_PaymentType && i.ID_PaymentStatus != (int)PaymentStatusEnum.CANCELED).ToList().ForEach(i => { i.Active = false; i.LastChangeDate = DateTime.Now; });
                    }
                    company.Payment.Add(payment);

                    switch (idType)
                    {
                        case CompanyTypeEnum.GARAGE:
                            InsuranceBusiness.BusinessLayer.EditGarage(company);
                            break;
                        case CompanyTypeEnum.MEDICAL_CLINIC:
                            InsuranceBusiness.BusinessLayer.EditMedicalClinic(company);
                            break;
                        case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                            InsuranceBusiness.BusinessLayer.EditConstructionCompany(company);
                            break;
                        case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                            InsuranceBusiness.BusinessLayer.EditHomeApplianceRepair(company);
                            break;
                        case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                            InsuranceBusiness.BusinessLayer.EditInsuranceCompanyContact(company);
                            break;
                        default:
                            break;
                    }

                    //return Redirect(payment.ep_link);
                    return PartialView("DirectDebitModal", new DirectDebitExternalViewModel() { ExternalURL = payment.ep_link_rp_dd });
                }
            }
            catch(Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return PartialView("Error");
            }
        }

        //[HttpPost]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult CancelDirectDebit(long id)
        {
            try
            {
                //InsuranceBusiness.BusinessLayer.CancelPayment(id);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(string.Format("{0} [{1}]", Request.UserHostAddress, id), string.Format("{0}.{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()), ex);
                return RedirectToAction("Error");
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
    }
}