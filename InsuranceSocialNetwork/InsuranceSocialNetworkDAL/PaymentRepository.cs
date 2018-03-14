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

        public static long CreatePayment(Post post)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                throw new NotImplementedException();
            }
        }

        public static long EditPayment(Post post)
        {
            throw new NotImplementedException();
        }

        public static bool DeletePayment(long postId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
