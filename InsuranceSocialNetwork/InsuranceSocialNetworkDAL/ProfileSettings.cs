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
    
    public partial class ProfileSettings
    {
        public long ID { get; set; }
        public long ID_Profile { get; set; }
        public bool ShowDisplayName { get; set; }
        public bool ShowBirthDate { get; set; }
        public bool ShowContactInformation { get; set; }
        public bool ShowSocialNetworks { get; set; }
        public bool LikesOnYourPosts { get; set; }
        public bool CommentsOnYourPosts { get; set; }
        public bool ReceiveNotificationsByEmail { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool Active { get; set; }
    
        public virtual Profile Profile { get; set; }
    }
}