﻿using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class BannerRepository
    {
        public static Banner GetBanner(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return GetBanner(context, id);
            }
        }

        public static Banner GetBanner(BackofficeUnitOfWork context, long id)
        {
            return context.Banner
                .Fetch()
                .Include(i => i.BannerType)
                .Where(i => i.Active)
                .FirstOrDefault(i => i.ID == id);
        }

        public static List<Banner> GetBanners()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.Banner
                    .Fetch()
                    .Include(i => i.BannerType)
                    .Where(i => i.Active)
                    .Select(i => i)
                    .ToList();
            }
        }

        public static List<BannerType> GetBannerTypes()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.BannerType.Fetch().Select(i => i).ToList();
            }
        }

        public static bool CreateBanner(Banner banner)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                banner.Active = true;
                banner.CreateDate = banner.LastChangeDate = DateTime.Now;

                context.Banner.Create(banner);
                context.Save();

                return banner.ID > 0;
            }
        }

        public static bool EditBanner(Banner banner)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Banner item = context.Banner.Get(banner.ID);
                item.LastChangeDate = DateTime.Now;
                item.Description = banner.Description;
                item.ID_Banner_Type = banner.ID_Banner_Type;
                item.StartDate = banner.StartDate;
                item.DueDate = banner.DueDate;
                item.Image = null != banner.Image ? banner.Image : item.Image;

                context.Banner.Update(item);
                context.Save();

                return true;
            }
        }

        public static bool DeleteBanner(long id)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                Banner item = context.Banner.Get(id);
                item.DeleteDate = DateTime.Now;
                item.Active = false;

                context.Banner.Update(item);
                context.Save();

                return true;
            }
        }
    }
}