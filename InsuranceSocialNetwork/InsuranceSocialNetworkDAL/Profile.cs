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
    
    public partial class Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            this.Payment = new HashSet<Payment>();
            this.ProfileSettings = new HashSet<ProfileSettings>();
        }
    
        public long ID { get; set; }
        public string ID_User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string AboutMe { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string Website { get; set; }
        public string ProfessionalNumber { get; set; }
        public string CompaniesWorkingWith { get; set; }
        public Nullable<long> ID_PostalCode { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool Active { get; set; }
        public string CompanyName { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string Fax { get; set; }
        public bool SameInformationForInvoice { get; set; }
        public string Invoice_Address { get; set; }
        public string Invoice_PostalCode { get; set; }
        public Nullable<long> Invoice_ID_Parish { get; set; }
        public Nullable<long> Invoice_ID_County { get; set; }
        public Nullable<long> Invoice_ID_District { get; set; }
        public Nullable<long> ID_Parish { get; set; }
        public Nullable<long> ID_County { get; set; }
        public Nullable<long> ID_District { get; set; }
        public Nullable<int> LibaxEntityID { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual County County { get; set; }
        public virtual County County1 { get; set; }
        public virtual District District { get; set; }
        public virtual District District1 { get; set; }
        public virtual Parish Parish { get; set; }
        public virtual Parish Parish1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileSettings> ProfileSettings { get; set; }
    }
}
