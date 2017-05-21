using AutoMapper;
using InsuranceSocialNetworkBusiness;
using InsuranceSocialNetworkDTO.UserProfile;
using InsuranceWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            });

            var mapper = mapperConfiguration.CreateMapper();

            var list = InsuranceBusiness.BusinessLayer.GetUsers();
            return mapper.Map<List<UserProfileModelObject>>(list);
        }
    }
}