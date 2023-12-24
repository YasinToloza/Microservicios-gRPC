using AutoMapper;
using Discount.Spa.gRPC.Protos;
using DIscount.Spa.gRPC.Models;
using Discount.Spa.gRPC.Repositories;
using Grpc.Core;

namespace Discount.Spa.gRPC.Services
{
    public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly IMapper mapper;
        public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.discountRepository = discountRepository;

        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Discount not found to product {request.ProductName}"));
            }
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            //transformamos un CouponModel a Coupon
            var coupon = mapper.Map<Coupon>(request.Coupon);
            await discountRepository.UpdateDiscount(coupon);
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            await discountRepository.CreateDiscount(coupon);
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var result = await discountRepository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse { Success = result };
        }
    }
}
