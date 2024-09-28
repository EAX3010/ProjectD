using AutoMapper;
using Core.Models;
using Shared.DTOs;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Server to Client mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name));

            CreateMap<Category, CategoryDto>();

            // Client to Server mappings
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // Avoid mapping the entire Category object
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) // Ignore CreatedDate to prevent overwriting
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()); // Ignore UpdatedDate as it is managed by the database

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore()); // Ignore Products collection to prevent circular references
        }
    }
}
