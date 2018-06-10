using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkCore.Types;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace InsuranceSocialNetworkDAL
{
    public class PostRepository
    {
        //public static List<Post> GetUserPosts(string Id)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        List<long> hiddenPostIds = context.PostHidden
        //        .Fetch()
        //        .Where(i => i.ID_User == Id && i.Hidden)
        //        .Select(i => i.ID_Post)
        //        .ToList();

        //        return context.Post
        //            .Fetch()
        //            .Include(i => i.AspNetUsers.Profile)
        //            .Include(i => i.PostType)
        //            .Include(i => i.PostSubject)
        //            .Include(i => i.PostLike)
        //            .Include(i => i.PostComment)
        //            .Include(i => i.PostImage)
        //            .Include(i => i.PostHidden)
        //            .Where(i => i.ID_User == Id
        //                && i.Active
        //                && !hiddenPostIds.Contains(i.ID)
        //                && (
        //                    i.PostSubject.Token.Equals("PERSONAL_POST")
        //                    || i.PostSubject.Token.Equals("BUSINESS_POST")
        //                    || i.PostSubject.Token.Equals("NEWS_POST")
        //                    || i.PostSubject.Token.Equals("PARTNERSHIP_POST")
        //                    || i.PostSubject.Token.Equals("WALLET_POST")
        //                    || i.PostSubject.Token.Equals("SPONSORED_POST")
        //                    || i.PostSubject.Token.Equals("GLOBAL_POST")
        //                ))
        //            .OrderByDescending(i => i.Sticky)
        //            .ThenByDescending(i => i.CreateDate)
        //            .ToList();
        //    }
        //}

        public static List<Post> GetUserRelatedPosts(BackofficeUnitOfWork context, string Id, int skipInterval = 0, int itemsCount = 10)
        {
            List<string> friendsUserIds = context
                .Friend
                .Fetch()
                .Include(i=>i.FriendStatus)
                .Where(i => i.ID_User == Id && i.FriendStatus.Token == FriendStatusEnum.REQUEST_ACCEPTED.ToString())
                .Select(i => i.ID_User_Friend)
                .ToList();
            List<string> auxFriendsUserIds = context
                .Friend
                .Fetch()
                .Include(i => i.FriendStatus)
                .Where(i => i.ID_User_Friend == Id && i.FriendStatus.Token == FriendStatusEnum.REQUEST_ACCEPTED.ToString())
                .Select(i => i.ID_User)
                .ToList();
            friendsUserIds = friendsUserIds.Concat(auxFriendsUserIds).ToList();

            List<long> hiddenPostIds = context.PostHidden
                .Fetch()
                .Where(i => i.ID_User == Id && i.Hidden)
                .Select(i => i.ID_Post)
                .ToList();

            List<Post> postList = context.Post
                .Fetch()
                .Where(i => (
                        i.ID_User == Id
                        || (friendsUserIds.Contains(i.ID_User) && i.PostSubject.Token.Equals("PERSONAL_POST"))
                        || i.PostSubject.Token.Equals("BUSINESS_POST")
                        || i.PostSubject.Token.Equals("NEWS_POST")
                        || i.PostSubject.Token.Equals("PARTNERSHIP_POST")
                        || i.PostSubject.Token.Equals("WALLET_POST")
                        || i.PostSubject.Token.Equals("SPONSORED_POST")
                        || i.PostSubject.Token.Equals("GLOBAL_POST")
                    )
                    && i.Active
                    && !hiddenPostIds.Contains(i.ID)
                )
                .Include(i => i.Post1)
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                .Include(i => i.PostHidden)
                .Include(i => i.PostImage)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                //.Include(i => i.PostImage)
                .OrderByDescending(i => i.Sticky)
                .ThenByDescending(i => i.CreateDate)
                //.Skip(skipInterval)
                //.Take(itemsCount)
                .ToList();

            //foreach (Post post in postList)
            //{
            //    post.PostComment = post.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList();
            //    post.PostImage = post.PostImage.Where(c => c.Active).Select(c => c).ToList();
            //}

            //var cmd = context.DataContext.Database.Connection.CreateCommand();
            //cmd.CommandText = "[dbo].[GetAllPosts]";
            

            //try
            //{

            //    context.DataContext.Database.Connection.Open();
            //    // Run the sproc 
            //    var reader = cmd.ExecuteReader();

            //    // Read Blogs from the first result set
            //    var blogs = ((IObjectContextAdapter)context.DataContext)
            //        .ObjectContext
            //        .Translate<Post>(reader, "Posts", MergeOption.AppendOnly);


            //    // Move to second result set and read Posts
            //    //reader.NextResult();
            //    //var posts = ((IObjectContextAdapter)db)
            //    //    .ObjectContext
            //    //    .Translate<Post>(reader, "Posts", MergeOption.AppendOnly);
                
            //}
            //finally
            //{
            //    context.DataContext.Database.Connection.Close();
            //}

            return postList;
        }

        public static List<Post> GetPostsBySubject(BackofficeUnitOfWork context, PostSubjectEnum postSubject)
        {
            List<Post> postList = context.Post
                .Fetch()
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                .Include(i => i.PostImage)
                .Where(i => i.PostSubject.Token == postSubject.ToString()
                    && i.Active)
                .OrderByDescending(i => i.Sticky)
                .ThenByDescending(i => i.CreateDate)
                .ToList();

            postList.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList());
            postList.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());

            return postList;
        }

        //public static List<Post> GetUserPostsOnly(BackofficeUnitOfWork context, string Id)
        //{
        //    List<long> hiddenPostIds = context.PostHidden
        //        .Fetch()
        //        .Where(i => i.ID_User == Id && i.Hidden)
        //        .Select(i => i.ID_Post)
        //        .ToList();

        //    List<Post> postList = context.Post
        //        .Fetch()
        //        .Include(i => i.AspNetUsers.Profile)
        //        .Include(i => i.PostType)
        //        .Include(i => i.PostSubject)
        //        .Include(i => i.PostLike)
        //        .Include(i => i.PostComment)
        //        .Include(i => i.PostHidden)
        //        //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
        //        .Include(i => i.PostImage)
        //        .Where(i => i.ID_User == Id
        //            && i.Active
        //            && !hiddenPostIds.Contains(i.ID)
        //            && (
        //                i.PostSubject.Token.Equals("PERSONAL_POST")
        //                || i.PostSubject.Token.Equals("BUSINESS_POST")
        //                || i.PostSubject.Token.Equals("NEWS_POST")
        //                || i.PostSubject.Token.Equals("PARTNERSHIP_POST")
        //                || i.PostSubject.Token.Equals("WALLET_POST")
        //                || i.PostSubject.Token.Equals("SPONSORED_POST")
        //                || i.PostSubject.Token.Equals("GLOBAL_POST")
        //            ))
        //        .OrderByDescending(i => i.Sticky)
        //        .ThenByDescending(i => i.CreateDate)
        //        .ToList();

        //    postList.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList());
        //    postList.ForEach(p => p.PostLike = p.PostLike.Where(c => c.Active).Select(c => c).ToList());
        //    postList.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());

        //    return postList;
        //}

        public static List<Post> GetPosts(BackofficeUnitOfWork context, string Id, PostSubjectEnum postSubject)
        {
            List<long> hiddenPostIds = context.PostHidden
                .Fetch()
                .Where(i => i.ID_User == Id && i.Hidden)
                .Select(i => i.ID_Post)
                .ToList();

            List<Post> postList = context.Post
                .Fetch()
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                .Include(i => i.PostHidden)
                .Include(i => i.PostImage)
                .Include(i => i.District)
                .Include(i => i.County)
                .Where(i => i.PostSubject.Token.Equals(postSubject.ToString())
                    && i.Active
                    && !hiddenPostIds.Contains(i.ID)
                )
                .OrderByDescending(i => i.Sticky)
                .ThenByDescending(i => i.CreateDate)
                .ToList();

            postList.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList());
            postList.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());

            return postList;
        }

        public static List<Post> SearchPosts(BackofficeUnitOfWork context, string Id, string searchTerm, PostSubjectEnum postSubject)
        {
            List<long> hiddenPostIds = context.PostHidden
                .Fetch()
                .Where(i => i.ID_User == Id && i.Hidden)
                .Select(i => i.ID_Post)
                .ToList();

            List<Post> postList = context.Post
                .Fetch()
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                .Include(i => i.PostHidden)
                .Include(i => i.PostImage)
                .Where(i => i.PostSubject.Token.Equals(postSubject.ToString())
                    && i.Active
                    && !hiddenPostIds.Contains(i.ID)
                    && (
                        i.Title.ToLower().Contains(searchTerm)
                        || i.Text.ToLower().Contains(searchTerm)
                        || i.URL.ToLower().Contains(searchTerm)
                    )
                )
                .OrderByDescending(i => i.Sticky)
                .ThenByDescending(i => i.CreateDate)
                .ToList();

            postList.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList());
            postList.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());

            return postList;
        }

        public static Post GetPost(BackofficeUnitOfWork context, long postId)
        {
            Post post = context.Post
                .Fetch()
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                .Include(i => i.PostImage)
                .Where(i => i.ID == postId)
                .FirstOrDefault();

            if (null != post)
            {
                post.PostComment = post.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList();
                post.PostImage = post.PostImage.Where(c => c.Active).Select(c => c).ToList();
                post.PostLike = post.PostLike.Where(c => c.Active).Select(c => c).ToList();
            }

            return post;
        }

        public static long CreatePost(Post post)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                post.Active = true;
                post.CreateDate = DateTime.Now;

                context.Post.Create(post);
                context.Save();

                //using (var transaction = new TransactionScope())
                //{
                //    context.Post.Create(post);
                //    context.Save();

                //    if(null != post.PostImage && post.PostImage.Count > 0)
                //    {

                //    }

                //    transaction.Complete();
                //}

                return post.ID;
            }
        }

        public static long EditPost(Post post)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                context.PostImage.Delete(i => i.ID_Post == post.ID);

                if (null != post.PostImage && post.PostImage.Count > 0)
                {
                    context.PostImage.Create(post.PostImage.ElementAt(0));
                }

                context.Post.Update(post);
                context.Save();

                return post.ID;
            }
        }

        public static bool DeletePost(long postId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Post post = context.Post
                    .Fetch()
                    .Where(i => i.ID == postId && i.ID_User == userId)
                    .FirstOrDefault();

                if (null == post)
                    return false;

                context.PostComment.Delete(i => i.ID_Post == post.ID);
                context.PostHidden.Delete(i => i.ID_Post == post.ID);
                context.PostImage.Delete(i => i.ID_Post == post.ID);
                context.PostLike.Delete(i => i.ID_Post == post.ID);
                context.Notification.Delete(i => i.ID_Post == post.ID);

                context.Post.Delete(post.ID);
                context.Save();

                return true;
            }
        }

        public static bool DeletePostAsAdmin(long postId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Post post = context.Post
                    .Fetch()
                    .Where(i => i.ID == postId)
                    .FirstOrDefault();

                if (null == post)
                    return false;

                context.PostComment.Delete(i => i.ID_Post == post.ID);
                context.PostHidden.Delete(i => i.ID_Post == post.ID);
                context.PostImage.Delete(i => i.ID_Post == post.ID);
                context.PostLike.Delete(i => i.ID_Post == post.ID);
                context.Notification.Delete(i => i.ID_Post == post.ID);

                context.Post.Delete(post.ID);
                context.Save();

                return true;
            }
        }

        public static bool HidePost(long postId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                PostHidden hidePost = new PostHidden()
                {
                    Date=DateTime.Now,
                    Hidden = true,
                    ID_Post = postId,
                    ID_User = userId
                };
                
                context.PostHidden.Create(hidePost);
                context.Save();

                return true;
            }
        }

        public static bool ReportPost(long postId, string userId, string reason)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                PostReported reportPost = new PostReported()
                {
                    Date = DateTime.Now,
                    Reported = true,
                    Reason = reason,
                    ID_Post = postId,
                    ID_User = userId
                };

                context.PostReported.Create(reportPost);
                context.Save();

                return true;
            }
        }

        public static string GetPostOwnerUserId(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Post.Get(id).ID_User;
            }
        }

        public static PostType GetPostType(string token)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PostType.Fetch().FirstOrDefault(i => i.Token == token);
            }
        }

        public static PostSubject GetPostSubject(string token)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PostSubject.Fetch().FirstOrDefault(i => i.Token == token);
            }
        }

        public static bool LikePost(long postId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if(context.PostLike.Fetch().Count(i => i.ID_Post == postId && i.ID_User == userId)>0)
                {
                    return false;
                }

                PostLike like = new PostLike()
                {
                    Active = true,
                    AspNetUsers = context.AspNetUsers.Get(userId),
                    Date = DateTime.Now,
                    Post = context.Post.Get(postId)
                };

                context.PostLike.Create(like);
                context.Save();

                return like.ID > 0;
            }
        }

        public static bool UnlikePost(long postId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                PostLike like = context.PostLike.First(i => i.ID_Post == postId && i.ID_User == userId);

                if (null == like)
                    return false;

                context.PostLike.Delete(like.ID);
                context.Save();

                return true;
            }
        }

        public static bool IsOwnPost(long postId, string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Post post = context.Post.First(i => i.ID == postId);

                if (null == post)
                    return false;

                return post.ID_User == userId;
            }
        }

        public static int GetTotalLikes(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                var posts = context.Post
                    .Fetch()
                    .Include(i => i.PostLike)
                    .Where(i => i.ID_User == userId);

                if (posts == null || posts.Count() == 0)
                    return 0;

                return context.Post
                    .Fetch()
                    .Include(i => i.PostLike)
                    .Where(i => i.ID_User == userId)
                    .Sum(i => i.PostLike.Count);
            }
        }

        public static List<ListItem> GetSubjectTypeList()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.PostSubject
                    .Fetch()
                    .Select(i => new ListItem() { Key = i.ID, Value = i.Token })
                    .ToList();
            }
        }
    }
}
