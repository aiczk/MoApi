using System;
using System.Threading.Tasks;
using MagicOnion;
using MessagePack;
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
        UnaryResult<bool> RegisterMatch(MatchData matchData);
        
        Task<ServerStreamingResult<MatchData>> NewMatch();
        Task<ServerStreamingResult<MatchData>> UpdateMatch();

        UnaryResult<Nil> JoinMatch(string matchName);
        UnaryResult<Nil> LeaveMatch(string matchName);
    }
    
    //入退室は鯖全体に広める必要がある。
    //入室 :Unary(ローカル) => Server(グローバル)
    //退室 :Unary(ローカル) => Server(グローバル)
    //                   ||
    //更新 :Unary(ローカル) => Server(グローバル)
}
