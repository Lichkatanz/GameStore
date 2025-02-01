using GameStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Core.Interfaces
{
    public interface IGameService
    {
        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="gameDto">The game data transfer object.</param>
        /// <returns>The added game.</returns>
        Task<Game> AddGame(GameDto gameDto);

        /// <summary>
        /// Retrieves all games from the database.
        /// </summary>
        /// <returns>A list of all games.</returns>
        Task<IEnumerable<Game>> GetAllGames();
    }
}