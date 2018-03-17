using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Payment
{
    public class PaymentNotificationDTO
    {
        public long ID { get; set; }
        public string ep_key { get; set; }
        public string ep_doc { get; set; }
        public string ep_cin { get; set; }
        public string ep_user { get; set; }
        public string ep_status { get; set; }
        public string ep_entity { get; set; }
        public string ep_reference { get; set; }
        public string ep_value { get; set; }
        public string ep_date { get; set; }
        public string ep_payment_type { get; set; }
        public string ep_value_fixed { get; set; }
        public string ep_value_var { get; set; }
        public string ep_value_tax { get; set; }
        public string ep_value_transf { get; set; }
        public string ep_date_transf { get; set; }
        public string t_key { get; set; }
        public string ep_type { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime LastChangeDate { get; set; }
    }
}
