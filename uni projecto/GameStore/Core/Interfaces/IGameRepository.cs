using GameStore.Core.Models;

namespace GameStore.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> AddGame(Game game);
        Task<IEnumerable<Game>> GetAllGames();
    }
}