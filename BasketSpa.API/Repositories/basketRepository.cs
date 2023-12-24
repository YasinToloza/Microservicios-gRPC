using BasketSpa.API.grpServices;
using BasketSpa.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketSpa.API.Repositories
{
    public class basketRepository : IBasketRepository
    {
        private readonly IDistributedCache redis;
        private readonly DiscountgRPCService discountgRPCService;
        public basketRepository(IDistributedCache redis, DiscountgRPCService discountgRPCService) 
        {
            this.redis = redis;
            this.discountgRPCService = discountgRPCService;
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await redis.GetStringAsync(userName);
            if (String.IsNullOrEmpty(userName))
                return null;
            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            // Por cada producto verificar si hay un cupon de descuento
            foreach (var item in shoppingCart.Items)
            {
                //Llamamos al gRPC Discount
               var coupon = await discountgRPCService.GetDiscount(item.ProductName);
                item.ProductDiscount = coupon.Amount;
            }
            await redis.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart));
            return await GetBasket(shoppingCart.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await redis.RemoveAsync(userName);
        }
    }
}
