using System;
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
                .Subscribe(async context => await room.RemoveAsync(context));
            
            return base.OnConnecting();
        }
        
        public Task DropAsync(DroppedItem droppedItem)
        {
            BroadcastExceptSelf(room).Drop(droppedItem);
            return Task.CompletedTask;;
        }

        public Task GetAsync(DroppedItem droppedItem)
        {
            BroadcastExceptSelf(room).Get(droppedItem);
            return Task.CompletedTask;
        }
    }
}