using System.Linq;
using API_Server.Script.Hub;
using MagicOnion;
using MagicOnion.Server;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    public class RoomService : ServiceBase<IRoomService>,IRoomService
    {
        public UnaryResult<PlayerIdentifier[]> GetCurrentRoomMate() => UnaryResult(AccessControlHub.Players.ToArray());
    }
}