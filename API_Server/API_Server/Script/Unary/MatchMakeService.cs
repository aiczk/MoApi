using System.Collections.Generic;
using System.Threading.Tasks;
using API_Server.Script.Utility;
using MagicOnion;
using MagicOnion.Server;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class MatchMakeService : ServiceBase<IMatchMakeService>, IMatchMakeService
    {
        //部屋名 接続人数
        private Dictionary<string, int> matches = new Dictionary<string, int>();
        private string roomNameCache = null;

        public async UnaryResult<string> RequireMatch(PlayerIdentifier playerIdentifier) => await GetMatch();

        public UnaryResult<bool> RegisterMatch((string, int) matchData)
        {
            var (name, count) = matchData;
            return UnaryResult(matches.TryAdd(name, count));
        }
        
        public async Task<ServerStreamingResult<(string, int)>> NewMatch()
        {
            var stream = GetServerStreamingContext<(string, int)>();
            await stream.WriteAsync((roomNameCache, 1));
            return stream.Result();
        }

        //空きが存在していなければ作成。
        //空きが存在していれば入室。
        //4人になったら削除。
        private async ValueTask<string> GetMatch()
        {
            string name = null;
            foreach (var match in matches)
            {
                var (roomName, count) = match;
                
                if(count >= 4)
                    continue;
                
                name = roomName;
                break;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = Utils.GUID;
                roomNameCache = name;
                matches.Add(name, 1);
                await NewMatch();
            }

            return name;
        }
    }
}