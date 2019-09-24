using System.Linq;
using _Server.Script.Hub;
using MagicOnion;
using MagicOnion.Server;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace _Server.Script.Unary
{
    public class AccessControlService : ServiceBase<IAccessControlService>,IAccessControlService
    {
        public UnaryResult<PlayerIdentifier[]> GetCurrentTeamMate() => UnaryResult(AccessControlHub.Players.ToArray());
    }
}