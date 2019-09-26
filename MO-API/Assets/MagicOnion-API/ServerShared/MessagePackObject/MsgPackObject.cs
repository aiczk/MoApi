using MessagePack;
using ServerShared.Utility;
using UnityEngine;
// ReSharper disable CheckNamespace

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
        public int index { get; set; }
        
        [Key(1)]
        public Vector3 position { get; set; }
    }

    [MessagePackObject]
    public class RotationParameter
    {
        [Key(0)]
        public int index { get; set; }
        
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
        
        public MatchData(string roomName, int count)
        {
            this.roomName = roomName;
            this.count = count;
        }

        public MatchData()
        {
            roomName = default;
            count = default;
        }
    }

    [MessagePackObject]
    public class DroppedItem
    {
        [Key(0)]
        public int dropOrGetPlayerIndex { get; set; }
        
        [Key(1)]
        public DroppedItemType droppedItemType { get; set; }
        
        [Key(2)]
        public Vector3 position { get; set; }
        
        [Key(3)]
        public Quaternion rotation { get; set; }
    }

    [MessagePackObject]
    public class WeaponParameter
    {
        [Key(0)] 
        public int index { get; set; }
        
        [Key(1)]
        public WeaponType main { get; set; }
        
        [Key(2)]
        public WeaponType sub { get; set; }
    }

    [MessagePackObject]
    public class ShotParameter
    {
        [Key(0)]
        public int index { get; set; }
        
        [Key(1)] 
        public Vector3 position { get; set; }

        [Key(2)] 
        public Vector3 velocity { get; set; }
    }
    
    [MessagePackObject]
    public class EquipmentParameter
    {
        [Key(0)]
        public int index { get; set; }

        [Key(1)] 
        public WeaponType weaponType { get; set; }
    }
}
