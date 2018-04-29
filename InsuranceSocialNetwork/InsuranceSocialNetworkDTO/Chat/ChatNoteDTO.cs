using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Chat
{
    public class ChatNoteDTO
    {
        public long ID { get; set; }
        public long ID_Chat { get; set; }
        public string ID_User { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        //public UserDTO AspNetUsers { get; set; }
    }
}
