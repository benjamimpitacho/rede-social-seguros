using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Chat
{
    public class ChatDTO
    {
        public long ID { get; set; }
        public string ID_Chat { get; set; }
        public string ID_ChatCreator_User { get; set; }
        public string ChatTitle { get; set; }
        public byte[] ChatLogo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }

        public UserDTO AspNetUsers { get; set; }
        public List<ChatMemberDTO> ChatMember { get; set; }
        public List<UserProfileDTO> ChatMemberProfile { get; set; }
        public List<ChatMessageDTO> ChatMessage { get; set; }

    }
}
