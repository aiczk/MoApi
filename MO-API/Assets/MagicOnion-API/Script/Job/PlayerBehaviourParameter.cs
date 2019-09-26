using ServerShared.Utility;

namespace MagicOnion.API.Job
{
    public readonly struct PlayerBehaviourParameter
    {
        public readonly WeaponType CurrentEquipmentWeapon;
        public readonly WeaponType MainWeapon;
        public readonly WeaponType SubWeapon;

        public PlayerBehaviourParameter(in PlayerBehaviourParameter copy, WeaponType currentEquipmentWeapon)
        {
            CurrentEquipmentWeapon = currentEquipmentWeapon;
            MainWeapon = copy.MainWeapon;
            SubWeapon = copy.SubWeapon;
        }
        
        public PlayerBehaviourParameter(in PlayerBehaviourParameter copy, WeaponType mainWeapon,WeaponType subWeapon)
        {
            CurrentEquipmentWeapon = copy.CurrentEquipmentWeapon;
            MainWeapon = mainWeapon;
            SubWeapon = subWeapon;
        }
    }
}