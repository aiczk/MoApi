using _Script.Application.Utility.Base;
using ServerShared.MessagePackObject;
using UnityEngine;

namespace Info
{
    public class PlayerInfo : SingletonMonoBehaviour<PlayerInfo>
    {
        public PlayerIdentifier PlayerIdentifier => new PlayerIdentifier
        {
            id = "Test",
            name = "PLAYER"
        };
    }
}