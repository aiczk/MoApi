using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared.Hub;
using ServerShared.MessagePackObject;

namespace _Server.Script.Hub
{
    public class MovementHub : StreamingHubBase<IMovementHub,IMovementReceiver>,IMovementHub
    {
        private static IGroup room;
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