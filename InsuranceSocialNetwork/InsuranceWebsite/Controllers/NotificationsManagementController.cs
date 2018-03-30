using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.UserProfile;
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
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
            catch (Exception)
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

        [FunctionalityAutorizeAttribute("ALERTS_MANAGEMENT")]
        public JsonResult Get()
        {
            return Json(BusinessItemsLists.GetUsers().ToArray(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public void EasypayPaymentNotification(string ep_cin, string ep_user, string ep_doc, string ep_type)
        {
            try
            {
                PaymentDTO payment = null;

                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "", string.Format("Easypay payment notification received"), string.Format("Easypay infromation received is ep_cin {0}, ep_user {1}, ep_doc {2}, ep_type {3}", ep_cin, ep_user, ep_doc, ep_type));
                
                long notificationId = InsuranceBusiness.BusinessLayer.CreatePaymentNotification(ep_cin, ep_user, ep_doc, ep_type);
                if (notificationId <= 0)
                {
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, "Could not create Payment Notification on easypay notification call", "Could not create Payment Notification on easypay notification call");
                    return;
                }
                PaymentNotificationDTO paymentNotification = InsuranceBusiness.BusinessLayer.GetPaymentNotification(notificationId);

                string baseUrl = string.Format("{0}?ep_cin={1}&ep_user={2}&ep_doc={3}"
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL_FETCH_PAYMENT_DETAIL_URL).Value
                        , ep_cin
                        , ep_user
                        , ep_doc);

                if (!string.IsNullOrEmpty(ep_type))
                {
                    baseUrl += string.Format("&ep_type={0}", ep_type);
                }

                using (var client = new WebClient())
                {
                    XmlDocument response = new XmlDocument();
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "NotificationsManagementController::EasypayPaymentNotification", baseUrl);
                    var result = client.DownloadString(baseUrl);
                    response.LoadXml(result);

                    var node = response.SelectSingleNode("getautoMB_detail/ep_status");
                    if (node.InnerText.Equals("ok0"))
                    {
                        // Sucesso
                        // Update Notification information
                        paymentNotification.ep_date = response.SelectSingleNode("getautoMB_detail/ep_date_read").InnerText;
                        paymentNotification.ep_date_transf = response.SelectSingleNode("getautoMB_detail/ep_date_transf").InnerText;
                        paymentNotification.ep_key = response.SelectSingleNode("getautoMB_detail/ep_key").InnerText;
                        paymentNotification.ep_payment_type = response.SelectSingleNode("getautoMB_detail/ep_payment_type").InnerText;
                        paymentNotification.ep_reference = response.SelectSingleNode("getautoMB_detail/ep_reference").InnerText;
                        paymentNotification.ep_status = response.SelectSingleNode("getautoMB_detail/ep_status_read").InnerText;
                        paymentNotification.ep_value = response.SelectSingleNode("getautoMB_detail/ep_value").InnerText;
                        paymentNotification.ep_value_fixed = response.SelectSingleNode("getautoMB_detail/ep_value_fixed").InnerText;
                        paymentNotification.ep_value_var = response.SelectSingleNode("getautoMB_detail/ep_value_var").InnerText;
                        paymentNotification.ep_value_tax = response.SelectSingleNode("getautoMB_detail/ep_value_tax").InnerText;
                        paymentNotification.ep_value_transf = response.SelectSingleNode("getautoMB_detail/ep_value_transf").InnerText;
                        paymentNotification.t_key = response.SelectSingleNode("getautoMB_detail/t_key").InnerText;
                        paymentNotification.LastChangeDate = DateTime.Now;
                        paymentNotification.NotificationDate = DateTime.Now;

                        payment = InsuranceBusiness.BusinessLayer.GetPayment(paymentNotification.t_key);

                        if (null == payment || payment.ep_cin != ep_cin || payment.ep_user != ep_user)
                        {
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, "Easypay notification infromation doesn't match payment information", string.Format("Easypay infromation received is ep_cin {0}, ep_user {1}, ep_doc {2}, ep_type {3} for payment {4}", ep_cin, ep_user, ep_doc, ep_type, payment.Payment_GUID));
                            return;
                        }

                        payment.LastChangeDate = DateTime.Now;
                        payment.PaymentDate = DateTime.Now;
                        payment.o_obs = response.SelectSingleNode("getautoMB_detail/o_obs").InnerText;
                        payment.o_email = response.SelectSingleNode("getautoMB_detail/o_email").InnerText;
                        payment.o_mobile = response.SelectSingleNode("getautoMB_detail/o_mobile").InnerText;
                        payment.ep_doc = response.SelectSingleNode("getautoMB_detail/ep_doc").InnerText;
                        payment.ep_key = response.SelectSingleNode("getautoMB_detail/ep_key").InnerText;
                        payment.ID_PaymentStatus = (int)PaymentStatusEnum.PAYED;

                        InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                        InsuranceBusiness.BusinessLayer.UpdatePaymentNotification(paymentNotification);
                    }
                    else
                    {
                        // Erro
                        InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, Request.UserHostAddress, "Easypay returned error fetching payment details", result);
                        //payment = InsuranceBusiness.BusinessLayer.GetPayment(paymentNotification.t_key);
                        //payment.LastChangeDate = DateTime.Now;
                        //payment.Message = response.InnerXml;
                        //InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                        return;
                    }
                }

                if (payment.ID_Profile.HasValue)
                {
                    UserProfileDTO profile = InsuranceBusiness.BusinessLayer.GetUserProfile(payment.ID_Profile.Value);
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateUser(payment.ID_Profile.Value);
                    // Send user notification
                    InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, false, payment.ID_Profile.Value, payment.ID);
                    // Get Invoice to send to Client
                    //LibaxUtils.LibaxUtils.CreateInvoice(profile, payment);
                    // Send user email
                    SendPaymentConfirmationEmail(string.Format("{0} {1}", profile.FirstName, profile.LastName), profile.User.UserName);
                }
                else if (payment.ID_Garage.HasValue)
                {
                    CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_Garage.Value);
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateGarage(payment.ID_Garage.Value);
                    // Send user notification
                    //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_Garage.Value, payment.ID, CompanyTypeEnum.GARAGE);
                    // Get Invoice to send to Client
                    LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                    // Send user email
                    SendPaymentConfirmationEmail(company.Name, company.ContactEmail);

                    // Request Direct Debit payment to be executed in one year
                    PaymentDTO directDebitPayment = InsuranceBusiness.BusinessLayer.GetPaymentByUserAndType(payment.ID_Garage.Value, PaymentTypeEnum.DIRECT_DEBIT, CompanyTypeEnum.GARAGE);
                    if (null != directDebitPayment)
                    {
                        baseUrl = string.Format("{0}?e={1}&r={2}&v={3}&ep_k1={4}&rec=yes&ep_key_rec={5}&request_date={6}-{7}-{8}"
                            , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL_REQUEST_PAYMENT_URL).Value
                            , directDebitPayment.ep_entity
                            , directDebitPayment.ep_reference
                            , directDebitPayment.ep_value
                            , directDebitPayment.ep_k1
                            , directDebitPayment.Payment_GUID
                            , DateTime.Now.AddYears(1).Year
                            , DateTime.Now.AddYears(1).Month
                            , DateTime.Now.AddYears(1).Day);

                        using (var client = new WebClient())
                        {
                            XmlDocument response = new XmlDocument();
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, "NotificationsManagementController::EasypayPaymentNotification", baseUrl);
                            var result = client.DownloadString(baseUrl);
                            response.LoadXml(result);

                            var node = response.SelectSingleNode("request_recurring_payment/ep_status");
                            if (node.InnerText.Equals("ok"))
                            {
                                PaymentDTO recurrentDebitDirectPayment = new PaymentDTO()
                                {
                                    ID_Garage = directDebitPayment.ID_Garage,
                                    Active = true,
                                    CreateDate = DateTime.Now,
                                    ExpiracyDate = DateTime.Now.AddYears(1),
                                    ID_PaymentStatus = (int)PaymentStatusEnum.PENDING,
                                    ID_PaymentType = directDebitPayment.ID_PaymentType,
                                    LastChangeDate = DateTime.Now,
                                    Payment_GUID = Guid.NewGuid()
                                };

                                InsuranceBusiness.BusinessLayer.CreatePayment(payment);
                            }
                            else
                            {
                                PaymentDTO recurrentDebitDirectPayment = new PaymentDTO()
                                {
                                    ID_Garage = directDebitPayment.ID_Garage,
                                    Active = true,
                                    CreateDate = DateTime.Now,
                                    ExpiracyDate = DateTime.Now.AddYears(1),
                                    ID_PaymentStatus = (int)PaymentStatusEnum.ERROR,
                                    ID_PaymentType = directDebitPayment.ID_PaymentType,
                                    LastChangeDate = DateTime.Now,
                                    Payment_GUID = Guid.NewGuid(),
                                    Message = node.InnerXml
                                };

                                InsuranceBusiness.BusinessLayer.CreatePayment(payment);
                            }
                        }
                    }
                }
                else if (payment.ID_ConstructionCompany.HasValue)
                {
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateConstructionCompany(payment.ID_ConstructionCompany.Value);
                    // Send user notification
                    //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_ConstructionCompany.Value, payment.ID, CompanyTypeEnum.CONSTRUCTION_COMPANY);
                    // Send user email
                    CompanyDTO company = InsuranceBusiness.BusinessLayer.GetConstructionCompany(payment.ID_ConstructionCompany.Value);
                    SendPaymentConfirmationEmail(company.Name, company.ContactEmail);
                }
                else if (payment.ID_HomeApplianceRepair.HasValue)
                {
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateHomeApplianceRepair(payment.ID_HomeApplianceRepair.Value);
                    // Send user notification
                    //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_HomeApplianceRepair.Value, payment.ID, CompanyTypeEnum.HOME_APPLIANCES_REPAIR);
                    // Send user email
                    CompanyDTO company = InsuranceBusiness.BusinessLayer.GetHomeApplianceRepair(payment.ID_HomeApplianceRepair.Value);
                    SendPaymentConfirmationEmail(company.Name, company.ContactEmail);
                }
                else if (payment.ID_InsuranceCompanyContact.HasValue)
                {
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateInsuranceCompanyContact(payment.ID_InsuranceCompanyContact.Value);
                    // Send user notification
                    //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_InsuranceCompanyContact.Value, payment.ID, CompanyTypeEnum.INSURANCE_COMPANY_CONTACT);
                    // Send user email
                    CompanyDTO company = InsuranceBusiness.BusinessLayer.GetInsuranceCompanyContact(payment.ID_InsuranceCompanyContact.Value);
                    SendPaymentConfirmationEmail(company.Name, company.ContactEmail);
                }
                else if (payment.ID_MedicalClinic.HasValue)
                {
                    // Activate user
                    InsuranceBusiness.BusinessLayer.ActivateMedicalClinic(payment.ID_MedicalClinic.Value);
                    // Send user notification
                    //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_MedicalClinic.Value, payment.ID, CompanyTypeEnum.MEDICAL_CLINIC);
                    // Send user email
                    CompanyDTO company = InsuranceBusiness.BusinessLayer.GetMedicalClinic(payment.ID_MedicalClinic.Value);
                    SendPaymentConfirmationEmail(company.Name, company.ContactEmail);
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, "NotificationsManagementController::EasypayPaymentNotification", ex);
                throw new NotImplementedException();
            }
        }

        [AllowAnonymous]
        public PartialViewResult EasypayDirectDebitNotification(string e, string r, string v, string k, string s, string t_key)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, string.Format("NotificationsManagementController::EasypayDirectDebitNotification"), string.Format("e:{0},r:{1},v:{2},k:{3},s:{4},t_key:{5}", e, r, v, k, s, t_key));

                DirectDebitConfirmViewModel model = new DirectDebitConfirmViewModel();
                PaymentDTO payment = InsuranceBusiness.BusinessLayer.GetPayment(t_key);
                if (s.ToLower().Equals("ok"))
                {
                    payment.ID_PaymentStatus = (int)PaymentStatusEnum.VALID;
                    payment.ep_k1 = k;
                    model.IsSuccess = true;
                    model.Title = Resources.Resources.DirectDebitConfirmationSuccessTitle;
                    model.Message = Resources.Resources.DirectDebitConfirmationSuccessMessage;
                }
                else
                {
                    payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;
                    payment.ep_k1 = k;
                    model.IsSuccess = false;
                    model.Title = Resources.Resources.DirectDebitConfirmationErrorTitle;
                    model.Message = Resources.Resources.DirectDebitConfirmationErrorMessage;
                }

                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);

                return PartialView("ConfirmDirectDebitOperation", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, "NotificationsManagementController::EasypayDirectDebitNotification", ex);
                throw new NotImplementedException();
            }
        }

        [AllowAnonymous]
        public ActionResult EasypayDirectDebitDetails(string e, string r, string v, string c, string l, string t_key)
        {
            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, string.Format("NotificationsManagementController::EasypayDirectDebitDetails"), string.Format("e:{0},r:{1},v:{2},c:{3},l:{4},t_key:{5}", e, r, v, c, l, t_key));
            
            return this.Content(string.Format("<!--?xml version=\"1.0\" encoding=\"ISO-8859-1\"?--><get_detail><ep_status>ok</ep_status><ep_message>success message</ep_message><ep_entity>{0}</ep_entity><ep_reference>{1}</ep_reference><ep_value>{2}</ep_value><t_key>{3}</t_key><order_info><total_taxes>2.50</total_taxes><total_including_taxes>{2}</total_including_taxes><bill_fiscal_number>123456789</bill_fiscal_number><bill_name>John Doe</bill_name><bill_address_1>Morada de teste 1</bill_address_1><bill_address_2>Nº 5 - 1º Dto</bill_address_2><bill_city>Lisboa</bill_city><bill_zip_code>1300-000</bill_zip_code><bill_country>Portugal</bill_country><shipp_fiscal_number>123456789</shipp_fiscal_number><shipp_name>John Doe</shipp_name><shipp_address_1>Morada de teste 1</shipp_address_1><shipp_address_2>Nº 5 - 1º Dto</shipp_address_2><shipp_city>Lisboa</shipp_city><shipp_zip_code>1300-000</shipp_zip_code><shipp_country>Portugal</shipp_country></order_info><order_detail><item><item_description>item 1</item_description><item_quantity>1</item_quantity><item_total>{2}</item_total></item></order_detail></get_detail>", e, r, v, t_key), "text/xml");
        }

        [AllowAnonymous]
        public ActionResult EasypayDirectDebitForward(string e, string r, string v, string k, string s, string t_key)
        {
            try
            {
                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, Request.UserHostAddress, string.Format("NotificationsManagementController::EasypayDirectDebitForward"), string.Format("e:{0},r:{1},v:{2},k:{3},s:{4},t_key:{5}", e, r, v, k, s, t_key));

                DirectDebitConfirmViewModel model = new DirectDebitConfirmViewModel();
                PaymentDTO payment = InsuranceBusiness.BusinessLayer.GetPayment(t_key);
                if (s.ToLower().Equals("ok"))
                {
                    payment.ID_PaymentStatus = (int)PaymentStatusEnum.VALID;
                    payment.ep_k1 = k;
                    model.IsSuccess = true;
                    model.Title = Resources.Resources.DirectDebitConfirmationSuccessTitle;
                    model.Message = Resources.Resources.DirectDebitConfirmationSuccessMessage;
                }
                else
                {
                    payment.ID_PaymentStatus = (int)PaymentStatusEnum.ERROR;
                    payment.ep_k1 = k;
                    model.IsSuccess = false;
                    model.Title = Resources.Resources.DirectDebitConfirmationErrorTitle;
                    model.Message = Resources.Resources.DirectDebitConfirmationErrorMessage;
                }

                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);

                return PartialView("ConfirmDirectDebitOperation", model);
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException(Request.UserHostAddress, "NotificationsManagementController::EasypayDirectDebitNotification", ex);
                throw new NotImplementedException();
            }
        }

        private bool SendPaymentConfirmationEmail(string name, string email)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.PLATFORM_EMAIL).Value, Resources.Resources.ApplicationNAme),
                new System.Net.Mail.MailAddress(email));
            m.Subject = Resources.Resources.EmailPaymentConfirmation;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/NotificationsManagement/EmailTemplates/PaymentConfirmationTemplate.html")))
            {
                m.Body = reader.ReadToEnd();
            }

            m.Body = m.Body.Replace("{NAME}", name); //replacing the required things
            m.Body = m.Body.Replace("{URL}", InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.APPLICATION_SITE_URL).Value);
            m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_HOST).Value, Int32.Parse(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PORT).Value))
            {
                Credentials = new System.Net.NetworkCredential(InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_USERNAME).Value, InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.SMTP_PASSWORD).Value),
                EnableSsl = false
            };
            smtp.Send(m);

            return true;
        }
    }
}