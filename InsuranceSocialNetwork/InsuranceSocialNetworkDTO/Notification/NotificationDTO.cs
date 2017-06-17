using InsuranceSocialNetworkCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Notification
{
    public class NotificationDTO
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        public long ID_NotificationType { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ReadDate { get; set; }
        public bool Read { get; set; }
        public bool Active { get; set; }

        public NotificationTypeDTO NotificationType { get; set; }

        public NotificationTypeEnum Type { get; set; }

        public string TextToNotificationList
        {
            get
            {
                switch(Type)
                {
                    case NotificationTypeEnum.COMPLETE_PROFILE_INFO:
                        return "COMPLETE_PROFILE_INFO";
                    case NotificationTypeEnum.NEW_MESSAGE_RECEIVED:
                        return "NEW_MESSAGE_RECEIVED";
                    case NotificationTypeEnum.NEW_POST_COMMENT:
                        return "NEW_POST_COMMENT";
                    case NotificationTypeEnum.NEW_POST_LIKE:
                        return "NEW_POST_LIKE";
                    default:
                        return "";
                }
            }
        }
    }
}
