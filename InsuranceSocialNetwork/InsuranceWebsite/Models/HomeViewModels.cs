using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class HomeViewModel : ProfileViewModel
    {
        public HomeViewModel()
        {
            this.IsPostsView = true;
            this.SearchModel = new SearchViewModel();
            this.SearchCompaniesModel = new SearchCompaniesViewModel();

            this.PostSubjects = InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.GetSubjectTypes().Select(i => new SelectListItem() { Value = i.Key.ToString(), Text = Resources.Resources.ResourceManager.GetString(i.Value) }).ToList();
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

        public bool IsCompanyProfileEdit { get; set; }

        public bool IsProfileTimeline { get; set; }
        public long? TimelineProfileId { get; set; }

        public long? NewPostSuject { get; set; }
        public List<SelectListItem> PostSubjects { get; set; }

        public List<PostDTO> Posts { get; set; }
        public List<ListItemString> QuickLinks { get; set; }

        public List<UserProfileDTO> Friends { get; set; }

        public SearchViewModel SearchModel { get; set; }

        public SearchCompaniesViewModel SearchCompaniesModel { get; set; }

        public ProfileEditModel ProfileEditModel { get; set; }

        public CompanyModelObject CompanyModel { get; set; }

        public PostItemsViewModel PostsModel { get; set; }

        #region Banners Fields

        public List<BannerDTO> TopBanners { get; set; }
        public List<BannerDTO> SideBanners { get; set; }

        #endregion Banners Fields

        #region Private Properties

        private bool _isPostsView { get; set; }
        private bool _isMessagesView { get; set; }
        private bool _isNotificationsView { get; set; }
        private bool _isSearchView { get; set; }
        private bool _isProfileSearchView { get; set; }

        #endregion Private Properties
    }
}
