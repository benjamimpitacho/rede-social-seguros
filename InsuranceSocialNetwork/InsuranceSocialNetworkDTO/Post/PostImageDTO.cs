using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Post
{
    public class PostImageDTO
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        public long ID_Post { get; set; }
        public byte[] Image { get; set; }
        public string Type { get; set; }
        public System.DateTime Date { get; set; }
        public bool Active { get; set; }
    }
}
