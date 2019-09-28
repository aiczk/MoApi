using System;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.API.Job;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using ServerShared.Utility;
using UniRx;
using UniRx.Async;
using Unity.Collections;
using UnityEngine;

namespace MagicOnion.API
{
    public class PlayerBehaviour : ChannelBehaviour, IPlayerBehaviourReceiver
    {
        public PlayerBehaviourData[] Parameters { get; } = new PlayerBehaviourData[4];
        
        private Subject<DroppedItem> drop = new Subject<DroppedItem>()
                                    ,get = new Subject<DroppedItem>();
        private Subject<WeaponParameter> register = new Subject<WeaponParameter>();
        private Subject<ShotParameter> shot = new Subject<ShotParameter>();
        private Subject<EquipmentParameter> change = new Subject<EquipmentParameter>();
        
        private IPlayerBehaviourHub playerBehaviourHub;
        
        public override void Connect(Channel channel)
        {
            playerBehaviourHub = StreamingHubClient.Connect<IPlayerBehaviourHub, IPlayerBehaviourReceiver>(channel, this);
        }

        public IObservable<DroppedItem> DropAsObservable => drop.Share();
        public IObservable<DroppedItem> GetAsObservable => get.Share();
        public IObservable<WeaponParameter> RegisterWeaponAsObservable => register.Share();
        public IObservable<ShotParameter> ShotAsObservable => shot.Share();
        public IObservable<EquipmentParameter> ChangeEquipmentAsObservable => change.Share();
        
        void IPlayerBehaviourReceiver.Drop(DroppedItem droppedItem) => drop.OnNext(droppedItem);
        void IPlayerBehaviourReceiver.Get(DroppedItem droppedItem) => get.OnNext(droppedItem);
        
        void IPlayerBehaviourReceiver.ChangeWeapon(EquipmentParameter equipmentParam)
        {
            var index = equipmentParam.Index;
            var currentEquipment = equipmentParam.MainEquipment;
            ref var cache = ref Parameters[index];
            cache = new PlayerBehaviourData(in cache, currentEquipment);
            
            change.OnNext(equipmentParam);
        }

        void IPlayerBehaviourReceiver.RegisterWeapon(WeaponParameter weaponParam)
        {
            var index = weaponParam.Index;
            ref var cache = ref Parameters[index];
            cache = new PlayerBehaviourData(in cache, weaponParam.Main, weaponParam.Sub);
            
            register.OnNext(weaponParam);
        }

        void IPlayerBehaviourReceiver.Shot(ShotParameter shotParam) => shot.OnNext(shotParam);

        public async UniTask Drop(DroppedItem droppedItem)
        {
            Debug.Log("Called");
            await playerBehaviourHub.DropAsync(droppedItem);
        }

        public async UniTask Get(DroppedItem droppedItem) => await playerBehaviourHub.GetAsync(droppedItem);

        public async UniTask ChangeWeapon(EquipmentParameter equipmentParam) =>
            await playerBehaviourHub.ChangeWeaponAsync(equipmentParam);

        public async UniTask RegisterWeapon(WeaponParameter weaponParam) =>
            await playerBehaviourHub.RegisterWeaponAsync(weaponParam);
        
        public async UniTask Shot(ShotParameter shotParam) => 
            await playerBehaviourHub.ShotAsync(shotParam);
    }
}
