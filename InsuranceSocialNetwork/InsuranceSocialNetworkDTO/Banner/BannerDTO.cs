using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Banner
{
    public class BannerDTO
    {
        public long ID { get; set; }
        public long ID_Banner_Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }

        public BannerTypeDTO BannerType { get; set; }
    }
}
