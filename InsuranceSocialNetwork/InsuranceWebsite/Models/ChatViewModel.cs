using InsuranceSocialNetworkDTO.Chat;
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

    }
}
