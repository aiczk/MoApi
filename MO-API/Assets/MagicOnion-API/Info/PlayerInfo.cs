using _Script.Application.Utility.Base;
using Info;
using ServerShared.MessagePackObject;
using UnityEngine;

namespace MagicOnion.API
{
    public class PlayerInfo : SingletonMonoBehaviour<PlayerInfo>
    {
        public PlayerIdentifier PlayerIdentifier => new PlayerIdentifier
        {
            Id = Utility.GUID,
            Name = "PLAYER"
        };
    }
}