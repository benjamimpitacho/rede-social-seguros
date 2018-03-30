using InsuranceSocialNetworkDTO.PostalCode;
using InsuranceSocialNetworkDTO.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.UserProfile
{
    public class UserProfileDTO
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public DateTime? Birthdate { get; set; }
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
        //public string CompaniesWorkingWith { get; set; }
        public long? ID_PostalCode { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
        public string CompanyName { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string Fax { get; set; }
        public bool SameInformationForInvoice { get; set; }
        public string Invoice_Address { get; set; }
        public string Invoice_PostalCode { get; set; }
        public long? Invoice_ID_Parish { get; set; }
        public long? Invoice_ID_County { get; set; }
        public long? Invoice_ID_District { get; set; }
        public long? ID_Parish { get; set; }
        public long? ID_County { get; set; }
        public long? ID_District { get; set; }

        public UserDTO User { get; set; }
        public RoleDTO Role { get; set; }
        public ParishDTO Parish { get; set; }
        public CountyDTO County { get; set; }
        public DistrictDTO District { get; set; }
        public List<ProfileSettingsDTO> ProfileSettings { get; set; }

        public int TotalFriends { get; set; }
        public int TotalLikes { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }

    public class ProfileSettingsDTO
    {
        public long ID { get; set; }
        public long ID_Profile { get; set; }
        public bool ShowDisplayName { get; set; }
        public bool ShowBirthDate { get; set; }
        public bool ShowContactInformation { get; set; }
        public bool ShowSocialNetworks { get; set; }
        public bool LikesOnYourPosts { get; set; }
        public bool CommentsOnYourPosts { get; set; }
        public bool ReceiveNotificationsByEmail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
    }
}
