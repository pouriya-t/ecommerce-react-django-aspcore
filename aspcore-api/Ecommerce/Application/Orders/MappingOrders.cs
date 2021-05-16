using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class MappingOrders : Profile
    {
        public MappingOrders()
        {
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest._id, src => src.MapFrom(i => i._id))
                .ForMember(dest => dest.Name, src => src.MapFrom(i => i.Name))
                .ForMember(dest => dest.Qty, src => src.MapFrom(i => i.Qty))
                .ForMember(dest => dest.Price, src => src.MapFrom(i => i.Price))
                .ForMember(dest => dest.Image, src => src.MapFrom(i => i.Image))
                .ForMember(dest => dest.Product, src => src.MapFrom(i => i.product_id))
                .ForMember(dest => dest.Order, src => src.MapFrom(i => i.order_id));


            CreateMap<ShippingAddress, ShippingAddressDto>()
                .ForMember(dest => dest._id, src => src.MapFrom(i => i._id))
                .ForMember(dest => dest.Address, src => src.MapFrom(i => i.Address))
                .ForMember(dest => dest.City, src => src.MapFrom(i => i.City))
                .ForMember(dest => dest.PostalCode, src => src.MapFrom(i => i.PostalCode))
                .ForMember(dest => dest.Country, src => src.MapFrom(i => i.Country))
                .ForMember(dest => dest.ShippingPrice, src => src.MapFrom(i => i.ShippingPrice))
                .ForMember(dest => dest.Order, src => src.MapFrom(i => i.order_id));


        }
    }
}
