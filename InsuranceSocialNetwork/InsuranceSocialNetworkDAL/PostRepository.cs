﻿using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceSocialNetworkCore.Enums;

namespace InsuranceSocialNetworkDAL
{
    public class PostRepository
    {
        public static Post GetPost(long Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Post
                    .Fetch()
                    .Include(i => i.AspNetUsers.Profile)
                    .Include(i => i.PostType)
                    .Include(i => i.PostSubject)
                    .Include(i => i.PostLike)
                    .Include(i => i.PostComment)
                    .Include(i => i.PostImage)
                    .Where(i => i.ID == Id && i.Active)
                    .FirstOrDefault();
            }
        }

        public static List<Post> GetUserPosts(string Id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Post
                    .Fetch()
                    .Include(i => i.AspNetUsers.Profile)
                    .Include(i => i.PostType)
                    .Include(i => i.PostSubject)
                    .Include(i => i.PostLike)
                    .Include(i => i.PostComment)
                    .Include(i => i.PostImage)
                    .Where(i => i.ID_User == Id && i.Active)
                    .OrderByDescending(i => i.Sticky)
                    .ThenByDescending(i => i.CreateDate)
                    .ToList();
            }
        }

        public static List<Post> GetUserRelatedPosts(BackofficeUnitOfWork context, string Id)
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

            List<Post> postList = context.Post
                .Fetch()
                .Include(i => i.AspNetUsers.Profile)
                .Include(i => i.PostType)
                .Include(i => i.PostSubject)
                .Include(i => i.PostLike)
                .Include(i => i.PostComment)
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
                .Include(i => i.PostImage)
                .Where(i => (
                    i.ID_User == Id
                    || friendsUserIds.Contains(i.ID_User)
                    )
                    && i.Active
                    && 
                        (i.PostSubject.Token == PostSubjectEnum.BUSINESS_POST.ToString()
                        || i.PostSubject.Token == PostSubjectEnum.NEWS_POST.ToString()
                        || i.PostSubject.Token == PostSubjectEnum.PARTNERSHIP_POST.ToString()
                        || i.PostSubject.Token == PostSubjectEnum.PERSONAL_POST.ToString()
                        || i.PostSubject.Token == PostSubjectEnum.SPONSORED_POST.ToString()
                        || i.PostSubject.Token == PostSubjectEnum.WALLET_POST.ToString()
                        )
                    )
                .OrderByDescending(i => i.Sticky)
                .ThenByDescending(i => i.CreateDate)
                .ToList();

            postList.ForEach(p => p.PostComment = p.PostComment.Where(c => c.Active).Select(c => c).OrderBy(i => i.Date).ToList());
            postList.ForEach(p => p.PostImage = p.PostImage.Where(c => c.Active).Select(c => c).ToList());

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
                //.Include(i => i.ChatMessage.OrderByDescending(j => j.CreateDate).Take(20))
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

        public static bool CreatePost(Post post)
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

                return post.ID > 0;
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
    }
}
