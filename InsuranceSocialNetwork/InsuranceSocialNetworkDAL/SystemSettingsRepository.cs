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
    public class SystemSettingsRepository
    {
        public static SystemSettings Get(string token)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context
                    .SystemSettings
                    .Fetch()
                    .FirstOrDefault(i => i.Token == token);
            }
        }

        public static void Set(string token, string value)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                SystemSettings setting = context
                    .SystemSettings
                    .Fetch()
                    .FirstOrDefault(i => i.Token == token);

                setting.Value = value;
                setting.LastChangeDate = DateTime.Now;
                context.SystemSettings.Update(setting);
                context.Save();
            }
        }
    }
}
