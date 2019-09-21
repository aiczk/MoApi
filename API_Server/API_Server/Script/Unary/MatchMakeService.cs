using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using API_Server.Script.Utility;
using MagicOnion;
using MagicOnion.Server;
using Reactive.Bindings;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class MatchMakeService : ServiceBase<IMatchMakeService>, IMatchMakeService
    {
        //部屋名 接続人数
        private static Dictionary<string, int> matches = new Dictionary<string, int>();
        private static ReactiveProperty<string> roomNameCache = new ReactiveProperty<string>("lol");
        private AsyncSubject<Unit> completeMatchMaking = new AsyncSubject<Unit>();
        private ServerStreamingContext<MatchData> stream;
        
        public UnaryResult<string> RequireMatch() => UnaryResult(GetMatch());
        public UnaryResult<bool> RegisterMatch(MatchData matchData) => 
            UnaryResult(matches.TryAdd(matchData.roomName, matchData.count));

        public async Task<ServerStreamingResult<MatchData>> NewMatch()
        { 
            stream = GetServerStreamingContext<MatchData>();

            roomNameCache
                .Subscribe(async roomName =>
                {
                    DeleteZeroOrMax();
                    await stream.WriteAsync(new MatchData {roomName = roomName, count = 1});
                    Logger.Debug(roomName);
                });
            
            await completeMatchMaking;
            Logger.Debug("DONE");

            return stream.Result();
        }

        //空きが存在していなければ作成。
        //空きが存在していれば入室。
        //4人になったら削除。
        private string GetMatch()
        {
            var name = "";
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if(count >= 4)
                    continue;
                
                name = roomName;
                break;
            }

            if (!string.IsNullOrEmpty(name)) 
                return name;
            
            name = Utils.GUID;
            roomNameCache.Value = name;
            matches.Add(name, 1);
            
            return name;
        }

        private void DeleteZeroOrMax()
        {
            foreach (var match in matches)
            {
                var (roomName, count) = match;

                if (count != 0 && count != 4)
                    continue;
                
                matches.Remove(roomName);
            }
        }
    }
}