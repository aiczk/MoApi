using MagicOnion;
using ServerShared.MessagePackObject;
// ReSharper disable CheckNamespace

namespace ServerShared.Unary
{
    public interface IRoomService : IService<IRoomService>
    {
        UnaryResult<PlayerIdentifier[]> GetCurrentRoomMate();
    }

    public interface IMatchMakeService : IService<IMatchMakeService>
    {
        UnaryResult<string> RequireMatchName();
    }

    public interface IWorldService : IService<IWorldService>
    {
        UnaryResult<bool> RegisterWorld(PlayerIdentifier playerIdentifier);
    }
}
