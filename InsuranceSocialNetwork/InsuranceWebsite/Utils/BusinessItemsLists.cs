using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceSocialNetworkDTO.Banner;
using InsuranceWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceSocialNetworkDTO.Role;
using InsuranceSocialNetworkDTO.Company;

namespace InsuranceWebsite.Utils
{
    public static class BusinessItemsLists
    {
        public static List<UserProfileModelObject> GetUsers()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfileDTO, UserProfileModelObject>();
                cfg.CreateMap<UserProfileModelObject, UserProfileDTO>();

                cfg.CreateMap<UserDTO, UserModelObject>();
                cfg.CreateMap<UserModelObject, UserDTO>();

                cfg.CreateMap<RoleDTO, RoleModelObject>();
                cfg.CreateMap<RoleModelObject, RoleDTO>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            var list = InsuranceBusiness.BusinessLayer.GetUsers();
            return mapper.Map<List<UserProfileModelObject>>(list);
        }
        public static List<UserProfileModelObject> GetRoles()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfileDTO, UserProfileModelObject>();
                cfg.CreateMap<UserProfileModelObject, UserProfileDTO>();

                cfg.CreateMap<UserDTO, UserModelObject>();
                cfg.CreateMap<UserModelObject, UserDTO>();

                cfg.CreateMap<RoleDTO, RoleModelObject>();
                cfg.CreateMap<RoleModelObject, RoleDTO>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            var list = InsuranceBusiness.BusinessLayer.GetUsers();
            return mapper.Map<List<UserProfileModelObject>>(list);
        }

        public static List<BannerModelObject> GetBanners()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BannerDTO, BannerModelObject>();
                cfg.CreateMap<BannerModelObject, BannerDTO>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            var list = InsuranceBusiness.BusinessLayer.GetBanners();
            return mapper.Map<List<BannerModelObject>>(list);
        }

        public static List<CompanyModelObject> GetGarages()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyDTO, CompanyModelObject>();
                cfg.CreateMap<CompanyModelObject, CompanyDTO>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            var list = InsuranceBusiness.BusinessLayer.GetGarages();
            return mapper.Map<List<CompanyModelObject>>(list);
        }
    }
}