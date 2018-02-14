using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class NotificationRepository
    {
        public static List<Notification> GetUserNotifications(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                //IQueryable<Notification> query = context.Notification
                return context.Notification
                    .Fetch()
                    .Include(i=>i.NotificationType)
                    .Where(i => i.Active && i.ToUserID == Id)
                    .OrderByDescending(i => i.CreateDate)
                    .Take(50)
                    .ToList();
            }
        }

        public static bool CreateNotification(Notification notification)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                notification.Read = false;
                notification.Active = true;
                notification.CreateDate = DateTime.Now;

                context.Notification.Create(notification);
                context.Save();

                return notification.ID > 0;
            }
        }

        public static NotificationType GetNotificationType(string token)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.NotificationType.Fetch().FirstOrDefault(i => i.Token == token);
            }
        }

        public static bool MarkNotificationAsRead(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                //IQueryable<Notification> query = context.Notification
                Notification item = context.Notification
                    .Fetch()
                    .FirstOrDefault(i => i.ID == id);

                if (null == item)
                    return false;

                item.Read = true;
                item.ReadDate = DateTime.Now;

                context.Notification.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool MarkNotificationsAsRead(string id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                //IQueryable<Notification> query = context.Notification
                List<Notification> items = context.Notification
                    .Fetch()
                    .Where(i => i.ToUserID == id && !i.Read)
                    .ToList();

                if (null == items || items.Count == 0)
                    return false;

                foreach(Notification item in items)
                {
                    item.Read = true;
                    item.ReadDate = DateTime.Now;
                    context.Notification.Update(item);
                }
                
                context.Save();

                return true;
            }
        }
    }
}
