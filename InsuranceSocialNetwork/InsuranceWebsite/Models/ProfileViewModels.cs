using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class ProfileViewModel
    {
        public UserProfileDTO Profile { get; set; }

        public UserProfileDTO ProfileInfo { get; set; }

        public List<NotificationDTO> Notifications { get; set; }

        public int TotalUnreadNotifications { get { return null == Notifications ? 0 : Notifications.Count(i => !i.Read); } }

        public int TotalUnreadMessages { get; set; }

        public bool IsOwnProfile { get; set; }
        public bool IsProfileConfigurations { get; set; }

        public bool IsProfileEdit { get; set; }
        public bool IsProfileEditConfiguations { get; set; }

        public bool IsFriendRequest { get; set; }

        public bool IsFriend { get; set; }

        public ProfileSettingsModel ProfileSettings { get; set; }

        public List<SelectListItem> AllowedEmails { get; set; }
        public string[] SelectedAllowedEmails { get; set; }
        public string AllowedEmailsText {
            get {
                return string.Join(";", AllowedEmails.Select(o => o.Text));
            }
        }
    }

    public class ProfileEditModel : ProfileViewModel
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        public System.DateTime? Birthdate { get; set; }
        public string CompanyName { get; set; }
        public string ProfessionalNumber { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        [MaxLength(10)]
        public string PostalCode { get; set; }
        //[MaxLength(64)]
        public long? ID_Parish { get; set; }
        //[MaxLength(64)]
        public long? ID_County { get; set; }
        //[MaxLength(64)]
        public long? ID_District { get; set; }
        public bool SameInformationForInvoice { get; set; }
        public string Invoice_Address { get; set; }
        public string Invoice_PostalCode { get; set; }
        public long? Invoice_ID_Parish { get; set; }
        public long? Invoice_ID_County { get; set; }
        public long? Invoice_ID_District { get; set; }
        [Url]
        public string Facebook { get; set; }
        [Url]
        public string Twitter { get; set; }
        [Url]
        public string GooglePlus { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string CompaniesWorkingWith { get; set; }
        public long? ID_PostalCode { get; set; }
        public byte[] ProfilePhoto { get; set; }
        [Url]
        public string Website { get; set; }
        public string AboutMe { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public bool Active { get; set; }

        //public List<SelectListItem> AllowedEmails { get; set; }
        //public string[] SelectedAllowedEmails { get; set; }
        //public string AllowedEmailsText
        //{
        //    get
        //    {
        //        return string.Join(";", AllowedEmails.Select(o => o.Text));
        //    }
        //}

        //public ProfileSettingsModel Settings { get; set; }
    }

    public class ProfileSettingsModel
    {
        #region Password
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordMustMatch")]
        public string RepeatPassword { get; set; }
        #endregion Password

        #region Privacy
        public bool ShowDisplayName { get; set; }
        public bool ShowBirthDate { get; set; }
        public bool ShowContactInformation { get; set; }
        public bool ShowSocialNetworks { get; set; }
        #endregion Privacy

        #region Notifications
        public bool LikesOnYourPosts { get; set; }
        public bool CommentsOnYourPosts { get; set; }
        public bool ReceiveNotificationsByEmail { get; set; }
        #endregion Notifications
    }
}
