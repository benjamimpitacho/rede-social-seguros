using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Company
{
    public class CompanySearchFilterDTO
    {
        public string UserId { get; set; }

        #region Garages Search

        public string GarageName { get; set; }
        public string GarageDistrict { get; set; }
        public string GarageCounty { get; set; }
        public long? GarageServiceID { get; set; }
        public string GaragePartnership { get; set; }
        public string GarageOfficialAgent { get; set; }

        #endregion Garages Search

        #region Clinics Search

        public string ClinicName { get; set; }
        public string ClinicDistrict { get; set; }
        public string ClinicCounty { get; set; }
        public long? ClinicServiceID { get; set; }
        public string ClinicPartnership { get; set; }
        public string ClinicOfficialAgent { get; set; }

        #endregion Clinics Search

        #region ConstructionCompany Search

        public string ConstructionCompanyName { get; set; }
        public string ConstructionCompanyDistrict { get; set; }
        public string ConstructionCompanyCounty { get; set; }
        public long? ConstructionCompanyServiceID { get; set; }
        public string ConstructionCompanyPartnership { get; set; }
        public string ConstructionCompanyOfficialAgent { get; set; }

        #endregion ConstructionCompany Search

        #region HomeApplianceRepair Search

        public string HomeApplianceRepairName { get; set; }
        public string HomeApplianceRepairDistrict { get; set; }
        public string HomeApplianceRepairCounty { get; set; }
        public long? HomeApplianceRepairServiceID { get; set; }
        public string HomeApplianceRepairPartnership { get; set; }
        public string HomeApplianceRepairOfficialAgent { get; set; }

        #endregion HomeApplianceRepair Search

        #region InsuranceContact Search

        public string InsuranceContactName { get; set; }
        public string InsuranceContactDistrict { get; set; }
        public string InsuranceContactCounty { get; set; }
        public long? InsuranceContactServiceID { get; set; }
        public string InsuranceContactPartnership { get; set; }
        public string InsuranceContactOfficialAgent { get; set; }

        #endregion InsuranceContact Search
    }
}
