﻿using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.PostalCode;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using InsuranceWebsite.Utils;
using Microsoft.AspNet.Identity.Owin;
using MVCGrid.Models;
using MVCGrid.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var grid = MVCGridDefinitionTable.GetDefinition<CompanyModelObject>("CompaniesManagementGrid");
            }
            catch (Exception)
            {
                MVCGridDefinitionTable.Add("CompaniesManagementGrid", new MVCGridBuilder<CompanyModelObject>()
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
                        .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit'>" + Resources.Resources.Edit + "</a>")
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
                    
                    switch(model.CompanyType)
                    {
                        case CompanyTypeEnum.GARAGE:
                            query = BusinessItemsLists.GetGarages();
                            break;
                        case CompanyTypeEnum.MEDICAL_CLINIC:
                            query = BusinessItemsLists.GetGarages();
                            break;
                        case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                            query = BusinessItemsLists.GetGarages();
                            break;
                        case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                            query = BusinessItemsLists.GetGarages();
                            break;
                        case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                            query = BusinessItemsLists.GetGarages();
                            break;
                    }
                    
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
                            (null != i.Name && i.Name.Contains(globalSearch))
                            || (null != i.NIF && i.NIF.Contains(globalSearch))
                            || (null != i.ContactEmail && i.ContactEmail.Contains(globalSearch))
                            //|| (null != i.ContactEmail && i.ContactEmail.Contains(globalSearch))
                        ).ToList();
                    }

                    return new QueryResult<CompanyModelObject>()
                    {
                        Items = query,
                        TotalRecords = query.Count // if paging is enabled, return the total number of records of all pages
                    };
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
            CompanyModelObject model = new CompanyModelObject()
            {
                CompanyType = id
            };

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Create(CompanyModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
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
                    MobilePhone_1 = model.MobilePhone_1,
                    MobilePhone_2 = model.MobilePhone_2,
                    Name = model.Name,
                    NIF = model.NIF,
                    OfficialAgent = model.OfficialAgent,
                    OfficialPartner = model.OfficialPartner,
                    Telephone_1 = model.Telephone_1,
                    Telephone_2 = model.Telephone_2,
                    Website = model.Website
                };

                if (null != fileUploaderControl)
                {
                    MemoryStream target = new MemoryStream();
                    fileUploaderControl.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    newCompany.LogoPhoto = data;
                }

                switch(model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index", new { id = model.CompanyType });
        }

        // GET: /User/Edit/5
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Edit(long id, CompanyTypeEnum idType)
        {
            CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(id);

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
                LogoPhoto = company.LogoPhoto,
                MobilePhone_1 = company.MobilePhone_1,
                MobilePhone_2 = company.MobilePhone_2,
                NIF = company.NIF,
                OfficialAgent = company.OfficialAgent,
                OfficialPartner = company.OfficialPartner,
                Telephone_1 = company.Telephone_1,
                Telephone_2 = company.Telephone_2,
                Website = company.Website
            };

            if(model.ID_District.HasValue)
            {
                model.CountyList = model.CountyList.Concat(InsuranceBusiness.BusinessLayer.GetCountiesByDistrict(model.ID_District.Value).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            }

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Edit(CompanyModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(model.ID);

                company.Active = model.Active;
                company.Name = model.Name;
                company.Description = model.Description;
                company.Address = model.Address;
                company.ContactEmail = model.ContactEmail;
                company.ID_District = model.ID_District;
                company.ID_County = model.ID_County;
                company.ID_Parish = model.ID_Parish;
                company.LogoPhoto = model.LogoPhoto;
                company.MobilePhone_1 = model.MobilePhone_1;
                company.MobilePhone_2 = model.MobilePhone_2;
                company.NIF = model.NIF;
                company.OfficialAgent = model.OfficialAgent;
                company.OfficialPartner = model.OfficialPartner;
                company.Telephone_1 = model.Telephone_1;
                company.Telephone_2 = model.Telephone_2;
                company.Website = model.Website;

                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.EditGarage(company);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index", new { id = model.CompanyType });
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Delete(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeleteGarage(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Activate(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.ActivateGarage(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Deactivate(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.ActivateGarage(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetPostalCodeInformation(string postalCode)
        {
            try
            {
                PostalCodeDTO postalCodeInfo = InsuranceBusiness.BusinessLayer.GetPostalCodeInformation(postalCode);

                if (null == postalCodeInfo)
                {
                    return Json(new { ok = false }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { ok = true, localocality = postalCodeInfo.Localidade, postalCodeId = postalCodeInfo.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}