using InsuranceSocialNetworkDTO.Chat;
using System.Collections.Generic;

namespace InsuranceWebsite.Models
{
    public class SearchMessagesModel //: ProfileViewModel
    {
        public SearchMessagesModel()
        {
        }
        public string SearchTerm { get; set; }

        public List<ChatDTO> Chats { get; set; }
        public List<long> AlreadyFriends { get; set; }

    }
}
