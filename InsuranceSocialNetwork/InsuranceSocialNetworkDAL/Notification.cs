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
    
    public partial class Notification
    {
        public long ID { get; set; }
        public string ToUserID { get; set; }
        public string FromUserID { get; set; }
        public long ID_NotificationType { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public Nullable<long> ID_Post { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public bool Read { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual Post Post { get; set; }
    }
}
