using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products
{
    public class MappingProducts : Profile
    {
        public MappingProducts()
        {

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest._id, src => src.MapFrom(i => i._id))
                .ForMember(dest => dest.name, src => src.MapFrom(i => i.name))
                .ForMember(dest => dest.image, src => src.MapFrom(i => i.image))
                .ForMember(dest => dest.brand, src => src.MapFrom(i => i.brand))
                .ForMember(dest => dest.category, src => src.MapFrom(i => i.category))
                .ForMember(dest => dest.description, src => src.MapFrom(i => i.description))
                .ForMember(dest => dest.numReviews, src => src.MapFrom(i => i.numReviews))
                .ForMember(dest => dest.price, src => src.MapFrom(i => i.price))
                .ForMember(dest => dest.countInStock, src => src.MapFrom(i => i.countInStock))
                .ForMember(dest => dest.createdAt, src => src.MapFrom(i => i.createdAt))
                .ForMember(dest => dest.user, src => src.MapFrom(i => i.user_id));
        }

    }
}
