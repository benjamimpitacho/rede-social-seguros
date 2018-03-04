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
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string CompanyName { get; set; }
        public string ProfessionalNumber { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Locality { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string CompaniesWorkingWith { get; set; }
        public Nullable<long> ID_PostalCode { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string Website { get; set; }
        public string AboutMe { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool Active { get; set; }
        public UserDTO User { get; set; }
        public RoleDTO Role { get; set; }

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
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }
}
