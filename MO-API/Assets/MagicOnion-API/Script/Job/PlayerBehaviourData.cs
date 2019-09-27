using System.Runtime.InteropServices;
using ServerShared.Utility;

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
}