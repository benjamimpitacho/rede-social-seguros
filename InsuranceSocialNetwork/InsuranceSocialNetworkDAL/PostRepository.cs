﻿using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InsuranceSocialNetworkDAL
{
    public class PostRepository
    {
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

        public static bool CreatePost(Post post)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                post.Active = true;
                post.CreateDate = DateTime.Now;

                using (var transaction = new TransactionScope())
                {
                    context.Post.Create(post);
                    context.Save();

                    if(null != post.PostImage && post.PostImage.Count > 0)
                    {
                        
                    }

                    transaction.Complete();
                }

                return post.ID > 0;
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
