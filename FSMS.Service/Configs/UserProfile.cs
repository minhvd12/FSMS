using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUser>().ForMember(dept => dept.RoleName, opts => opts.MapFrom(src => src.Role.RoleName)).ReverseMap();
            CreateMap<User, SignInAccount>();
        }
    }
}
