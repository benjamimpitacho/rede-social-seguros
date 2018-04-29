using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkDTO.Payment;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace InsuranceWebsite.Commons
{
    public static class WorkerUtils
    {
        public static void CheckPendingPaymentsToConfirm()
        {
            try
            {
                string baseUrl = string.Format("{0}?ep_cin={1}&ep_user={2}&ep_entity={3}&o_list_type={4}&o_ini={5}"
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_URL_FETCH_ALL_PAYMENT_URL).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_CIN).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_USER).Value
                        , InsuranceBusiness.BusinessLayer.GetSystemSetting(SystemSettingsEnum.EP_ENTITY).Value
                        , "last"
                        , 100);

                using (var client = new WebClient())
                {
                    XmlDocument response = new XmlDocument();
                    InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.INFO, "WORKER", "TIMER JOB TASK - CheckPendingPaymentsToConfirm", string.Format("Base URL: {0}", baseUrl));
                    var result = client.DownloadString(baseUrl);
                    response.LoadXml(result);

                    var node = response.SelectSingleNode("get_ref/ep_status");
                    if (node.InnerText.Equals("ok0"))
                    {
                        // Sucesso
                        int totalPayments = 0;
                        if (!Int32.TryParse(response.SelectSingleNode("get_ref/ep_num_records").InnerText, out totalPayments))
                        {
                            InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, "WORKER", "TIMER JOB TASK - CheckPendingPaymentsToConfirm", string.Format("Cannot count [int32 parse error] received payments. {0}", response.SelectSingleNode("get_ref/ep_num_records").InnerText));
                            return;
                        }

                        var paymentsList = response.SelectSingleNode("get_ref/ref_detail").SelectNodes("ref");
                        if (null == paymentsList || paymentsList.Count == 0)
                        {
                            return;
                        }

                        foreach (XmlNode paymentInfo in paymentsList)
                        {
                            PaymentDTO payment = InsuranceBusiness.BusinessLayer.GetPayment(paymentInfo.SelectSingleNode("t_key").InnerText);
                            if (null == payment)
                            {
                                InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, "WORKER", "TIMER JOB TASK - CheckPendingPaymentsToConfirm", string.Format("Cannot find payment on database. {0}", paymentInfo.InnerXml));
                                continue;
                            }

                            if (payment.ID_PaymentStatus == (int)PaymentStatusEnum.PAYED && payment.NotificationSent)
                            {
                                continue;
                            }

                            long notificationId = InsuranceBusiness.BusinessLayer.CreatePaymentNotification(paymentInfo.SelectSingleNode("ep_cin").InnerText, paymentInfo.SelectSingleNode("ep_user").InnerText, paymentInfo.SelectSingleNode("ep_doc").InnerText, paymentInfo.SelectSingleNode("ep_payment_type").InnerText);
                            PaymentNotificationDTO paymentNotification = InsuranceBusiness.BusinessLayer.GetPaymentNotification(notificationId);

                            paymentNotification.ep_date = paymentInfo.SelectSingleNode("ep_date_read").InnerText;
                            paymentNotification.ep_date_transf = paymentInfo.SelectSingleNode("ep_date_transf").InnerText;
                            paymentNotification.ep_key = paymentInfo.SelectSingleNode("ep_key").InnerText;
                            paymentNotification.ep_payment_type = paymentInfo.SelectSingleNode("ep_payment_type").InnerText;
                            paymentNotification.ep_reference = paymentInfo.SelectSingleNode("ep_reference").InnerText;
                            paymentNotification.ep_status = paymentInfo.SelectSingleNode("ep_status_read").InnerText;
                            paymentNotification.ep_value = paymentInfo.SelectSingleNode("ep_value").InnerText;
                            paymentNotification.ep_value_fixed = paymentInfo.SelectSingleNode("ep_value_fixed").InnerText;
                            paymentNotification.ep_value_var = paymentInfo.SelectSingleNode("ep_value_var").InnerText;
                            paymentNotification.ep_value_tax = paymentInfo.SelectSingleNode("ep_value_tax").InnerText;
                            paymentNotification.ep_value_transf = paymentInfo.SelectSingleNode("ep_value_transf").InnerText;
                            paymentNotification.t_key = paymentInfo.SelectSingleNode("t_key").InnerText;
                            paymentNotification.LastChangeDate = DateTime.Now;
                            paymentNotification.NotificationDate = DateTime.Now;

                            payment.LastChangeDate = DateTime.Now;
                            payment.PaymentDate = DateTime.Now;
                            //payment.o_obs = paymentInfo.SelectSingleNode("o_obs").InnerText;
                            //payment.o_email = paymentInfo.SelectSingleNode("o_email").InnerText;
                            //payment.o_mobile = paymentInfo.SelectSingleNode("o_mobile").InnerText;
                            payment.ep_doc = paymentInfo.SelectSingleNode("ep_doc").InnerText;
                            payment.ep_key = paymentInfo.SelectSingleNode("ep_key").InnerText;
                            payment.ID_PaymentStatus = (int)PaymentStatusEnum.PAYED;

                            InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                            InsuranceBusiness.BusinessLayer.UpdatePaymentNotification(paymentNotification);

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
                                new NotificationsManagementController().SendPaymentConfirmationEmail(string.Format("{0} {1}", profile.FirstName, profile.LastName), profile.User.UserName, null);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                            }
                            else if (payment.ID_Garage.HasValue)
                            {
                                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_Garage.Value);
                                // Activate user
                                InsuranceBusiness.BusinessLayer.ActivateGarage(payment.ID_Garage.Value);
                                // Send user notification
                                //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_Garage.Value, payment.ID, CompanyTypeEnum.GARAGE);
                                // Get Invoice to send to Client
                                byte[] invoiceDocument = LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                                // Send user email
                                new NotificationsManagementController().SendPaymentConfirmationEmail(company.Name, company.ContactEmail, invoiceDocument, payment.ID);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                                // Request Direct Debit payment - schedule for one year later since today
                                new NotificationsManagementController().RequestDirectDebitOrder(payment, baseUrl);
                            }
                            else if (payment.ID_ConstructionCompany.HasValue)
                            {
                                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_ConstructionCompany.Value);
                                // Activate user
                                InsuranceBusiness.BusinessLayer.ActivateConstructionCompany(payment.ID_ConstructionCompany.Value);
                                // Send user notification
                                //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_ConstructionCompany.Value, payment.ID, CompanyTypeEnum.CONSTRUCTION_COMPANY);
                                // Get Invoice to send to Client
                                byte[] invoiceDocument = LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                                // Send user email
                                new NotificationsManagementController().SendPaymentConfirmationEmail(company.Name, company.ContactEmail, invoiceDocument, payment.ID);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                                // Request Direct Debit payment - schedule for one year later since today
                                new NotificationsManagementController().RequestDirectDebitOrder(payment, baseUrl);
                            }
                            else if (payment.ID_HomeApplianceRepair.HasValue)
                            {
                                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_HomeApplianceRepair.Value);
                                // Activate user
                                InsuranceBusiness.BusinessLayer.ActivateHomeApplianceRepair(payment.ID_HomeApplianceRepair.Value);
                                // Send user notification
                                //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_HomeApplianceRepair.Value, payment.ID, CompanyTypeEnum.HOME_APPLIANCES_REPAIR);
                                // Get Invoice to send to Client
                                byte[] invoiceDocument = LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                                // Send user email
                                new NotificationsManagementController().SendPaymentConfirmationEmail(company.Name, company.ContactEmail, invoiceDocument, payment.ID);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                                // Request Direct Debit payment - schedule for one year later since today
                                new NotificationsManagementController().RequestDirectDebitOrder(payment, baseUrl);
                            }
                            else if (payment.ID_InsuranceCompanyContact.HasValue)
                            {
                                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_InsuranceCompanyContact.Value);
                                // Activate user
                                InsuranceBusiness.BusinessLayer.ActivateInsuranceCompanyContact(payment.ID_InsuranceCompanyContact.Value);
                                // Send user notification
                                //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_InsuranceCompanyContact.Value, payment.ID, CompanyTypeEnum.INSURANCE_COMPANY_CONTACT);
                                // Get Invoice to send to Client
                                byte[] invoiceDocument = LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                                // Send user email
                                new NotificationsManagementController().SendPaymentConfirmationEmail(company.Name, company.ContactEmail, invoiceDocument, payment.ID);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                                // Request Direct Debit payment - schedule for one year later since today
                                new NotificationsManagementController().RequestDirectDebitOrder(payment, baseUrl);
                            }
                            else if (payment.ID_MedicalClinic.HasValue)
                            {
                                CompanyDTO company = InsuranceBusiness.BusinessLayer.GetGarage(payment.ID_MedicalClinic.Value);
                                // Activate user
                                InsuranceBusiness.BusinessLayer.ActivateMedicalClinic(payment.ID_MedicalClinic.Value);
                                // Send user notification
                                //InsuranceBusiness.BusinessLayer.CreateNotificationForPaymentDone(NotificationTypeEnum.PAYMENT_CONFIRMED, true, payment.ID_MedicalClinic.Value, payment.ID, CompanyTypeEnum.MEDICAL_CLINIC);
                                // Get Invoice to send to Client
                                byte[] invoiceDocument = LibaxUtils.LibaxUtils.CreateInvoice(company, payment);
                                // Send user email
                                new NotificationsManagementController().SendPaymentConfirmationEmail(company.Name, company.ContactEmail, invoiceDocument, payment.ID);
                                payment.NotificationSent = true;
                                InsuranceBusiness.BusinessLayer.UpdatePayment(payment);
                                // Request Direct Debit payment - schedule for one year later since today
                                new NotificationsManagementController().RequestDirectDebitOrder(payment, baseUrl);
                            }
                        }
                    }
                    else
                    {
                        // Erro
                        InsuranceBusiness.BusinessLayer.Log(SystemLogLevelEnum.ERROR, "WORKER", "TIMER JOB TASK - CheckPendingPaymentsToConfirm", response.SelectSingleNode("get_ref/ep_message").InnerText);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                InsuranceBusiness.BusinessLayer.LogException("WORKER", "TIMER JOB TASK - CheckPendingPaymentsToConfirm", ex);
            }
        }

        public static void CheckUsersToDisableDueToPaymentFail()
        {

        }

        public static void CheckUsersForRenewal()
        {

        }
    }
}