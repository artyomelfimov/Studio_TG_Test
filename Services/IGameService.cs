using StudioTG_Test.Models;

namespace StudioTG_Test.Services
{
    public interface IGameService
    {
        Dictionary<Guid, Game> GetGames();
        GameInfoResponse CreateNew(NewGameRequest request);
        Game GetById(Guid id);

    }
}
