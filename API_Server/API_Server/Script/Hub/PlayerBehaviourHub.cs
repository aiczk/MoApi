using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared.Hub;
using ServerShared.MessagePackObject;

namespace _Server.Script.Hub
{
    public class PlayerBehaviourHub : StreamingHubBase<IPlayerBehaviourHub, IPlayerBehaviourReceiver>,IPlayerBehaviourHub
    {
        private IGroup room;
        
        protected override ValueTask OnConnecting()
        {
            AccessControlHub
                .JoinAsObservable
                .Subscribe(async groupName => room = await Group.AddAsync(groupName));

            AccessControlHub
                .LeaveAsObservable
                .Subscribe(async _ => await room.RemoveAsync(Context));
            
            return base.OnConnecting();
        }
        
        public Task DropAsync(DroppedItem droppedItem)
        {
            BroadcastExceptSelf(room).Drop(droppedItem);
            return Task.CompletedTask;
        }

        public Task GetAsync(DroppedItem droppedItem)
        {
            BroadcastExceptSelf(room).Get(droppedItem);
            return Task.CompletedTask;
        }
        
        public Task ChangeWeaponAsync(EquipmentParameter equipmentParameter)
        {
            BroadcastExceptSelf(room).ChangeWeapon(equipmentParameter);
            return Task.CompletedTask;
        }

        public Task RegisterWeaponAsync(WeaponParameter weaponParameter)
        {
            BroadcastExceptSelf(room).RegisterWeapon(weaponParameter);
            return Task.CompletedTask;
        }

        public Task ShotAsync(ShotParameter shotParameter)
        {
            BroadcastExceptSelf(room).Shot(shotParameter);
            return Task.CompletedTask;;
        }

        public Task ReloadAsync(int index)
        {
            BroadcastExceptSelf(room).Reload(index);
            return Task.CompletedTask;
        }
    }
}