using StudioTG_Test.Models;

namespace StudioTG_Test.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<Guid,Game> _games = [];
        public  GameInfoResponse CreateNew(NewGameRequest requestedparams)
        {
            Game game = new Game(requestedparams);
            _games.Add(game.Game_id, game);
            return new GameInfoResponse { GameID = game.Game_id,Completed = game.Completed,Field  = game.Field};
        }

        public Game GetById(Guid id)
        {
            return _games.GetValueOrDefault(id) ?? throw new Exception($"нет игры с идентификатором {id}");
        }

        public Dictionary<Guid, Game> GetGames()
        {
            return _games;
        }

        public Game Turn(GameTurnRequest turn)
        {
            Game game = GetById(turn.game_id);
            return(game.GameTurn(turn));
        }
    }
}
