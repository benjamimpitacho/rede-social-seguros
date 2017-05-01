using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Post
{
    public class PostSubjectDTO
    {
        public long ID { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
