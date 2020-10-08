using AutoMapper;
using ServerApp.Data.Models;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>().ForMember(dest => dest.Bill, act => act.Ignore());
        }
    }
}
