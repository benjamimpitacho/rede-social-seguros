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
    
    public partial class SystemLog
    {
        public long ID { get; set; }
        public System.DateTime LogDate { get; set; }
        public string Level { get; set; }
        public string ID_User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
