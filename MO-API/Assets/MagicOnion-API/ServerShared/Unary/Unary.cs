using System;
using System.Threading.Tasks;
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
        UnaryResult<string> RequireMatch();
        Task<ServerStreamingResult<MatchData>> NewMatch();
        UnaryResult<bool> RegisterMatch(MatchData matchData);
    }
}
