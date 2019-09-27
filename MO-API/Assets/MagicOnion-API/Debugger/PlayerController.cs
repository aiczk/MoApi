using Info;
using MagicOnion.API;
using ServerShared.MessagePackObject;
using ServerShared.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Debugger
{
    public class PlayerController : MonoBehaviour
    {
        private Movement movement;
        private PlayerBehaviour behaviour;
        
        private PositionParameter positionParam;
        private RotationParameter rotationParam;
        private WeaponParameter weaponParam;
        private EquipmentParameter equipmentParam;

        private int playerIndex;
        
        private void Awake()
        {
            var system = GameObject.FindGameObjectWithTag("System_Online");
            movement = system.GetComponent<Movement>();
            behaviour = system.GetComponent<PlayerBehaviour>();
            
            playerIndex = GetComponent<IdentifierComponent>().index;
            GetComponent<Renderer>().material.color = Color.red;

            positionParam.Index = playerIndex;
            rotationParam.Index = playerIndex;
            weaponParam.Index = playerIndex;
            equipmentParam.Index = playerIndex;
            
            transform
                .ObserveEveryValueChanged(x => x.position)
                .Skip(1)
                .Subscribe(async position =>
                {
                    positionParam.Position = position;
                    await movement.Move(positionParam);
                });

            transform
                .ObserveEveryValueChanged(x => x.rotation)
                .Skip(1)
                .Subscribe(async rotation =>
                {
                    rotationParam.Rotation = rotation;
                    await movement.Rotation(rotationParam);
                });

            var update = this.UpdateAsObservable().Share();

            update
                .Subscribe(async _=>
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        weaponParam.Main = WeaponType.Pistol;
                        weaponParam.Sub = WeaponType.Rifle;
                        await behaviour.RegisterWeapon(weaponParam);
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        var subCache = weaponParam.Sub;
                        weaponParam.Sub = weaponParam.Main;
                        equipmentParam.MainEquipment = weaponParam.Main = subCache;
                        
                        await behaviour.ChangeWeapon(equipmentParam);
                    }

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        var dropItem = new DroppedItem(DroppedItemType.Recovery, transform.position);
                        await behaviour.Drop(dropItem);
                    }
                });
        }
    }
}
