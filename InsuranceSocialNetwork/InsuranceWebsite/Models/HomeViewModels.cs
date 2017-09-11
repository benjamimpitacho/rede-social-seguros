using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class HomeViewModel : ProfileViewModel
    {
        public HomeViewModel()
        {
            this.IsPostsView = true;
            this.SearchModel = new SearchViewModel();
        }

        public bool IsPostsView
        {
            get { return _isPostsView; }
            set { _isPostsView = value; _isMessagesView = false; _isNotificationsView = false; _isProfileSearchView = false; _isSearchView = false; }
        }
        public bool IsMessagesView
        {
            get { return _isMessagesView; }
            set { _isPostsView = false; _isMessagesView = value; _isNotificationsView = false; _isProfileSearchView = false; _isSearchView = false; }
        }
        public bool IsNotificationsView
        {
            get { return _isNotificationsView; }
            set { _isPostsView = false; _isMessagesView = false; _isNotificationsView = value; _isProfileSearchView = false; _isSearchView = false; }
        }
        public bool IsSearchView
        {
            get { return _isSearchView; }
            set { _isPostsView = false; _isMessagesView = false; _isNotificationsView = false; _isProfileSearchView = false; _isSearchView = true; }
        }
        public bool IsProfileSearchView
        {
            get { return _isProfileSearchView; }
            set { _isPostsView = false; _isMessagesView = false; _isNotificationsView = false; _isProfileSearchView = true; _isSearchView = false; }
        }

        public bool IsSearchGarages { get; set; }
        public bool IsSearchClinics { get; set; }
        public bool IsSearchConstructionCompanies { get; set; }
        public bool IsSearchHomeApplianceRepair { get; set; }
        public bool IsSearchInsuranceContacts { get; set; }

        public List<PostDTO> Posts { get; set; }

        public SearchViewModel SearchModel { get; set; }

        #region Search Fields

        #region Garages Search

        public string GarageName { get; set; }
        public string GarageDistrict { get; set; }
        public string GarageLocation { get; set; }
        public string GarageService { get; set; }
        public string GaragePartnership { get; set; }
        public string GarageOfficialAgent { get; set; }

        #endregion Garages Search

        #region Clinics Search

        public string ClinicName { get; set; }
        public string ClinicDistrict { get; set; }
        public string ClinicLocation { get; set; }
        public string ClinicService { get; set; }
        public string ClinicPartnership { get; set; }
        public string ClinicOfficialAgent { get; set; }

        #endregion Clinics Search

        #region ConstructionCompany Search

        public string ConstructionCompanyName { get; set; }
        public string ConstructionCompanyDistrict { get; set; }
        public string ConstructionCompanyLocation { get; set; }
        public string ConstructionCompanyService { get; set; }
        public string ConstructionCompanyPartnership { get; set; }
        public string ConstructionCompanyOfficialAgent { get; set; }

        #endregion ConstructionCompany Search

        #region HomeApplianceRepair Search

        public string HomeApplianceRepairName { get; set; }
        public string HomeApplianceRepairDistrict { get; set; }
        public string HomeApplianceRepairLocation { get; set; }
        public string HomeApplianceRepairService { get; set; }
        public string HomeApplianceRepairPartnership { get; set; }
        public string HomeApplianceRepairOfficialAgent { get; set; }

        #endregion HomeApplianceRepair Search

        #region InsuranceContact Search

        public string InsuranceContactName { get; set; }
        public string InsuranceContactDistrict { get; set; }
        public string InsuranceContactLocation { get; set; }
        public string InsuranceContactService { get; set; }
        public string InsuranceContactPartnership { get; set; }
        public string InsuranceContactOfficialAgent { get; set; }

        #endregion InsuranceContact Search

        #endregion Search Fields

        #region Private Properties

        private bool _isPostsView { get; set; }
        private bool _isMessagesView { get; set; }
        private bool _isNotificationsView { get; set; }
        private bool _isSearchView { get; set; }
        private bool _isProfileSearchView { get; set; }

        #endregion Private Properties
    }
}
