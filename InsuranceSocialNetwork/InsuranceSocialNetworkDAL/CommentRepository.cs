using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class CommentRepository
    {
        public static long CreateComment(PostComment comment)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                comment.Active = true;
                comment.Date = DateTime.Now;

                context.PostComment.Create(comment);
                context.Save();

                return comment.ID;
            }
        }
    }
}
