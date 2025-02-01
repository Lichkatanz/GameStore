using GameStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameStore.Repositories
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
        }

        public async Task<List<Order>> GetAllAsync() =>
            await _orders.Find(_ => true).ToListAsync();

        public async Task<Order> GetByIdAsync(string id) =>
            await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order order) =>
            await _orders.InsertOneAsync(order);

        public async Task UpdateAsync(string id, Order order) =>
            await _orders.ReplaceOneAsync(o => o.Id == id, order);

        public async Task DeleteAsync(string id) =>
            await _orders.DeleteOneAsync(o => o.Id == id);
    }
}
