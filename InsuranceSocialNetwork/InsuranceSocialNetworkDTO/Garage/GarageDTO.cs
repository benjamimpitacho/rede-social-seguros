using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Garage
{
    public class GarageDTO
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
        public long? ID_PostalCode { get; set; }
        public byte[] LogoPhoto { get; set; }
        public string Website { get; set; }
        public string OficialPartner { get; set; }
        public string OficialAgent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }

        //public virtual PostalCode PostalCode { get; set; }
    }
}
