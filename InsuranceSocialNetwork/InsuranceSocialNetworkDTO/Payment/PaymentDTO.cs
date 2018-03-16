using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Payment
{
    public class PaymentDTO
    {
        public long ID { get; set; }
        public long? ID_Profile { get; set; }
        public long? ID_Garage { get; set; }
        public long? ID_HomeApplianceRepair { get; set; }
        public long? ID_MedicalClinic { get; set; }
        public long? ID_ConstructionCompany { get; set; }
        public long? ID_InsuranceCompanyContact { get; set; }
        public long ID_PaymentType { get; set; }
        public long ID_PaymentStatus { get; set; }
        public Guid Payment_GUID { get; set; }
        public string ep_cin { get; set; }
        public string ep_user { get; set; }
        public string ep_entity { get; set; }
        public string ep_ref_type { get; set; }
        public string ep_type { get; set; }
        public string ep_currency { get; set; }
        public string ep_country { get; set; }
        public string ep_language { get; set; }
        public string ep_reference { get; set; }
        public string ep_value { get; set; }
        public string t_value { get; set; }
        public string t_key { get; set; }
        public string o_name { get; set; }
        public string o_description { get; set; }
        public string o_obs { get; set; }
        public string o_mobile { get; set; }
        public string o_email { get; set; }
        public string o_max_date { get; set; }
        public string ep_partner { get; set; }
        public string ep_rec { get; set; }
        public string ep_rec_freq { get; set; }
        public string ep_rec_url { get; set; }
        public string ep_status { get; set; }
        public string ep_message { get; set; }
        public string ep_original_value { get; set; }
        public string ep_link { get; set; }
        public string ep_link_rp_cc { get; set; }
        public string ep_link_rp_dd { get; set; }
        public string ep_k1 { get; set; }
        public string ep_periodicity { get; set; }
        public string ep_boleto { get; set; }
        public string ep_key { get; set; }
        public string ep_doc { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? ExpiracyDate { get; set; }
        public bool Active { get; set; }
        public string Message { get; set; }
    }
}
