﻿using InsuranceSocialNetworkDTO.Chat;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class ChatViewModel : ProfileViewModel
    {
        public ChatDTO Chat { get; set; }

        public string ChatOtherNames
        {
            get
            {
                return string.Join(",", this.Chat.ChatMemberProfile.Where(i => i.ID_User != this.Profile.ID_User).Select(j => j.FirstName + " " + j.LastName).ToArray());
            }
        }
    }

    public class NoteModelObject
    {
        public NoteModelObject() { }

        public long ID_Chat { get; set; }
        public string ID_User { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
