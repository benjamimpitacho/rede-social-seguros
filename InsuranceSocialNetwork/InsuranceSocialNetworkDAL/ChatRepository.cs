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
                    .Include(i => i.AspNetUsers.Profile)
                    .Where(i => i.AspNetUsers.Profile.FirstOrDefault().ID == profileId && i.Active)
                    .OrderByDescending(i => i.LastChangeDate)
                    .ToList();
            }
        }

        public static Chat CreateChat(Chat chat)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                chat.Active = true;
                chat.CreateDate = chat.LastChangeDate = DateTime.Now;

                context.Chat.Create(chat);
                context.Save();

                return chat;
            }
        }

        public static Chat GetChat(string userId, string userId2)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Chat
                    .Fetch()
                    .Include(i => i.AspNetUsers.Profile)
                    .Include(i => i.ChatMember)
                    .Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                    .Where(i => ((i.AspNetUsers.Id == userId && i.ChatMember.Where(j => j.AspNetUsers.Id == userId2).Count() > 0)
                        || (i.AspNetUsers.Id == userId2 && i.ChatMember.Where(j => j.AspNetUsers.Id == userId).Count() > 0))
                        && i.Active)
                    .OrderByDescending(i => i.LastChangeDate)
                    .FirstOrDefault();
            }
        }

        public static List<Chat> GetChats(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Chat
                    .Fetch()
                    .Include(i => i.AspNetUsers.Profile)
                    .Include(i => i.ChatMember)
                    .Where(i => ((i.AspNetUsers.Id == userId)
                        || (i.ChatMember.Where(j => j.AspNetUsers.Id == userId).Count() > 0))
                        && i.Active)
                    .OrderByDescending(i => i.LastChangeDate)
                    .ToList();
            }
        }
            }
}
