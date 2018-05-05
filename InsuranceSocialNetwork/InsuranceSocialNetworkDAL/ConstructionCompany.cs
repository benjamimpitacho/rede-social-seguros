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
    
    public partial class ConstructionCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConstructionCompany()
        {
            this.ConstructionCompanyFavorite = new HashSet<ConstructionCompanyFavorite>();
            this.Payment = new HashSet<Payment>();
        }
    
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
        public Nullable<long> ID_District { get; set; }
        public Nullable<long> ID_County { get; set; }
        public Nullable<long> ID_Parish { get; set; }
        public Nullable<long> ID_Service { get; set; }
        public byte[] LogoPhoto { get; set; }
        public string Website { get; set; }
        public string OfficialPartner { get; set; }
        public string OfficialAgent { get; set; }
        public string BusinessName { get; set; }
        public string IBAN { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool Active { get; set; }
        public string Fax { get; set; }
        public string PostalCode { get; set; }
        public bool SameInformationForInvoice { get; set; }
        public string Invoice_Address { get; set; }
        public string Invoice_PostalCode { get; set; }
        public Nullable<long> Invoice_ID_Parish { get; set; }
        public Nullable<long> Invoice_ID_County { get; set; }
        public Nullable<long> Invoice_ID_District { get; set; }
        public Nullable<int> LibaxEntityID { get; set; }
        public string ID_User { get; set; }
    
        public virtual CompanyService CompanyService { get; set; }
        public virtual County County { get; set; }
        public virtual District District { get; set; }
        public virtual County County1 { get; set; }
        public virtual District District1 { get; set; }
        public virtual Parish Parish { get; set; }
        public virtual Parish Parish1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstructionCompanyFavorite> ConstructionCompanyFavorite { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
