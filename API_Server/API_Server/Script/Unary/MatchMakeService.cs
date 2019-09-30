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
        
        public UnaryResult<int> JoinMatch(string matchName)
        {
            var index = matches[matchName] += 1;
            return UnaryResult(index);
        }

        public UnaryResult<Nil> LeaveMatch(string matchName)
        { 
            matches[matchName] -= 1;
            return UnaryResult(Nil.Default);
        }

        public UnaryResult<int> MatchCount(string matchName) => UnaryResult(matches[matchName] + 1);

        public UnaryResult<string> RequireMatch()
        {
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
            matches.Add(matchName, -1);
            
            return matchName;
        }
    }
}