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
    
    public partial class ChatMessage
    {
        public long ID { get; set; }
        public long ID_Chat { get; set; }
        public string ID_User { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
