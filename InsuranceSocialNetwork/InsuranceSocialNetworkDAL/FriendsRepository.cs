using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class FriendsRepository
    {
        public static bool AddFriend(long userId, long newFriendId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Friend friend = new Friend()
                {
                    ID_User = context.Profile.Fetch().FirstOrDefault(i => i.ID == userId).AspNetUsers.Id,
                    ID_User_Friend = context.Profile.Fetch().FirstOrDefault(i => i.ID == newFriendId).AspNetUsers.Id,
                    CreateDate = DateTime.Now,
                    ID_FriendStatus = context.FriendStatus.Fetch().FirstOrDefault(i => i.Token == "REQUEST_SENT").ID
                };

                context.Friend.Create(friend);
                
                context.Save();

                return friend.ID > 0;
            }
        }
    }
}
