using System;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.API.Job;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx;
using UniRx.Async;

namespace MagicOnion.API
{
    //todo ECS向けに4要素だけの配列に変更を加える。
    //movement参照。
    public class PlayerBehaviour : ChannelBehaviour, IPlayerBehaviourReceiver
    {
        public PlayerBehaviourParameter[] Parameters { get; } = new PlayerBehaviourParameter[4];
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
        
        void IPlayerBehaviourReceiver.ChangeWeapon(EquipmentParameter equipmentParameter)
        {
            var index = equipmentParameter.index;
            var currentEquipment = equipmentParameter.weaponType;
            ref readonly var cache = ref Parameters[index];
            Parameters[index] = new PlayerBehaviourParameter(in cache, currentEquipment);
            
            change.OnNext(equipmentParameter);
        }

        void IPlayerBehaviourReceiver.RegisterWeapon(WeaponParameter weaponParameter)
        {
            var index = weaponParameter.index;
            var mainWeapon = weaponParameter.main;
            var subWeapon = weaponParameter.sub;
            ref readonly var cache = ref Parameters[index];
            Parameters[index] = new PlayerBehaviourParameter(in cache, mainWeapon, subWeapon);
            
            register.OnNext(weaponParameter);
        }

        void IPlayerBehaviourReceiver.Shot(ShotParameter shotParameter) => shot.OnNext(shotParameter);

        public async UniTask Drop(DroppedItem droppedItem) => await playerBehaviourHub.DropAsync(droppedItem);
        public async UniTask Get(DroppedItem droppedItem) => await playerBehaviourHub.GetAsync(droppedItem);

        public async UniTask Change(EquipmentParameter equipmentParameter) =>
            await playerBehaviourHub.ChangeWeaponAsync(equipmentParameter);

        public async UniTask Register(WeaponParameter weaponParameter) =>
            await playerBehaviourHub.RegisterWeaponAsync(weaponParameter);

        public async UniTask Shot(ShotParameter shotParameter) => 
            await playerBehaviourHub.ShotAsync(shotParameter);
    }
}
