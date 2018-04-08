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
                string userId1 = context.Profile.Fetch().FirstOrDefault(i => i.ID == userId).AspNetUsers.Id;
                string userId2 = context.Profile.Fetch().FirstOrDefault(i => i.ID == newFriendId).AspNetUsers.Id;

                long requestSentStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_SENT").ID;

                Friend friend = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        i.AspNetUsers.Id == userId1 && i.AspNetUsers1.Id == userId2
                        || i.AspNetUsers1.Id == userId1 && i.AspNetUsers.Id == userId2);

                if (null == friend)
                {
                    friend = new Friend()
                    {
                        ID_User = context.Profile.Fetch().FirstOrDefault(i => i.ID == userId).AspNetUsers.Id,
                        ID_User_Friend = context.Profile.Fetch().FirstOrDefault(i => i.ID == newFriendId).AspNetUsers.Id,
                        CreateDate = DateTime.Now,
                        LastChangeDate = DateTime.Now,
                        ID_FriendStatus = requestSentStatusId
                    };
                    context.Friend.Create(friend);
                }
                else
                {
                    friend.ID_FriendStatus = requestSentStatusId;
                    friend.LastChangeDate = DateTime.Now;
                    context.Friend.Update(friend);
                }
                
                context.Save();

                return friend.ID > 0;
            }
        }

        public static bool RemoveFriend(string userId, string friendUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestCanceledStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_CANCELED").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        i.AspNetUsers.Id == userId && i.AspNetUsers1.Id == friendUserId
                        || i.AspNetUsers1.Id == userId && i.AspNetUsers.Id == friendUserId);

                if (null == friendItem)
                    return true;

                friendItem.ID_FriendStatus = requestCanceledStatusId;
                friendItem.LastChangeDate = DateTime.Now;

                context.Friend.Update(friendItem);
                context.Save();

                return true;
            }
        }

        public static bool AcceptFriendRequest(string userId, string newFriendId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestAcceptedStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_ACCEPTED").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        i.AspNetUsers.Id == userId && i.AspNetUsers1.Id == newFriendId
                        || i.AspNetUsers1.Id == userId && i.AspNetUsers.Id == newFriendId);

                if (null == friendItem)
                    return false;

                friendItem.ID_FriendStatus = requestAcceptedStatusId;
                friendItem.LastChangeDate = DateTime.Now;

                context.Friend.Update(friendItem);
                context.Save();

                return true;
            }
        }

        public static bool IgnoreFriendRequest(string userId, string newFriendId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestRefusedStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_REFUSED").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        i.AspNetUsers.Id == userId && i.AspNetUsers1.Id == newFriendId
                        || i.AspNetUsers1.Id == userId && i.AspNetUsers.Id == newFriendId);

                if (null == friendItem)
                    return false;

                friendItem.ID_FriendStatus = requestRefusedStatusId;
                friendItem.LastChangeDate = DateTime.Now;

                context.Friend.Update(friendItem);
                context.Save();

                return true;
            }
        }

        public static bool AreFriends(string userId, string otherUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestAcceptedStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_ACCEPTED").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        (i.AspNetUsers.Id == userId && i.AspNetUsers1.Id == otherUserId
                        || i.AspNetUsers1.Id == userId && i.AspNetUsers.Id == otherUserId)
                        &&
                        (i.ID_FriendStatus == requestAcceptedStatusId)
                        );

                if (null == friendItem)
                    return false;

                return true;
            }
        }

        public static bool HasPendingFriendRequest(string userId, string otherUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestSentStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_SENT").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        (i.AspNetUsers1.Id == userId && i.AspNetUsers.Id == otherUserId)
                        &&
                        (i.ID_FriendStatus == requestSentStatusId)
                        );

                if (null == friendItem)
                    return false;

                return true;
            }
        }

        public static bool HasPendingFriendRequested(string userId, string otherUserId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestSentStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_SENT").ID;

                Friend friendItem = context.Friend
                    .Fetch()
                    .FirstOrDefault(i =>
                        (i.AspNetUsers.Id == userId && i.AspNetUsers1.Id == otherUserId)
                        &&
                        (i.ID_FriendStatus == requestSentStatusId)
                        );

                if (null == friendItem)
                    return false;

                return true;
            }
        }

        public static int GetTotalFriends(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                long requestAcceptedStatusId = context.FriendStatus.Fetch().FirstOrDefault(j => j.Token == "REQUEST_ACCEPTED").ID;

                return context.Friend
                    .Fetch()
                    .Where(i =>
                        (i.AspNetUsers.Id == userId || i.AspNetUsers1.Id == userId)
                        &&
                        (i.ID_FriendStatus == requestAcceptedStatusId)
                        )
                    .Count();
            }
        }
    }
}
