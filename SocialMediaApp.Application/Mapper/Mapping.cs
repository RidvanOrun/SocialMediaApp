using AutoMapper;
using SocialMediaApp.Application.Model.DTOs;
using SocialMediaApp.DomainLayer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Mapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, RegisterDTO>().ReverseMap();
            CreateMap<AppUser, LoginDTO>().ReverseMap();
            CreateMap<AppUser, EditProfileDTO>().ReverseMap();
            CreateMap<AppUser, ProfileSummaryDTO>().ReverseMap();

            CreateMap<Follow, FollowDTO>().ReverseMap();
            CreateMap<Like, LikeDTO>().ReverseMap();

            CreateMap<Mention, AddMentionDTO>().ReverseMap();
        }


    }
}
