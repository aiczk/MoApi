// ReSharper disable CheckNamespace

using MessagePack;
using UnityEngine;

namespace ServerShared.MessagePackObject
{
    [MessagePackObject]
    public class PlayerIdentifier
    {
        [Key(0)]
        public string name { get; set; }
        
        [Key(1)]
        public string id { get; set; }
    }

    [MessagePackObject]
    public class PositionParameter
    {
        [Key(0)] 
        public string id { get; set; }
        
        [Key(1)]
        public Vector3 position { get; set; }
    }

    [MessagePackObject]
    public class RotationParameter
    {
        [Key(0)]
        public string id { get; set; }
        
        [Key(1)]
        public Quaternion rotation { get; set; }
    }

    [MessagePackObject]
    public class MatchData
    {
        [Key(0)]
        public string roomName { get; set; }
        
        [Key(1)]
        public int count { get; set; }
    }
}
