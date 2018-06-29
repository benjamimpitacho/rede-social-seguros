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
                .Include(i => i.ChatNotification)
                .Include(i => i.ChatNote)
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
                ChatDelete chatDelete = context.ChatDelete.Fetch().Where(i => i.ID_Chat == chat.ID && i.ID_User == userId).FirstOrDefault();
                if (null == chatDelete)
                {
                    chat.ChatMessage = chat.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
                }
                else
                {
                    chat.ChatMessage = chat.ChatMessage.Where(i => i.CreateDate > chatDelete.LastChatDeleteDate).OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
                }
            }

            return chat;
            //}
        }

        public static Chat GetChat(BackofficeUnitOfWork context, long chatId, string userId)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
            Chat chat = context.Chat
                .Fetch()
                .Include(i => i.AspNetUsers)
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.ChatMember)
                .Include(i => i.ChatMessage)
                .Include(i => i.ChatNotification)
                .Include(i => i.ChatNote)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                .Where(i => i.ID == chatId && i.Active)
                //.Select(i => i)
                .OrderByDescending(i => i.LastChangeDate)
                .FirstOrDefault();

            if (null != chat && null != chat.ChatMessage)
            {
                ChatDelete chatDelete = context.ChatDelete.Fetch().Where(i => i.ID_Chat == chatId && i.ID_User == userId).FirstOrDefault();
                if (null == chatDelete)
                {
                    chat.ChatMessage = chat.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
                }
                else
                {
                    chat.ChatMessage = chat.ChatMessage.Where(i => i.CreateDate > chatDelete.LastChatDeleteDate).OrderByDescending(j => j.CreateDate).Take(20).OrderBy(j => j.CreateDate).ToList();
                }
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
                .Include(i => i.ChatMember.Select(j => j.AspNetUsers.Profile))
                .Include(i => i.ChatMessage)
                .Include(i => i.ChatNotification)
                .Include(i => i.ChatNote)
                .Include(i => i.ChatDelete)
                .Where(i => (
                        (i.ID_ChatCreator_User == userId)
                        || (i.ChatMember.Where(j => j.ID_User == userId).Count() > 0)
                    )
                    && i.Active
                    && (
                        i.ChatDelete.Count == 0
                        || null == i.ChatDelete.FirstOrDefault(j => j.ID_User == userId)
                        || i.ChatMessage.Max(j => j.CreateDate) > i.ChatDelete.Where(j => j.ID_User == userId).Max(j => j.LastChatDeleteDate))
                    )
                .OrderByDescending(i => i.LastChangeDate)
                .ToList();

                return chats;
            //}
        }

        public static ChatMessage GetChatMessage(BackofficeUnitOfWork context, long id)
        {
            return context.ChatMessage
                .Fetch()
                .Where(i => i.ID == id)
                .FirstOrDefault();
        }

        public static List<Chat> SearchChats(BackofficeUnitOfWork context, string userId, string searchTerm)
        {
            //using (var context = new BackofficeUnitOfWork())
            //{
            List<Chat> chats = context.Chat
                .Fetch()
                .Include(i => i.AspNetUsers)
                .Include(i => i.ChatMember)
                .Include(i => i.ChatMember.Select(j => j.AspNetUsers))
                .Include(i => i.ChatMember.Select(j => j.AspNetUsers.Profile))
                .Include(i => i.ChatMessage)
                .Include(i => i.ChatNotification)
                .Include(i => i.ChatNote)
                .Include(i => i.ChatDelete)
                .Where(i => (
                        (i.ID_ChatCreator_User == userId)
                        || (i.ChatMember.Where(j => j.ID_User == userId).Count() > 0)
                    )
                    && i.Active
                    && (
                        i.ChatDelete.Count == 0
                        || null == i.ChatDelete.FirstOrDefault(j => j.ID_User == userId)
                        || i.ChatMessage.Max(j => j.CreateDate) > i.ChatDelete.Where(j => j.ID_User == userId).Max(j => j.LastChatDeleteDate))
                    && i.ChatMessage.Any(j => j.Text.ToLower().Contains(searchTerm)))
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

        public static void MarkAllChatMessagesRead(long chatId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var messageList = context.ChatMessage
                    .Fetch()
                    .Include(i => i.Chat)
                    .Where(i => null == i.ReadDate
                        && i.ID_Chat == chatId
                        && i.ID_User != userId)
                    .Select(i => i)
                    .ToList();

                messageList.ForEach(i => i.ReadDate = DateTime.Now);

                context.Save();
            }
        }

        public static bool DeleteChat(long chatId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ChatMember chatMember = context.ChatMember
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                if (null == chatMember)
                    return false;

                ChatDelete deleteEntry = context.ChatDelete
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                if(null == deleteEntry)
                {
                    context.ChatDelete.Create(new ChatDelete() {
                        ID_Chat = chatId,
                        ID_User = userId,
                        LastChatDeleteDate = DateTime.Now
                    });
                }
                else
                {
                    deleteEntry.LastChatDeleteDate = DateTime.Now;
                    context.ChatDelete.Update(deleteEntry);
                }

                context.Save();

                return true;
            }
        }

        public static bool MarkChatAsUnread(long chatId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ChatMember chatMember = context.ChatMember
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                if (null == chatMember)
                    return false;

                ChatDelete deleteEntry = context.ChatDelete
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                ChatMessage lastMessage = null;
                if (null == deleteEntry)
                {
                    lastMessage = context.ChatMessage
                        .Fetch()
                        .Where(i => i.ID_Chat == chatId && i.ID_User != userId)
                        .OrderByDescending(i => i.CreateDate)
                        .FirstOrDefault();
                }
                else
                {
                    lastMessage = context.ChatMessage
                        .Fetch()
                        .Where(i => i.ID_Chat == chatId && i.ID_User != userId && i.CreateDate > deleteEntry.LastChatDeleteDate)
                        .OrderByDescending(i => i.CreateDate)
                        .FirstOrDefault();
                }

                if (null == lastMessage)
                    return false;

                lastMessage.ReadDate = null;
                lastMessage.LastChangeDate = DateTime.Now;
                context.ChatMessage.Update(lastMessage);
                context.Save();

                return true;
            }
        }

        public static List<ChatNotification> GetChatNotificationsState(string chatId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Chat
                    .Fetch()
                    .Include(i => i.ChatNotification)
                    .FirstOrDefault(i => i.ID_Chat == chatId)
                    .ChatNotification
                    .ToList();
            }
        }

        public static bool ChangeChatNotificationsState(long chatId, string userId, bool state)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ChatMember chatMember = context.ChatMember
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                if (null == chatMember)
                    return false;

                ChatNotification notification = context.ChatNotification
                    .Fetch()
                    .Where(i => i.ID_Chat == chatId && i.ID_User == userId)
                    .FirstOrDefault();

                if (null == notification)
                {
                    context.ChatNotification.Create(new ChatNotification()
                    {
                        ID_Chat = chatId,
                        ID_User = userId,
                        ReceiveNotifications = state,
                        LastUpdateDate = DateTime.Now
                    });
                }
                else
                {
                    notification.LastUpdateDate = DateTime.Now;
                    notification.ReceiveNotifications = state;
                    context.ChatNotification.Update(notification);
                }

                context.Save();

                return true;
            }
        }

        public static ChatNote CreateNote(ChatNote note)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                note.CreateDate = note.LastUpdateDate = DateTime.Now;

                context.ChatNote.Create(note);
                context.Save();

                return note;
            }
        }

        public static bool DeleteNote(long noteId, string ID_User)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                ChatNote note = context.ChatNote.Fetch().FirstOrDefault(i => i.ID == noteId && i.ID_User == ID_User);
                if (null == note)
                    return false;

                context.ChatNote.Delete(note.ID);
                context.Save();

                return true;
            }
        }
    }
}
