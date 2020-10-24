using AutoMapper;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Models.CategoryModels;
using ServerApp.Models.RecordModels;
using ServerApp.Models.UserModels;
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
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>()               
                .ForMember(dest => dest.Id, act => act.Ignore());
            CreateMap<Record, RecordModel>();
            CreateMap<CreateRecordModel, Record>();
            CreateMap<UpdateRecordModel, Record>()
                .ForMember(dest => dest.Id, act => act.Ignore());


        }
    }
}
