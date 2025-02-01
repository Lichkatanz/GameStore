using System.Runtime.InteropServices;
using GameStore.Models;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameRepository _gameRepository;

        public GameController(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _gameRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return game == null ? NotFound() : Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Game game)
        {
            await _gameRepository.CreateAsync(game);
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Game game)
        {
            var existingGame = await _gameRepository.GetByIdAsync(id);
            if (existingGame == null) return NotFound();

            game.Id = id;
            await _gameRepository.UpdateAsync(id, game);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingGame = await _gameRepository.GetByIdAsync(id);
            if (existingGame == null) return NotFound();

            await _gameRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
