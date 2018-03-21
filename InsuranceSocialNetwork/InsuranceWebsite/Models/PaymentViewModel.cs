using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
        }

        [Required]
        public long ID_PaymentType { get; set; }
        public string IBAN { get; set; }
        [Required]
        public decimal Value { get; set; }

    }

    public class DirectDebitExternalViewModel
    {
        public DirectDebitExternalViewModel()
        {
        }

        [Url]
        [Required]
        public string ExternalURL { get; set; }

    }

    public class DirectDebitConfirmViewModel
    {
        public DirectDebitConfirmViewModel()
        {
        }

        public bool IsSuccess { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

    }
}
