using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Garage;
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
            cfg.CreateMap<GarageDTO, CompanyModelObject>();
            cfg.CreateMap<CompanyModelObject, GarageDTO>();
        });

        public CompaniesManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public async Task<ActionResult> Index()
        {
            CompaniesManagementViewModel model = new CompaniesManagementViewModel();

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
                var grid = MVCGridDefinitionTable.GetDefinition<UserProfileModelObject>("CompaniesManagementGrid");
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
                        .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "CompaniesManagement", new { id = p.ID }))
                        .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit'>" + Resources.Resources.Edit + "</a>")
                        .WithVisibility(true, false);
                    cols.Add("Delete").WithHtmlEncoding(false)
                        .WithSorting(false)
                        .WithHeaderText(" ")
                        .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "CompaniesManagement", new { id = p.ID }))
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
                    var query = BusinessItemsLists.GetGarages();
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
        public ActionResult Create()
        {
            CompanyModelObject model = new CompanyModelObject()
            {
                //StartDate = DateTime.Now
            };

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public async Task<ActionResult> Create(CompanyModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                GarageDTO newGarage = Mapper.Map<GarageDTO>(model);

                if (null != fileUploaderControl)
                {
                    MemoryStream target = new MemoryStream();
                    fileUploaderControl.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    newGarage.LogoPhoto = data;
                }

                InsuranceBusiness.BusinessLayer.CreateGarage(newGarage);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        // GET: /User/Edit/5
        [FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        public ActionResult Edit(long id = 0)
        {
            GarageDTO garage = InsuranceBusiness.BusinessLayer.GetGarage(id);

            CompanyModelObject model = new CompanyModelObject()
            {
                ID = garage.ID,
                Active = garage.Active,
                Name = garage.Name,
                Description = garage.Description,
                Address = garage.Address,
                ContactEmail = garage.ContactEmail,
                ID_PostalCode = garage.ID_PostalCode,
                LogoPhoto = garage.LogoPhoto,
                MobilePhone_1 = garage.MobilePhone_1,
                MobilePhone_2 = garage.MobilePhone_2,
                NIF = garage.NIF,
                OficialAgent = garage.OficialAgent,
                OficialPartner = garage.OficialPartner,
                Telephone_1 = garage.Telephone_1,
                Telephone_2 = garage.Telephone_2,
                Website = garage.Website
            };

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
                GarageDTO garage = InsuranceBusiness.BusinessLayer.GetGarage(model.ID);

                //garage.ID = model.ID;
                garage.Active = model.Active;
                garage.Name = model.Name;
                garage.Description = model.Description;
                garage.Address = model.Address;
                garage.ContactEmail = model.ContactEmail;
                garage.ID_PostalCode = model.ID_PostalCode;
                garage.LogoPhoto = model.LogoPhoto;
                garage.MobilePhone_1 = model.MobilePhone_1;
                garage.MobilePhone_2 = model.MobilePhone_2;
                garage.NIF = model.NIF;
                garage.OficialAgent = model.OficialAgent;
                garage.OficialPartner = model.OficialPartner;
                garage.Telephone_1 = model.Telephone_1;
                garage.Telephone_2 = model.Telephone_2;
                garage.Website = model.Website;

                InsuranceBusiness.BusinessLayer.EditGarage(garage);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
    }
}