using BasketSpa.API.Protos;
using static BasketSpa.API.Protos.DiscountProtoService;

namespace BasketSpa.API.grpServices
{
    public class DiscountgRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountgRPCClient;
        public DiscountgRPCService(DiscountProtoServiceClient discountgRPCClient) 
        {
            this.discountgRPCClient = discountgRPCClient;
        }

        public async Task<CouponModel> GetDiscount(string productName) 
        {
            GetDiscountRequest request = new GetDiscountRequest { ProductName = productName };
            return await discountgRPCClient.GetDiscountAsync(request);
        }
    }
}
