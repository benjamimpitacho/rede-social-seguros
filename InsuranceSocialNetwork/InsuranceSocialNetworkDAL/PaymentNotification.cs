//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsuranceSocialNetworkDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentNotification
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
        public System.DateTime NotificationDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
    }
}
