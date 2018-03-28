using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;

namespace InsuranceSocialNetworkDAL
{
    public class PaymentRepository
    {
        public static List<ListItem> GetPaymentTypes()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PaymentType
                    .Fetch()
                    .Where(i => i.Active)
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Token })
                    .OrderBy(i => i.Value)
                    .ToList();
            }
        }

        public static PaymentType GetPaymentType(string typeToken)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PaymentType
                    .Fetch()
                    .Where(i => i.Active && i.Token == typeToken)
                    .FirstOrDefault();
            }
        }

        public static PaymentStatus GetPaymentStatus(string statusToken)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PaymentStatus
                    .Fetch()
                    .Where(i => i.Active && i.Token == statusToken)
                    .FirstOrDefault();
            }
        }

        public static Payment GetPayment(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Payment
                    .Fetch()
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.ID == id)
                    .FirstOrDefault();
            }
        }

        public static Payment GetPayment(string paymentId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Guid id = new Guid(paymentId);
                return context.Payment
                    .Fetch()
                    .Include(i => i.PaymentType)
                    .Include(i => i.PaymentStatus)
                    .Where(i => i.Payment_GUID == id)
                    .FirstOrDefault();
            }
        }

        public static Payment GetPayment(long id, string paymentType, CompanyTypeEnum companyType)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                switch(companyType)
                {
                    case CompanyTypeEnum.GARAGE:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_Garage == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                    case CompanyTypeEnum.CONSTRUCTION_COMPANY:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_ConstructionCompany == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                    case CompanyTypeEnum.HOME_APPLIANCES_REPAIR:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_HomeApplianceRepair == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                    case CompanyTypeEnum.INSURANCE_COMPANY_CONTACT:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_InsuranceCompanyContact == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                    case CompanyTypeEnum.MEDICAL_CLINIC:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_MedicalClinic == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                    default:
                        return context.Payment
                            .Fetch()
                            .Include(i => i.PaymentType)
                            .Include(i => i.PaymentStatus)
                            .Where(i => i.ID_Profile == id && i.PaymentType.Token == paymentType && i.Active)
                            .FirstOrDefault();
                }
            }
        }

        public static long CreatePayment(Payment payment)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                payment.CreateDate = DateTime.Now;
                payment.LastChangeDate = DateTime.Now;

                context.Payment.Create(payment);
                context.Save();

                return payment.ID;
            }
        }

        public static bool EditPayment(Payment payment)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                payment.LastChangeDate = DateTime.Now;

                context.Payment.Update(payment);
                context.Save();

                return true;
            }
        }

        public static bool DeletePayment(long paymentId, string userId)
        {
            throw new NotImplementedException();
        }

        public static PaymentNotification GetPaymentNotification(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PaymentNotification
                    .Fetch()
                    .Where(i => i.ID == id)
                    .FirstOrDefault();
            }
        }

        public static long CreatePaymentNotification(PaymentNotification paymentNotification)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                context.PaymentNotification.Create(paymentNotification);
                context.Save();
                return paymentNotification.ID;
            }
        }

        public static bool EditPaymentNotification(PaymentNotification paymentNotification)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                paymentNotification.LastChangeDate = DateTime.Now;

                context.PaymentNotification.Update(paymentNotification);
                context.Save();

                return true;
            }
        }
    }
}
