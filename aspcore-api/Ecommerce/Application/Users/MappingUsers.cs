using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public class MappingUsers : Profile
    {
        public MappingUsers()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest._id , src => src.MapFrom(i => i.Id))
                .ForMember(dest => dest.Id , src => src.MapFrom(i => i.Id))
                .ForMember(dest => dest.Username, src => src.MapFrom(i => i.UserName))
                .ForMember(dest => dest.Email, src => src.MapFrom(i => i.Email))
                .ForMember(dest => dest.Name, src => src.MapFrom(i => i.DisplayName));
        }
    }
}
