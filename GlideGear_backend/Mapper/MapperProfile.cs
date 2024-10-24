using AutoMapper;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.CategoryDtos;
using GlideGear_backend.Models.Dtos.ProductDtos;
using GlideGear_backend.Models.Dtos.UserDtos;

namespace GlideGear_backend.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<User,UserRegistrationDto>().ReverseMap();
            CreateMap<User, UserViewDto>().ReverseMap();
            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<ProductDto,Product>().ReverseMap();
        }
    }
}
