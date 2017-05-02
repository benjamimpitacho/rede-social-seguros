using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace InsuranceWebsite.Commons
{
    public static class ConfigurationSettings
    {
        public static string AppEmailAddress {
            get {
                return ConfigurationManager.AppSettings["AppEmailAddress"];
            }
        }

        public static string SmtpHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpHost"];
            }
        }

        public static int SmtpPort
        {
            get
            {
                int port;
                bool parseTry = Int32.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out port);
                if (parseTry)
                    return port;
                return 587;
            }
        }

        public static string SmtpUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpUsername"];
            }
        }

        public static string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }

    }
}