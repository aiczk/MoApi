using System;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared.Hub;
using ServerShared.MessagePackObject;

namespace _Server.Script.Hub
{
    public class MovementHub : StreamingHubBase<IMovementHub,IMovementReceiver>,IMovementHub
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

        public Task MoveAsync(PositionParameter positionParams)
        {
            BroadcastExceptSelf(room).Move(positionParams);
            
            return Task.CompletedTask;
        }

        public Task RotateAsync(RotationParameter rotationParams)
        {
            BroadcastExceptSelf(room).Rotate(rotationParams);
            
            return Task.CompletedTask;
        }
    }
}