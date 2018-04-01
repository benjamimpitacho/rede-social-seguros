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
        public long? GarageDistrictID { get; set; }
        public long? GarageCountyID { get; set; }
        public long? GarageServiceID { get; set; }
        public string GaragePartnership { get; set; }
        public string GarageOfficialAgent { get; set; }

        #endregion Garages Search

        #region Clinics Search

        public string ClinicName { get; set; }
        public long? ClinicDistrictID { get; set; }
        public long? ClinicCountyID { get; set; }
        public long? ClinicServiceID { get; set; }
        public string ClinicPartnership { get; set; }
        public string ClinicOfficialAgent { get; set; }

        #endregion Clinics Search

        #region ConstructionCompany Search

        public string ConstructionCompanyName { get; set; }
        public long? ConstructionCompanyDistrictID { get; set; }
        public long? ConstructionCompanyCountyID { get; set; }
        public long? ConstructionCompanyServiceID { get; set; }
        public string ConstructionCompanyPartnership { get; set; }
        public string ConstructionCompanyOfficialAgent { get; set; }

        #endregion ConstructionCompany Search

        #region HomeApplianceRepair Search

        public string HomeApplianceRepairName { get; set; }
        public long? HomeApplianceRepairDistrictID { get; set; }
        public long? HomeApplianceRepairCountyID { get; set; }
        public long? HomeApplianceRepairServiceID { get; set; }
        public string HomeApplianceRepairPartnership { get; set; }
        public string HomeApplianceRepairOfficialAgent { get; set; }

        #endregion HomeApplianceRepair Search

        #region InsuranceContact Search

        public string InsuranceContactName { get; set; }
        public long? InsuranceContactDistrictID { get; set; }
        public long? InsuranceContactCountyID { get; set; }
        public long? InsuranceContactServiceID { get; set; }
        public string InsuranceContactPartnership { get; set; }
        public string InsuranceContactOfficialAgent { get; set; }

        #endregion InsuranceContact Search
    }
}
