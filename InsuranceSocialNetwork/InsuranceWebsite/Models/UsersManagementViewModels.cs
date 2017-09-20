﻿using InsuranceSocialNetworkDTO.Notification;
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
        }

        public List<UserProfileModelObject> UserProfiles { get; set; }

    }

    public class UserProfileModelObject
    {
        public UserProfileModelObject()
        {
            _userRolesList = InsuranceSocialNetworkBusiness.InsuranceBusiness.BusinessLayer.GetRoles().Select(i => new ListItemStr() { Id = i.Id, Name = Resources.Resources.ResourceManager.GetString(i.Name) }).ToList();
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
        public long? ID_PostalCode { get; set; }
        public byte[] ProfilePhoto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public System.DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
        public UserModelObject User { get; set; }
        public RoleModelObject Role { get; set; }

        private List<ListItemStr> _userRolesList;
        public IEnumerable<SelectListItem> UserRolesList
        {
            get { return new SelectList(_userRolesList, "Id", "Name"); }
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
