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
}
