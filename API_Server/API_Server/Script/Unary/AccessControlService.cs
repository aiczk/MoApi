using System.Linq;
using API_Server.Script.Hub;
using MagicOnion;
using MagicOnion.Server;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class AccessControlService : ServiceBase<IAccessControlService>,IAccessControlService
    {
        public UnaryResult<PlayerIdentifier[]> GetCurrentTeamMate() => UnaryResult(AccessControlHub.Players.ToArray());
    }
}