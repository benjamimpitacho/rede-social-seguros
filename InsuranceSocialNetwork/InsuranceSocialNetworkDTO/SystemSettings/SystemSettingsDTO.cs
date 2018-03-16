using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.SystemSettings
{
    public class SystemSettingsDTO
    {
        public long ID { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DateTime LastChangeDate { get; set; }
        public bool Active { get; set; }
    }
}
