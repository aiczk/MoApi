using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using _Server.Script.Utility;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Reactive.Bindings;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace _Server.Script.Unary
{
    public class MatchMakeService : ServiceBase<IMatchMakeService>, IMatchMakeService
    {
        private static Dictionary<string, int> matches = new Dictionary<string, int>();
        private static MatchData updateMatchCache = new MatchData();
        
        public UnaryResult<int> JoinMatch(string matchName)
        {
            updateMatchCache.RoomName = matchName;
            var index = updateMatchCache.Count = matches[matchName]++;

            return UnaryResult(index);
        }

        public UnaryResult<Nil> LeaveMatch(string matchName)
        {
            updateMatchCache.RoomName = matchName;
            updateMatchCache.Count = --matches[matchName];
            
            return UnaryResult(Nil.Default);
        }

        public UnaryResult<int> MatchCount(string matchName) => UnaryResult(matches[matchName]);

        public UnaryResult<string> RequireMatch()
        {
            RemoveZeroOrMax();
            return UnaryResult(GetMatch());
        }
        
        private static string GetMatch()
        {
            string matchName = null;
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if(count >= 3)
                    continue;
                
                matchName = roomName;
                break;
            }
            
            if (!string.IsNullOrEmpty(matchName)) 
                return matchName;
            
            matchName = Utils.GUID;
            matches.Add(matchName, 0);
            
            return matchName;
        }
        
        private static void RemoveZeroOrMax()
        {
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if (count != 0 && count != 3)
                    continue;
                
                matches.Remove(roomName);
            }
        }
    }
}