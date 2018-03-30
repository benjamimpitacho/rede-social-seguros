using InsuranceSocialNetworkDTO.Chat;
using InsuranceSocialNetworkDTO.Notification;
using InsuranceSocialNetworkDTO.Post;
using InsuranceSocialNetworkDTO.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsuranceWebsite.Models
{
    public class MessagesViewModel : ProfileViewModel
    {
        public MessagesViewModel()
        {
            this.Chats = new List<ChatDTO>();
        }

        public bool IsNewMessage{ get; set; }

        public List<ChatDTO> Chats { get; set; }

        public ChatDTO ActiveChat { get; set; }

        public SearchMessagesModel SearchModel { get; set; }

    }
}
