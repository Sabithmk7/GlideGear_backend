using AutoMapper;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<User,UserRegistrationDto>().ReverseMap();
        }
    }
}
