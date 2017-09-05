using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Chat
{
    public class ChatMessageDTO
    {
        public long ID { get; set; }
        public long ID_Chat { get; set; }
        public string ID_User { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }

        public UserDTO AspNetUsers { get; set; }
    }
}
