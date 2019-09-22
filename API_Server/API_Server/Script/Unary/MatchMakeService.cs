using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using API_Server.Script.Utility;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Reactive.Bindings;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class MatchMakeService : ServiceBase<IMatchMakeService>, IMatchMakeService
    {
        //部屋名 接続人数
        private static Dictionary<string, int> matches = new Dictionary<string, int>();
        private static Subject<string> newMatchSubject = new Subject<string>();
        private static Subject<Unit> updateMatchSubject = new Subject<Unit>();
        private static MatchData matchCache = new MatchData();
        
        private AsyncSubject<Unit> completeMatchMaking = new AsyncSubject<Unit>();
        private ServerStreamingContext<MatchData> stream;
        
        
        public UnaryResult<Nil> JoinMatch(string matchName)
        {
            matchCache.roomName = matchName;
            matchCache.count = matches[matchName]++;
            
            Logger.Debug(matchCache.count.ToString());
            
            updateMatchSubject.OnNext(default);
            return UnaryResult(Nil.Default);
        }

        public  UnaryResult<Nil> LeaveMatch()
        {
            matchCache.count = --matches[matchCache.roomName];
            
            Logger.Debug(matchCache.count.ToString());

            updateMatchSubject.OnNext(default);
            return UnaryResult(Nil.Default);
        }

        
        public async Task<ServerStreamingResult<MatchData>> NewMatch()
        {
            stream = GetServerStreamingContext<MatchData>();

            newMatchSubject
                .Subscribe(async newMatchName =>
                {
                    RemoveZeroOrMax();
                    await stream.WriteAsync(new MatchData(newMatchName, 1));
                });
            
            await completeMatchMaking;
            return stream.Result();
        }

        public async Task<ServerStreamingResult<MatchData>> UpdateMatch()
        {
            stream = GetServerStreamingContext<MatchData>();
            
            updateMatchSubject
                .Subscribe(async _ => await stream.WriteAsync(matchCache));
            
            await completeMatchMaking;
            return stream.Result();
        }
        
        public UnaryResult<string> RequireMatch() => UnaryResult(GetMatch());
        
        public UnaryResult<bool> RegisterMatch(MatchData matchData) => 
            UnaryResult(matches.TryAdd(matchData.roomName, matchData.count));

        #region Utils
        
        private string GetMatch()
        {
            string matchName = null;
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if(count >= 4)
                    continue;
                
                matchName = roomName;
                break;
            }
            
            if (!string.IsNullOrEmpty(matchName)) 
                return matchName;
            
            matchName = Utils.GUID;
            newMatchSubject.OnNext(matchName);
            matches.Add(matchName, 1);
            
            return matchName;
        }

        private static void RemoveZeroOrMax()
        {
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if (count != 0 && count != 4)
                    continue;
                
                matches.Remove(roomName);
            }
        }
        
        #endregion
    }
}