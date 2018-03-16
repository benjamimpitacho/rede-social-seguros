using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;

namespace InsuranceSocialNetworkDAL
{
    public class SystemLogRepository
    {
        public static void Log(string level, string userId, string title, string message)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                SystemLog log = new SystemLog()
                {
                    Level = level,
                    LogDate = DateTime.Now,
                    Title = title,
                    Message = message
                };

                if(!string.IsNullOrEmpty(userId))
                {
                    log.ID_User = userId;
                }

                context.SystemLog.Create(log);
                context.Save();
            }
        }
    }
}
