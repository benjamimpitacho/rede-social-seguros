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
    
    public partial class PostalCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostalCode()
        {
            this.Garage = new HashSet<Garage>();
            this.MedicalClinic = new HashSet<MedicalClinic>();
            this.Profile = new HashSet<Profile>();
        }
    
        public long ID { get; set; }
        public int Codigo_Postal { get; set; }
        public int Extensao { get; set; }
        public string Designacao_Postal { get; set; }
        public string Localidade { get; set; }
        public Nullable<int> Codigo_Arteria { get; set; }
        public string Tipo_Arteria { get; set; }
        public string Prep1 { get; set; }
        public string Titulo_Arteria { get; set; }
        public string Prep2 { get; set; }
        public string Nome_Arteria { get; set; }
        public string Local_Arteria { get; set; }
        public string Troco { get; set; }
        public Nullable<int> Porta { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Garage> Garage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalClinic> MedicalClinic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profile { get; set; }
    }
}
