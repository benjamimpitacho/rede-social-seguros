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
    }
}
