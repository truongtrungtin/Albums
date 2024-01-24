using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;
        private const string BasketKeyPrefix = "Basket";


        public BasketRepository(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
            _database = _redisConnection.GetDatabase();

        }

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            var basketData = await _database.StringGetAsync(GetBasketKey(basketId));

            return basketData.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<Basket>(json: basketData);
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        { 
            var serializedBasket = JsonSerializer.Serialize(basket);

            // Set the basket data in Redis
            await _database.StringSetAsync(GetBasketKey(basket.Id), serializedBasket);

            return basket;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            // Delete the basket from Redis
            return await _database.KeyDeleteAsync(GetBasketKey(basketId));
        }

        private string GetBasketKey(string basketId) => $"{BasketKeyPrefix}:{basketId}";
    }
}
