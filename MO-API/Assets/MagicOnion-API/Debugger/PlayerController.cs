using System;
using Info;
using MagicOnion.API;
using MagicOnion.Utils;
using ServerShared.MessagePackObject;
using ServerShared.Utility;
using UniRx;
using UnityEngine;

namespace Debugger
{
    public class PlayerController : MonoBehaviour
    {
        private Movement movement;
        private PlayerBehaviour behaviour;
        private Renderer render;
        
        private PositionParameter positionParam;
        private RotationParameter rotationParam;
        private WeaponParameter weaponParam;
        private EquipmentParameter equipmentParam;
        private IDisposable disposable;

        private int playerIndex;
        
        private void Awake()
        {
            var system = GameObject.FindGameObjectWithTag("System_Online");
            movement = system.GetComponent<Movement>();
            behaviour = system.GetComponent<PlayerBehaviour>();
            
            playerIndex = GetComponent<IdentifierComponent>().index;
            render = GetComponent<Renderer>();
            render.material.color = Color.red;

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
            
            disposable =
            Observable
                .EveryUpdate()
                .Subscribe(async _ =>
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        weaponParam.Main = WeaponType.Pistol;
                        weaponParam.Sub = WeaponType.Rifle;
                        await behaviour.RegisterWeapon(weaponParam);
                    }

                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        var subCache = weaponParam.Sub;
                        weaponParam.Sub = weaponParam.Main;
                        equipmentParam.MainEquipment = weaponParam.Main = subCache;

                        await behaviour.ChangeWeapon(equipmentParam);
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        var dropItem = new DroppedItem
                        (
                            Utility.GetRandomValue(),
                            DroppedItemType.Recovery,
                            transform.position
                        );
                        
                        await behaviour.Drop(dropItem);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        var shot = new ShotParameter
                        {
                            Index = playerIndex,
                            Direction = transform.forward
                        };
                        
                        await behaviour.Shot(shot);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        await behaviour.Reload(playerIndex);
                    }
                });
        }

        private void OnDestroy()
        {
            render.material.color = Color.white;
            disposable.Dispose();
        }
    }
}
