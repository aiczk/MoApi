using System.Collections.Generic;
using MagicOnion;
using MagicOnion.Server;
using ServerShared.MessagePackObject;
using ServerShared.Unary;

namespace API_Server.Script.Unary
{
    //今サーバーにつながっている人数。
    public class WorldService : ServiceBase<IWorldService>, IWorldService
    {
        public static HashSet<PlayerIdentifier> ConnectedPlayers { get; private set; }
        
        public UnaryResult<bool> RegisterWorld(PlayerIdentifier playerIdentifier)
        {
            return UnaryResult(ConnectedPlayers.Add(playerIdentifier));
        }
    }
}