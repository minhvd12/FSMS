using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, GetOrderDetail>()
                .ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.Fruit.FruitName))
                .ForMember(dest => dest.DiscountName, opts => opts.MapFrom(src => src.FruitDiscount.DiscountName))
                .ReverseMap();
            CreateMap<Fruit, GetOrderDetail>().ForMember(dest => dest.FruitName, opts => opts.MapFrom(src => src.FruitName))
                     .ReverseMap();
            CreateMap<FruitDiscount, GetOrderDetail>().ForMember(dest => dest.DiscountName, opts => opts.MapFrom(src => src.DiscountName))
                    .ReverseMap();

        }

    }
}
