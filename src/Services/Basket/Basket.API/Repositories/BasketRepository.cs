using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
  public class BasketRepository : IBasketRepository
  {
    private readonly IDistributedCache _RedisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
      _RedisCache = redisCache;
    }
    public async Task DeleteBasket(string userName) => await _RedisCache.RemoveAsync(userName);

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
      var basket = await _RedisCache.GetStringAsync(userName);
      if(string.IsNullOrEmpty(basket))
        return null;
      return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
      await _RedisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
      return await GetBasket(basket.UserName);
    }
  }
}
