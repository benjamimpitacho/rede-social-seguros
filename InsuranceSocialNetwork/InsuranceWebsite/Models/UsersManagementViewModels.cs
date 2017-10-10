using InsuranceSocialNetworkCore.Types;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceWebsite.Models
{
    public class UsersManagementViewModel : ProfileViewModel
    {
        public UsersManagementViewModel()
        {
            AllowedEmails = new List<SelectListItem>()
            {
                new SelectListItem() { Text="item1", Value="1" },
                new SelectListItem() { Text="item2", Value="2" },
                new SelectListItem() { Text="item3", Value="3" },
                new SelectListItem() { Text="item4", Value="4" },
                new SelectListItem() { Text="item5", Value="5" }
            };
        }

        public List<UserProfileModelObject> UserProfiles { get; set; }

        public List<SelectListItem> AllowedEmails { get; set; }
        public string[] SelectedAllowedEmails { get; set; }

    }

    public class UserProfileModelObject
    {
        public UserProfileModelObject()
        {
            _userRolesList = InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.GetRoles().Select(i => new ListItemString() { Key = i.Id, Value = Resources.Resources.ResourceManager.GetString(i.Name) }).ToList();
        }

        public long ID { get; set; }
        public string ID_User { get; set; }
        //public string ID_Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public System.DateTime? Birthdate { get; set; }
        public string MobilePhone_1 { get; set; }
        public string MobilePhone_2 { get; set; }
        public string Telephone_1 { get; set; }
        public string Telephone_2 { get; set; }
        public string Address { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string CompaniesWorkingWith { get; set; }
        public string ProfessionalNumber { get; set; }
        public byte[] ProfilePhoto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public System.DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
        public UserModelObject User { get; set; }
        public RoleModelObject Role { get; set; }

        private List<ListItemString> _userRolesList;
        public IEnumerable<SelectListItem> UserRolesList
        {
            get { return new SelectList(_userRolesList, "Key", "Value"); }
        }
    }

    public class UserModelObject
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public System.DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }
}
