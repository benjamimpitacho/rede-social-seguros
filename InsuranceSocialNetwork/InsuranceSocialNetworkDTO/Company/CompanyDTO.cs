using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Company
{
    public class CompanyDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NIF { get; set; }
        public string ContactEmail { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        //public long? ID_PostalCode { get; set; }
        public long? ID_District { get; set; }
        public long? ID_County { get; set; }
        public long? ID_Parish { get; set; }
        public long? ID_Service { get; set; }
        public byte[] LogoPhoto { get; set; }
        public string Website { get; set; }
        public string OfficialPartner { get; set; }
        public string OfficialAgent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
        public bool IsFavorite { get; set; }
        public string BusinessName { get; set; }
        public string IBAN { get; set; }

        public List<PaymentDTO> Payment { get; set; }
    }
}
