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

        public static long CreatePayment(Payment payment)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                throw new NotImplementedException();
            }
        }

        public static bool UpdatePayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        public static bool DeletePayment(long paymentId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
