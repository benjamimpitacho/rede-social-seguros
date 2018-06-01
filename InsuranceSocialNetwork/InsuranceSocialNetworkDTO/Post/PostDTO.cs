using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.PostalCode;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.Post
{
    public class PostDTO
    {
        public long ID { get; set; }
        public string ID_User { get; set; }
        public long ID_PostType { get; set; }
        public long ID_PostSubject { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Video { get; set; }
        public string URL { get; set; }
        public string URL_Domain { get; set; }
        public string URL_Title { get; set; }
        public string URL_Description { get; set; }
        public string URL_Image_Address { get; set; }

        public bool IsRepost { get; set; }
        public string Repost_Text { get; set; }
        public long? Repost_PostID { get; set; }
        public long? Repost_ProfileID { get; set; }

        public long? ID_County { get; set; }
        public long? ID_District { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Sticky { get; set; }
        public bool Sponsored { get; set; }
        public bool Active { get; set; }

        public PostTypeEnum Type { get; set; }
        public PostSubjectEnum Subject { get; set; }
        public PostTypeDTO PostType { get; set; }
        public PostSubjectDTO PostSubject { get; set; }

        public PostDTO RepostPost { get; set; }

        public UserProfileDTO PostOwner { get; set; }

        public List<PostCommentDTO> PostComment { get; set; }

        public List<PostLikeDTO> PostLike { get; set; }

        public List<PostImageDTO> PostImage { get; set; }

        //public List<PostDTO> Post1 { get; set; }
        public PostDTO Post2 { get; set; }

        public CountyDTO County { get; set; }
        public DistrictDTO District { get; set; }

        /*
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PostSubject PostSubject { get; set; }
        public virtual PostType PostType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostComment> PostComment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLike> PostLike { get; set; }
        */

        public string ImageSource
        {
            get
            {
                if (null == PostImage || PostImage.Count == 0)
                    return string.Empty;

                //string mimeType = /* Get mime type somehow (e.g. "image/png") */;
                string base64 = Convert.ToBase64String(PostImage[0].Image);
                return string.Format("data:{0};base64,{1}", PostImage[0].Type, base64);
            }
        }
    }
}
