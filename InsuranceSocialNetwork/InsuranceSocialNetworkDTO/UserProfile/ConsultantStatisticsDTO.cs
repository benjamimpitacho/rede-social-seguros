using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.PostalCode;
using InsuranceSocialNetworkDTO.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.UserProfile
{
    public class ConsultantStatisticsDTO
    {
        // Consultant Information
        public string Consultant_ID_User { get; set; }
        public string Consultant_Username { get; set; }
        public string Consultant_FirstName { get; set; }
        public string Consultant_LastName { get; set; }

        // Company Information
        public long Company_ID { get; set; }
        public string Company_ID_User { get; set; }
        public string Company_Name { get; set; }
        public string Company_BusinessName { get; set; }
        public string Company_Description { get; set; }
        public string Company_NIF { get; set; }
        public string Company_ContactEmail { get; set; }
        public string Company_CompanyType { get; set; }
        public DateTime Company_CreationDate { get; set; }

        // Payment Information
        public long Payment_ID { get; set; }
        public decimal Payment_LiquidValue { get; set; }
        public decimal Payment_TaxValue { get; set; }
        public decimal Payment_TotalValue { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Status { get; set; }
        public DateTime Payment_CreationDate { get; set; }
        public DateTime Payment_LastUpdateDate { get; set; }
        public DateTime? Payment_PaymentDate { get; set; }
    }
}
