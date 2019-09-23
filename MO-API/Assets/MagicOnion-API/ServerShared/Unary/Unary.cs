using System;
using System.Threading.Tasks;
using MagicOnion;
using MessagePack;
using ServerShared.MessagePackObject;
// ReSharper disable CheckNamespace

namespace ServerShared.Unary
{
    public interface IAccessControlService : IService<IAccessControlService>
    {
        UnaryResult<PlayerIdentifier[]> GetCurrentTeamMate();
    }

    public interface IMatchMakeService : IService<IMatchMakeService>
    {
        UnaryResult<string> RequireMatch();
        UnaryResult<Nil> JoinMatch(string matchName);
        UnaryResult<Nil> LeaveMatch(string matchName);
        UnaryResult<int> MatchCount(string matchName);
    }
}
