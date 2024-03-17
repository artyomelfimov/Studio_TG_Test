using Microsoft.AspNetCore.Mvc;
using StudioTG_Test.Models;
using StudioTG_Test.Services;

namespace StudioTG_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MineSweeperController : ControllerBase
    {
            private readonly IGameService GameService;
            public MineSweeperController(IGameService game)
            {
                GameService = game;
            }
            [HttpPost]
            [Route("new")]
            public async Task<ActionResult<GameInfoResponse>> NewGame([FromBody]NewGameRequest newGame)
            {
                try
                {
                    if (newGame.Width <= 0 || newGame.Height <= 0)
                    return BadRequest(new ErrorResponse { Error = "Попытка создать поле с 0 ячеек!" });
                else if (newGame.MinesCount > newGame.Width * newGame.Height - 1)
                    return BadRequest(new ErrorResponse { Error = "Количество мин превышает количество ячеек!" });
                else if (newGame.Width > 30 || newGame.Height > 30)
                    return BadRequest(new ErrorResponse { Error = "Превышен размер поля" });
                    GameInfoResponse game =  GameService.CreateNew(newGame);
                    return Ok(game);
                }
                catch
                {
                return BadRequest(new ErrorResponse {Error =  "Неизвестная ошибка" });
                }
            }
            [HttpPost]
            [Route("turn")]
            public async Task<ActionResult<GameInfoResponse>> Turn([FromBody]GameTurnRequest turn)
            {
                try
                {
                    var game = GameService.GetById(turn.game_id);
                        game.GameTurn(turn);
                        if (game.Completed)
                        {
                            await Task.Delay(TimeSpan.FromMinutes(10))
                                .ContinueWith(_ => GameService.GetGames().Remove(game.Game_id));
                        }
                        return Ok(game);
                    
                }
                catch
                {
                    return BadRequest(new ErrorResponse { Error = "Неизвестная ошибка" });
            }
            }
        }
}
