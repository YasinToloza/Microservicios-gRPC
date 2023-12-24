using AutoMapper;
using Discount.Spa.gRPC.Protos;
using DIscount.Spa.gRPC.Models;

namespace Discount.Spa.gRPC.Mappers
{
    public class CouponProfile:Profile
    {
        public CouponProfile() 
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
