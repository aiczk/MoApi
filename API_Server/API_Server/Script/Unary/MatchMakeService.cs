using MagicOnion;
using MagicOnion.Server;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class MatchMakeService : ServiceBase<IMatchMakeService>, IMatchMakeService
    {
        public async UnaryResult<string> RequireMatchName()
        {
            
        }
    }
}