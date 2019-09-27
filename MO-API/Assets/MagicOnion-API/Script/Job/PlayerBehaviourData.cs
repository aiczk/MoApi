using System.Runtime.InteropServices;
using ServerShared.MessagePackObject;
using ServerShared.Utility;
using UnityEngine;

namespace MagicOnion.API.Job
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct PlayerBehaviourData
    {
        public readonly WeaponType CurrentEquipmentWeapon;
        public readonly WeaponType MainWeapon;
        public readonly WeaponType SubWeapon;

        public PlayerBehaviourData(in PlayerBehaviourData copy, WeaponType currentEquipmentWeapon)
        {
            CurrentEquipmentWeapon = currentEquipmentWeapon;
            MainWeapon = copy.MainWeapon;
            SubWeapon = copy.SubWeapon;
        }
        
        public PlayerBehaviourData(in PlayerBehaviourData copy, WeaponType mainWeapon,WeaponType subWeapon)
        {
            CurrentEquipmentWeapon = copy.CurrentEquipmentWeapon;
            MainWeapon = mainWeapon;
            SubWeapon = subWeapon;
        }
    }
    
    public readonly struct ShotData
    {
        public readonly int index;
        public readonly Vector3 Direction;

        public ShotData(int index, Vector3 direction)
        {
            this.index = index;
            this.Direction = direction;
        }

        public ShotData(ShotParameter copy)
        {
            index = copy.Index;
            Direction = copy.Direction;
        }
    }
}