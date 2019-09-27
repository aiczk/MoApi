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
        public string Name { get; set; }
        
        [Key(1)]
        public string Id { get; set; }
    }

    [MessagePackObject]
    public struct PositionParameter
    {
        [Key(0)] 
        public int Index { get; set; }
        
        [Key(1)]
        public Vector3 Position { get; set; }
    }

    [MessagePackObject]
    public struct RotationParameter
    {
        [Key(0)]
        public int Index { get; set; }
        
        [Key(1)]
        public Quaternion Rotation { get; set; }
    }

    [MessagePackObject]
    public class MatchData
    {
        [Key(0)]
        public string RoomName { get; set; }
        
        [Key(1)]
        public int Count { get; set; }
        
        public MatchData(string roomName, int count)
        {
            RoomName = roomName;
            Count = count;
        }

        public MatchData()
        {
            RoomName = default;
            Count = default;
        }
    }

    [MessagePackObject]
    public class DroppedItem
    {
        [Key(0)]
        public int DropOrGetPlayerIndex { get; set; }
        
        [Key(1)]
        public DroppedItemType DroppedItemType { get; set; }
        
        [Key(2)]
        public Vector3 Position { get; set; }
    }

    [MessagePackObject]
    public class WeaponParameter
    {
        [Key(0)] 
        public int Index { get; set; }
        
        [Key(1)]
        public WeaponType Main { get; set; }
        
        [Key(2)]
        public WeaponType Sub { get; set; }
    }

    [MessagePackObject]
    public class ShotParameter
    {
        [Key(0)]
        public int Index { get; set; }

        [Key(1)] 
        public Vector3 Direction { get; set; }
    }
    
    [MessagePackObject]
    public class EquipmentParameter
    {
        [Key(0)]
        public int Index { get; set; }

        [Key(1)] 
        public WeaponType MainEquipment { get; set; }
    }
}
