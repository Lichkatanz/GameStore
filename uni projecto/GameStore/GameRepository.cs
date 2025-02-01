using GameStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameStore.Repositories
{
    public class GameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _games = database.GetCollection<Game>(settings.Value.GameCollection);
        }

        public async Task<List<Game>> GetAllAsync() =>
            await _games.Find(_ => true).ToListAsync();

        public async Task<Game> GetByIdAsync(string id) =>
            await _games.Find(g => g.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Game game) =>
            await _games.InsertOneAsync(game);

        public async Task UpdateAsync(string id, Game game) =>
            await _games.ReplaceOneAsync(g => g.Id == id, game);

        public async Task DeleteAsync(string id) =>
            await _games.DeleteOneAsync(g => g.Id == id);
    }
}
