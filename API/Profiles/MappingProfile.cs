using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Infrastructure.Data.ViewModel;

namespace API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name)).ReverseMap();

        CreateMap<ProductTypeCreateDTO, ProductType>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();

        //CreateMap<ProductModel, ProductViewModel>()
        //    .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
        //CreateMap<List<ProductModel>, List<ProductViewModel>>().ReverseMap();

        CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)).ReverseMap();

        CreateMap<Address, AddressDTO>()
            .ForMember(dest => dest.Fname, opt => opt.MapFrom(src => src.Fname))
            .ForMember(dest => dest.Lname, opt => opt.MapFrom(src => src.Lname))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
            .ReverseMap();
    }
}