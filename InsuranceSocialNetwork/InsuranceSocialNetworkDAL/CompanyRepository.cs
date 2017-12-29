using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Company;
using InsuranceSocialNetworkCore.Enums;

namespace InsuranceSocialNetworkDAL
{
    public class CompanyRepository
    {
        public static List<Garage> SearchGarages(CompanySearchFilterDTO searchFilter)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var query = context.Garage
                    .Fetch()
                    .Include(i => i.District)
                    .Include(i => i.County)
                    .Include(i => i.GarageFavorite);

                if(!string.IsNullOrEmpty(searchFilter.GarageName))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchFilter.GarageName.ToLower()));
                }

                if (searchFilter.GarageServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service.Value == searchFilter.GarageServiceID.Value);
                }

                if (searchFilter.GarageDistrictID.HasValue)
                {
                    query = query.Where(i => i.District != null && i.ID_District == searchFilter.GarageDistrictID.Value);
                }

                if (searchFilter.GarageCountyID.HasValue)
                {
                    query = query.Where(i => i.County != null && i.ID_County == searchFilter.GarageCountyID.Value);
                }

                if (searchFilter.GarageServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service.Value == searchFilter.GarageServiceID.Value);
                }

                if (!string.IsNullOrEmpty(searchFilter.GarageOfficialAgent))
                {
                    query = query.Where(i => i.OfficialAgent.ToLower().Contains(searchFilter.GarageOfficialAgent.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchFilter.GaragePartnership))
                {
                    query = query.Where(i => i.OfficialPartner.ToLower().Contains(searchFilter.GaragePartnership.ToLower()));
                }

                return query.ToList();
            }
        }

        public static List<MedicalClinic> SearchMedicalClinics(CompanySearchFilterDTO searchFilter)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var query = context.MedicalClinic.Fetch();

                if (!string.IsNullOrEmpty(searchFilter.ClinicName))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchFilter.ClinicName.ToLower()));
                }

                if (searchFilter.ClinicServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service == searchFilter.ClinicServiceID.Value);
                }

                if (!string.IsNullOrEmpty(searchFilter.ClinicOfficialAgent))
                {
                    query = query.Where(i => i.OfficialAgent.ToLower().Contains(searchFilter.ClinicOfficialAgent.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchFilter.ClinicPartnership))
                {
                    query = query.Where(i => i.OfficialPartner.ToLower().Contains(searchFilter.ClinicPartnership.ToLower()));
                }

                return query.ToList();
            }
        }

        public static List<ConstructionCompany> SearchConstructionCompanies(CompanySearchFilterDTO searchFilter)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var query = context.ConstructionCompany.Fetch();

                if (!string.IsNullOrEmpty(searchFilter.ConstructionCompanyName))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchFilter.ConstructionCompanyName.ToLower()));
                }

                if (searchFilter.ConstructionCompanyServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service == searchFilter.ConstructionCompanyServiceID.Value);
                }

                if (!string.IsNullOrEmpty(searchFilter.ConstructionCompanyOfficialAgent))
                {
                    query = query.Where(i => i.OfficialAgent.ToLower().Contains(searchFilter.ConstructionCompanyOfficialAgent.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchFilter.ConstructionCompanyPartnership))
                {
                    query = query.Where(i => i.OfficialPartner.ToLower().Contains(searchFilter.ConstructionCompanyPartnership.ToLower()));
                }

                return query.ToList();
            }
        }

        public static List<HomeApplianceRepair> SearchHomeApplianceRepairs(CompanySearchFilterDTO searchFilter)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var query = context.HomeApplianceRepair.Fetch();

                if (!string.IsNullOrEmpty(searchFilter.HomeApplianceRepairName))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchFilter.HomeApplianceRepairName.ToLower()));
                }

                if (searchFilter.HomeApplianceRepairServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service == searchFilter.HomeApplianceRepairServiceID.Value);
                }

                if (!string.IsNullOrEmpty(searchFilter.HomeApplianceRepairOfficialAgent))
                {
                    query = query.Where(i => i.OfficialAgent.ToLower().Contains(searchFilter.HomeApplianceRepairOfficialAgent.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchFilter.HomeApplianceRepairPartnership))
                {
                    query = query.Where(i => i.OfficialPartner.ToLower().Contains(searchFilter.HomeApplianceRepairPartnership.ToLower()));
                }

                return query.ToList();
            }
        }

        public static List<InsuranceCompanyContact> SearchInsuranceCompanyContacts(CompanySearchFilterDTO searchFilter)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var query = context.InsuranceCompanyContact.Fetch();

                if (!string.IsNullOrEmpty(searchFilter.InsuranceContactName))
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchFilter.InsuranceContactName.ToLower()));
                }

                if (searchFilter.InsuranceContactServiceID.HasValue)
                {
                    query = query.Where(i => i.ID_Service == searchFilter.InsuranceContactServiceID.Value);
                }

                if (!string.IsNullOrEmpty(searchFilter.InsuranceContactOfficialAgent))
                {
                    query = query.Where(i => i.OfficialAgent.ToLower().Contains(searchFilter.InsuranceContactOfficialAgent.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchFilter.InsuranceContactPartnership))
                {
                    query = query.Where(i => i.OfficialPartner.ToLower().Contains(searchFilter.InsuranceContactPartnership.ToLower()));
                }

                return query.ToList();
            }
        }

        public static bool AddFavoriteGarage(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.GarageFavorite.Fetch().Count(i => i.ID_Garage == companyId && i.ID_User == userId) > 0)
                {
                    return false;
                }

                context.GarageFavorite.Create(new GarageFavorite() { ID_Garage = companyId, ID_User = userId, CreateDate = DateTime.Now });
                context.Save();

                return true;
            }
        }

        public static bool AddFavoriteMedicalClinic(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.MedicalClinicFavorite.Fetch().Count(i => i.ID_MedicalClinic == companyId && i.ID_User == userId) > 0)
                {
                    return false;
                }

                context.MedicalClinicFavorite.Create(new MedicalClinicFavorite() { ID_MedicalClinic = companyId, ID_User = userId, CreateDate = DateTime.Now });
                context.Save();

                return true;
            }
        }

        public static bool AddFavoriteConstructionCompany(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.ConstructionCompanyFavorite.Fetch().Count(i => i.ID_ConstructionCompany == companyId && i.ID_User == userId) > 0)
                {
                    return false;
                }

                context.ConstructionCompanyFavorite.Create(new ConstructionCompanyFavorite() { ID_ConstructionCompany = companyId, ID_User = userId, CreateDate = DateTime.Now });
                context.Save();

                return true;
            }
        }

        public static bool AddFavoriteHomeApplianceRepair(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.HomeApplianceRepairFavorite.Fetch().Count(i => i.ID_HomeApplianceRepair == companyId && i.ID_User == userId) > 0)
                {
                    return false;
                }

                context.HomeApplianceRepairFavorite.Create(new HomeApplianceRepairFavorite() { ID_HomeApplianceRepair = companyId, ID_User = userId, CreateDate = DateTime.Now });
                context.Save();

                return true;
            }
        }

        public static bool AddFavoriteInsuranceCompanyContact(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.InsuranceCompanyContactFavorite.Fetch().Count(i => i.ID_InsuranceCompanyContact == companyId && i.ID_User == userId) > 0)
                {
                    return false;
                }

                context.InsuranceCompanyContactFavorite.Create(new InsuranceCompanyContactFavorite() { ID_InsuranceCompanyContact = companyId, ID_User = userId, CreateDate = DateTime.Now });
                context.Save();

                return true;
            }
        }

        public static bool RemoveFavoriteGarage(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.GarageFavorite.Fetch().Count(i => i.ID_Garage == companyId && i.ID_User == userId) == 0)
                {
                    return false;
                }

                context.GarageFavorite.Delete(i => i.ID_Garage == companyId && i.ID_User == userId);
                context.Save();

                return true;
            }
        }

        public static bool RemoveFavoriteMedicalClinic(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.MedicalClinicFavorite.Fetch().Count(i => i.ID_MedicalClinic == companyId && i.ID_User == userId) == 0)
                {
                    return false;
                }

                context.MedicalClinicFavorite.Delete(i => i.ID_MedicalClinic == companyId && i.ID_User == userId);
                context.Save();

                return true;
            }
        }

        public static bool RemoveFavoriteConstructionCompany(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.ConstructionCompanyFavorite.Fetch().Count(i => i.ID_ConstructionCompany == companyId && i.ID_User == userId) == 0)
                {
                    return false;
                }

                context.ConstructionCompanyFavorite.Delete(i => i.ID_ConstructionCompany == companyId && i.ID_User == userId);
                context.Save();

                return true;
            }
        }

        public static bool RemoveFavoriteHomeApplianceRepair(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.HomeApplianceRepairFavorite.Fetch().Count(i => i.ID_HomeApplianceRepair == companyId && i.ID_User == userId) == 0)
                {
                    return false;
                }

                context.HomeApplianceRepairFavorite.Delete(i => i.ID_HomeApplianceRepair == companyId && i.ID_User == userId);
                context.Save();

                return true;
            }
        }

        public static bool RemoveFavoriteInsuranceCompanyContact(long companyId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (context.InsuranceCompanyContactFavorite.Fetch().Count(i => i.ID_InsuranceCompanyContact == companyId && i.ID_User == userId) == 0)
                {
                    return false;
                }

                context.InsuranceCompanyContactFavorite.Delete(i => i.ID_InsuranceCompanyContact == companyId && i.ID_User == userId);
                context.Save();

                return true;
            }
        }

        public static List<ListItem> GetCompanyServices(CompanyTypeEnum type)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                string companyType = type.ToString();

                return context.CompanyService
                    .Fetch()
                    .Include(i => i.CompanyType)
                    .Where(i => i.CompanyType.Token == companyType && i.Active)
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Description })
                    .ToList();
            }
        }
    }
}
