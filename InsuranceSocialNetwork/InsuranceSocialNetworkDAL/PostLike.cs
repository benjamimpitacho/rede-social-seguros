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
    
    public partial class PostLike
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        public long ID_Post { get; set; }
        public System.DateTime Date { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Post Post { get; set; }
    }
}
