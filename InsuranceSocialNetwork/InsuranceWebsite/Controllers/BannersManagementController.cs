﻿using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Role;
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

namespace InsuranceWebsite.Controllers
{
    [Authorize]
    public class BannersManagementController : Controller
    {
        //MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<RoleDTO, RoleModelObject>();
        //    cfg.CreateMap<RoleModelObject, RoleDTO>();
        //});

        public BannersManagementController()
        {
        }

        [FunctionalityAutorizeAttribute("BANNERS_MANAGEMENT")]
        public async Task<ActionResult> Index()
        {
            BannersManagementViewModel model = new BannersManagementViewModel();

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
                var grid = MVCGridDefinitionTable.GetDefinition<BannerModelObject>("BannersManagementGrid");
            }
            catch (Exception)
            {
                //InsuranceBusiness.BusinessLayer.GetRoles

                MVCGridDefinitionTable.Add("BannersManagementGrid", new MVCGridBuilder<BannerModelObject>()
                    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                    .AddColumns(cols =>
                    {
                        // Add your columns here
                        cols.Add().WithColumnName("Description")
                                .WithHeaderText(Resources.Resources.Description)
                                .WithValueExpression(i => i.Description)
                                .WithAllowChangeVisibility(true)
                                .WithFiltering(true)
                                .WithSorting(true)
                                .WithVisibility(true, true); // use the Value Expression to return the cell text for this column
                        cols.Add("Type")
                                .WithHeaderText(Resources.Resources.Type)
                                .WithValueExpression(i => i.ID_Banner_Type.ToString());
                        cols.Add("StartDate")
                                .WithHeaderText(Resources.Resources.StartDate)
                                .WithCellCssClassExpression(p => "dateCell")
                                .WithValueExpression(p => p.StartDate.ToString("dd-MM-yyyy"));
                        cols.Add("DueDate")
                                .WithHeaderText(Resources.Resources.DueDate)
                                .WithCellCssClassExpression(p => "dateCell")
                                .WithValueExpression(p => p.DueDate.HasValue ? p.DueDate.Value.ToString("dd-MM-yyyy") : "");
                        cols.Add("Edit").WithHtmlEncoding(false)
                                .WithSorting(false)
                                .WithHeaderText(" ")
                                .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "BannersManagement", new { id = p.ID }))
                                .WithValueTemplate("<a href='{Value}' class='btn btn-warning lnkEdit'>" + Resources.Resources.Edit + "</a>");
                        cols.Add("Delete").WithHtmlEncoding(false)
                                .WithSorting(false)
                                .WithHeaderText(" ")
                                .WithValueExpression((p, c) => c.UrlHelper.Action("Delete", "BannersManagement", new { id = p.ID }))
                                .WithValueTemplate("<a href='{Value}' class='btn btn-danger lnkDelete' role='button'>" + Resources.Resources.Delete + "</a>");
                    })
                    .WithAdditionalQueryOptionNames("Search")
                    .WithSorting(true, "CreateDate")
                    .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
                    .WithRetrieveDataMethod((context) =>
                    {
                        // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                        // Use Entity Framework, a module from your IoC Container, or any other method.
                        // Return QueryResult object containing IEnumerable<YouModelItem>

                        var options = context.QueryOptions;
                        var result = new QueryResult<BannerModelObject>();
                        var query = BusinessItemsLists.GetBanners();
                        if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            switch (options.SortColumnName.ToLower())
                            {
                                case "firstname":
                                    //if (options.SortDirection == SortDirection.Asc
                                    //    || options.SortDirection == SortDirection.Unspecified)
                                    //{
                                    //    query = query.OrderBy(p => p.FirstName).ToList();
                                    //}
                                    //else
                                    //{
                                    //    query = query.OrderByDescending(p => p.FirstName).ToList();
                                    //}

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
                                i.Description.Contains(globalSearch)
                            //|| i.LastName.Contains(globalSearch)
                            //|| i.User.UserName.Contains(globalSearch)
                            //|| i.ContactEmail.Contains(globalSearch)
                            ).ToList();
                        }

                        return new QueryResult<BannerModelObject>()
                        {
                            Items = query,
                            TotalRecords = query.Count // if paging is enabled, return the total number of records of all pages
                        };
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

        [FunctionalityAutorizeAttribute("BANNERS_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetBanners().ToArray(), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[FunctionalityAutorizeAttribute("BANNERS_MANAGEMENT")]
        //public ActionResult CreateBanner(BannerModelObject model, HttpPostedFileBase fileUploaderControl)
        //{
        //    try
        //    {
        //        MemoryStream target = new MemoryStream();
        //        fileUploaderControl.InputStream.CopyTo(target);
        //        byte[] data = target.ToArray();

        //        BannerDTO newBanner = new BannerDTO()
        //        {
        //            ID_Banner_Type = model.ID_Banner_Type,
        //            Description = model.Description,
        //            StartDate = model.StartDate,
        //            DueDate = model.DueDate,
        //            Image = data
        //        };
        //        InsuranceBusiness.BusinessLayer.CreateBanner(newBanner);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return RedirectToAction("Index");
        //}

        //public ActionResult AddBanner(BannerModelObject model)
        //{
        //    try
        //    {
        //        model = new BannerModelObject();
        //        model.StartDate = DateTime.Now;

        //        return View(model);

        //        //InsuranceBusiness.BusinessLayer.CreatePost(newPost);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        // GET: /User/Edit/5
        public ActionResult Create()
        {
            BannerModelObject model = new BannerModelObject()
            {
                StartDate = DateTime.Now
            };

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BannerModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                BannerDTO newBanner = new BannerDTO()
                {
                    ID_Banner_Type = model.ID_Banner_Type,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    DueDate = model.DueDate
                };

                if (null != fileUploaderControl)
                {
                    MemoryStream target = new MemoryStream();
                    fileUploaderControl.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    newBanner.Image = data;
                }

                InsuranceBusiness.BusinessLayer.CreateBanner(newBanner);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        // GET: /User/Edit/5
        public ActionResult Edit(long id = 0)
        {
            BannerDTO banner = InsuranceBusiness.BusinessLayer.GetBanner(id);

            BannerModelObject model = new BannerModelObject()
            {
                Active = banner.Active,
                Description = banner.Description,
                DueDate = banner.DueDate,
                ID = banner.ID,
                ID_Banner_Type = banner.ID_Banner_Type,
                Image = banner.Image,
                StartDate = banner.StartDate
            };

            return PartialView(model);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BannerModelObject model, HttpPostedFileBase fileUploaderControl)
        {
            try
            {
                BannerDTO newBanner = new BannerDTO()
                {
                    ID_Banner_Type = model.ID_Banner_Type,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    DueDate = model.DueDate
                };

                if (null != fileUploaderControl)
                {
                    MemoryStream target = new MemoryStream();
                    fileUploaderControl.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    newBanner.Image = data;
                }

                InsuranceBusiness.BusinessLayer.EditBanner(newBanner);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(long id)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.DeleteBanner(id);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
        }
    }
}