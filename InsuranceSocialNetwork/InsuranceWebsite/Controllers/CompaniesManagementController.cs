using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.PostalCode;
using InsuranceWebsite.Commons;
using InsuranceWebsite.Models;
using InsuranceWebsite.Utils;
using Microsoft.AspNet.Identity.Owin;
using MVCGrid.Models;
using MVCGrid.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
                            query = BusinessItemsLists.GetMedicalClinis();
                            break;
                        case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                            query = BusinessItemsLists.GetConstructionCompanies();
                            break;
                        case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                            query = BusinessItemsLists.GetHomeApplianceRepairs();
                            break;
                        case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                            query = BusinessItemsLists.GetInsuranceCompanyContacts();
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
            CompanyModelObject model = new CompanyModelObject();

            decimal subscriptionValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT).Value);
            decimal vatValue = decimal.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.VAT_PERCENTAGE).Value);
            model.ID_PaymentType = InsuranceBusiness.BusinessLayer.GetPaymentTypeID(PaymentTypeEnum.ATM);
            model.Value = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero);

            List<SelectListItem>  initList = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = Resources.Resources.SelectService } };
            model.ServiceList = initList.Concat(InsuranceBusiness.BusinessLayer.GetCompanyServices(id).Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = i.Value }).ToList()).ToList();
            model.PaymentTypeList = initList.Concat(InsuranceBusiness.BusinessLayer.GetPaymentTypes().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList()).ToList();
            model.CompanyType = id;
            model.CreatePayment = true;

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
                    IBAN = model.IBAN
                };

                if (null != fileUploaderControl)
                {
                    MemoryStream target = new MemoryStream();
                    fileUploaderControl.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    newCompany.LogoPhoto = data;
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
                            PaymentDTO payment = new PaymentDTO()
                            {
                                CreateDate = DateTime.Now,
                                LastChangeDate = DateTime.Now,
                                ID_PaymentType = model.ID_PaymentType,
                                Payment_GUID = paymentId,
                                Active = true,
                                t_value = model.Value.ToString().Replace(",", "."),
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

                            if (null == newCompany.Payment)
                            {
                                newCompany.Payment = new List<PaymentDTO>();
                            }
                            newCompany.Payment.Add(payment);
                        }
                    }
                }

                switch (model.CompanyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        InsuranceBusiness.BusinessLayer.CreateGarage(newCompany);
                        break;
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        InsuranceBusiness.BusinessLayer.CreateMedicalClinic(newCompany);
                        break;
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        InsuranceBusiness.BusinessLayer.CreateConstructionCompany(newCompany);
                        break;
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        InsuranceBusiness.BusinessLayer.CreateHomeApplianceRepair(newCompany);
                        break;
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        InsuranceBusiness.BusinessLayer.CreateInsuranceCompanyContact(newCompany);
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
            
            if (model.ID_District.HasValue)
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
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "", string.Format("Company ID[{0}] will be edited", model.ID), string.Format("Company ID[{0}] will be edited.", model.ID));

                CompanyDTO company = null;
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#0", "Ponto#0");
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
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#0.1", "Ponto#0.1");
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

                bool hasPendingPayment = (null == company.Payment || company.Payment.Count == 0) ? false : company.Payment.Exists(i => i.ID_PaymentStatus == (int)PaymentStatusEnum.PENDING);
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#0.2", "Ponto#0.2");

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
                        InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#1", "Ponto#1");
                        if (!skipProcessing)
                        {
                            PaymentDTO payment = new PaymentDTO()
                            {
                                CreateDate = DateTime.Now,
                                LastChangeDate = DateTime.Now,
                                ID_PaymentType = model.ID_PaymentType,
                                Payment_GUID = paymentId,
                                Active = true,
                                t_value = model.Value.ToString().Replace(",", "."),
                                ExpiracyDate = DateTime.Now.AddDays(Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SUBSCRIPTION_PAYMENT_DEADLINE_DAYS).Value))
                            };
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#2", "Ponto#2");
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
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#3", "Ponto#3");
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
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "Ponto#4", "Ponto#4");
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
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, "Edit Company", ex);
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            return RedirectToAction("Index");
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
                    , "http://www.google.com");

                using (var client = new WebClient())
                {
                    XmlDocument response = new XmlDocument();
                    var result = client.DownloadString(baseUrl);
                    response.LoadXml(result);

                    PaymentDTO payment = new PaymentDTO()
                    {
                        CreateDate = DateTime.Now,
                        LastChangeDate = DateTime.Now,
                        ID_PaymentType = InsuranceBusiness.BusinessLayer.GetPaymentTypeID(PaymentTypeEnum.DIRECT_DEBIT),
                        Payment_GUID = paymentId,
                        Active = true,
                        t_value = decimal.Round((vatValue * subscriptionValue) + subscriptionValue, 2, MidpointRounding.AwayFromZero).ToString(),
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
                        payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;
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
                    return PartialView("DirectDebitModal", new DirectDebitExternalViewModel() { ExternalURL = payment.ep_link });
                }
            }
            catch(Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, "Setup Direct Debit", ex);
                throw new NotImplementedException();
            }
        }

        //[FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        //public ActionResult Activate(long id)
        //{
        //    try
        //    {
        //        InsuranceBusiness.BusinessLayer.ActivateGarage(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return RedirectToAction("Index");
        //}

        //[FunctionalityAutorizeAttribute("COMPANIES_MANAGEMENT")]
        //public ActionResult Deactivate(long id)
        //{
        //    try
        //    {
        //        InsuranceBusiness.BusinessLayer.ActivateGarage(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}