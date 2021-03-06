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
    
    public partial class CompanyService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyService()
        {
            this.ConstructionCompany = new HashSet<ConstructionCompany>();
            this.Garage = new HashSet<Garage>();
            this.HomeApplianceRepair = new HashSet<HomeApplianceRepair>();
            this.InsuranceCompanyContact = new HashSet<InsuranceCompanyContact>();
            this.MedicalClinic = new HashSet<MedicalClinic>();
        }
    
        public long ID { get; set; }
        public long ID_CompanyType { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    
        public virtual CompanyType CompanyType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstructionCompany> ConstructionCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Garage> Garage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeApplianceRepair> HomeApplianceRepair { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InsuranceCompanyContact> InsuranceCompanyContact { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalClinic> MedicalClinic { get; set; }
    }
}
