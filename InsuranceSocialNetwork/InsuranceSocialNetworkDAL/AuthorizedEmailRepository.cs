using InsuranceSocialNetworkCore.Enums;
using InsuranceSocialNetworkDTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDAL
{
    public class AuthorizedEmailRepository
    {
        public static List<AuthorizedEmail> GetAuthorizedEmails(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return context.AuthorizedEmail
                        .Fetch()
                        .Where(i => string.IsNullOrEmpty(i.ID_User))
                        .Select(i => i)
                        .ToList();
                }
                else
                {
                    return context.AuthorizedEmail
                        .Fetch()
                        .Where(i => i.ID_User == userId)
                        .Select(i => i)
                        .ToList();
                }
            }
        }

        public static bool IsEmailAuthorizedForAutomaticApproval(string email)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                List<string> authorizedEmails = context.AuthorizedEmail.Fetch().Where(i => i.ID_User != null).Select(i => i.Email).ToList();

                foreach (string item in authorizedEmails)
                {
                    if (email.ToLower().Equals(item.ToLower()))
                        return true;
                }

                authorizedEmails = context.AuthorizedEmail.Fetch().Where(i => i.ID_User == null).Select(i => i.Email).ToList();

                foreach (string item in authorizedEmails)
                {
                    if (email.ToLower().EndsWith(item.ToLower()))
                        return true;
                }

                return false;
            }
        }

        public static bool UpdateEmailAuthorizedForAutomaticApproval(string[] emailPatterns)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                context.AuthorizedEmail.Delete(i => string.IsNullOrEmpty(i.ID_User));

                foreach (string emailPattern in emailPatterns)
                {
                    context.AuthorizedEmail.Create(new AuthorizedEmail() { Active = true, Email = emailPattern.ToLower(), CreateDate = DateTime.Now, LastChangeDate = DateTime.Now });
                }

                context.Save();

                return true;
            }
        }

        public static bool UpdateEmailAuthorizedForAutomaticApproval(string userId, string[] emailPatterns)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                context.AuthorizedEmail.Delete(i => string.IsNullOrEmpty(i.ID_User));

                foreach (string emailPattern in emailPatterns)
                {
                    context.AuthorizedEmail.Create(new AuthorizedEmail() {
                        ID_User = userId,
                        Active = true,
                        Email = emailPattern.ToLower(),
                        CreateDate = DateTime.Now,
                        LastChangeDate = DateTime.Now
                    });
                }

                context.Save();

                return true;
            }
        }

        public static List<AuthorizedEmail> GetAuthorizedEmailsForAutomaticApproval()
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.AuthorizedEmail
                    .Fetch()
                    .Where(i => i.Active)
                    .Select(i => i)
                    .ToList();
            }
        }

        public static List<AuthorizedEmail> GetAuthorizedEmailsForAutomaticApproval(string userId)
        {
            using (var context = new BackofficeUnitOfWork())
            {
                return context.AuthorizedEmail
                    .Fetch()
                    .Where(i => i.Active && i.ID_User == userId)
                    .Select(i => i)
                    .ToList();
            }
        }

        //public static List<Banner> GetActiveBanners(BannerTypeEnum bannerType)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        return context.Banner
        //            .Fetch()
        //            .Include(i => i.BannerType)
        //            .Where(i => i.Active
        //                && i.BannerType.Token == bannerType.ToString()
        //                && i.StartDate < DateTime.Now
        //                && (!i.DueDate.HasValue || i.DueDate.Value > DateTime.Now))
        //            .Select(i => i)
        //            .OrderBy(i => Guid.NewGuid())
        //            .ToList();
        //    }
        //}

        //public static List<BannerType> GetBannerTypes()
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        return context.BannerType.Fetch().Select(i => i).ToList();
        //    }
        //}

        //public static bool CreateBanner(Banner banner)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        banner.Active = true;
        //        banner.CreateDate = banner.LastChangeDate = DateTime.Now;

        //        context.Banner.Create(banner);
        //        context.Save();

        //        return banner.ID > 0;
        //    }
        //}

        //public static bool EditBanner(Banner banner)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        Banner item = context.Banner.Get(banner.ID);
        //        item.LastChangeDate = DateTime.Now;
        //        item.Description = banner.Description;
        //        item.ID_Banner_Type = banner.ID_Banner_Type;
        //        item.StartDate = banner.StartDate;
        //        item.DueDate = banner.DueDate;
        //        item.Image = null != banner.Image ? banner.Image : item.Image;

        //        context.Banner.Update(item);
        //        context.Save();

        //        return true;
        //    }
        //}

        //public static bool DeleteBanner(long id)
        //{
        //    using (var context = new BackofficeUnitOfWork())
        //    {
        //        Banner item = context.Banner.Get(id);
        //        item.DeleteDate = DateTime.Now;
        //        item.Active = false;

        //        context.Banner.Update(item);
        //        context.Save();

        //        return true;
        //    }
        //}
    }
}
