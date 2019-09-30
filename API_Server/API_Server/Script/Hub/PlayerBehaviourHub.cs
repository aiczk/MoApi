using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;
using ServerShared.Hub;
using ServerShared.MessagePackObject;

namespace _Server.Script.Hub
{
    public class PlayerBehaviourHub : StreamingHubBase<IPlayerBehaviourHub, IPlayerBehaviourReceiver>,IPlayerBehaviourHub
    {
        private static IGroup room;
        
        protected override async ValueTask OnConnecting()
        {
            AccessControlHub
                .JoinAsObservable
                .Subscribe(async groupName => room = await Group.AddAsync(groupName));

            AccessControlHub
                .LeaveAsObservable
                .Subscribe(async _ => await room.RemoveAsync(Context));
        }
        
        public Task DropAsync(DroppedItem droppedItem)
        {
            Broadcast(room).Drop(droppedItem);
            return Task.CompletedTask;
        }

        public Task GetAsync(DroppedItem droppedItem)
        {
            Broadcast(room).Get(droppedItem);
            return Task.CompletedTask;
        }
        
        public Task ChangeWeaponAsync(EquipmentParameter equipmentParameter)
        {
            Broadcast(room).ChangeWeapon(equipmentParameter);
            return Task.CompletedTask;
        }

        public Task RegisterWeaponAsync(WeaponParameter weaponParameter)
        {
            Broadcast(room).RegisterWeapon(weaponParameter);
            return Task.CompletedTask;
        }

        public Task ShotAsync(ShotParameter shotParameter)
        {
            Broadcast(room).Shot(shotParameter);
            return Task.CompletedTask;;
        }

        public Task ReloadAsync(int index)
        {
            Broadcast(room).Reload(index);
            return Task.CompletedTask;
        }
    }
}