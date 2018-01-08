using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InsuranceSocialNetworkDAL
{
    public class ChatRepository
    {
        public static List<Chat> GetUserChats(long profileId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Chat
                    .Fetch()
                    .Where(i => i.AspNetUsers.Profile.FirstOrDefault().ID == profileId && i.Active)
                    .OrderByDescending(i => i.LastChangeDate)
                    .ToList();
            }
        }

        public static Chat CreateChat(BackofficeUnitOfWork context, Chat chat)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
                chat.Active = true;
                chat.CreateDate = chat.LastChangeDate = DateTime.Now;
                chat.ID_Chat = Guid.NewGuid().ToString().Replace("-", "");

                context.Chat.Create(chat);
                context.Save();

                return chat;
            //}
        }

        public static Chat GetChat(BackofficeUnitOfWork context, string userId, string userId2)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
            Chat chat = context.Chat
                .Fetch()
                .Include(i => i.AspNetUsers)
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.ChatMember)
                .Include(i => i.ChatMessage)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                .Where(i => (
                        (i.AspNetUsers.Id == userId && i.ChatMember.Where(j => j.AspNetUsers.Id == userId2).Count() > 0)
                        || (i.AspNetUsers.Id == userId2 && i.ChatMember.Where(j => j.AspNetUsers.Id == userId).Count() > 0)
                    )
                    && i.Active)
                //.Select(i => i)
                .OrderByDescending(i => i.LastChangeDate)
                .FirstOrDefault();

            if (null != chat && null != chat.ChatMessage)
            {
                chat.ChatMessage = chat.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
            }

            return chat;
            //}
        }

        public static Chat GetChat(BackofficeUnitOfWork context, long chatId)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
            Chat chat = context.Chat
                .Fetch()
                .Include(i => i.AspNetUsers)
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.ChatMember)
                .Include(i => i.ChatMessage)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                .Where(i => i.ID == chatId && i.Active)
                //.Select(i => i)
                .OrderByDescending(i => i.LastChangeDate)
                .FirstOrDefault();

            if (null != chat && null != chat.ChatMessage)
            {
                chat.ChatMessage = chat.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
            }

            return chat;
            //}
        }

        public static List<Chat> GetChats(BackofficeUnitOfWork context, string userId)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
                List<Chat> chats = context.Chat
                    .Fetch()
                    .Include(i => i.AspNetUsers)
                    //.Include(i => i.AspNetUsers.Profile)
                    .Include(i => i.ChatMember)
                    .Include(i => i.ChatMember.Select(j => j.AspNetUsers))
                    .Include(i => i.ChatMessage)
                    .Where(i => (
                            (i.ID_ChatCreator_User == userId)
                            || (i.ChatMember.Where(j => j.ID_User == userId).Count() > 0)
                        )
                        && i.Active)
                    //.Select(i => i)
                    .OrderByDescending(i => i.LastChangeDate)
                    .ToList();

                return chats;
            //}
        }

        public static List<string> GetChatMembersUserIds(string chatId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Chat chat = context.Chat
                    .Fetch()
                    .FirstOrDefault(i => i.ID_Chat == chatId);

                if (null == chat)
                    return new List<string>();

                return chat.ChatMember.Where(i => i.ID_User != userId).Select(i => i.ID_User).ToList();
            }
        }

        public static int GetTotalUnreadMessages(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<long> chatIds = context.Chat
                        .Fetch()
                        .Include(i => i.ChatMember)
                        .Where(i => i.ChatMember.Select(j => j.AspNetUsers.Id).ToList().Contains(userId))
                        .Select(i => i.ID)
                        .ToList();

                return context.ChatMessage
                    .Fetch()
                    .Include(i => i.Chat)
                    .Where(i => null == i.ReadDate
                        && i.ID_User != userId
                        && chatIds.Contains(i.ID_Chat))
                    .Select(i => i.ID_Chat)
                    .Distinct()
                    .Count();
            }
        }
    }
}
