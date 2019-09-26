using System.Collections.Generic;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.API.Job;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx.Async;

// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Movement : ChannelBehaviour,IMovementReceiver
    {
        public TransformParameter[] Parameters { get; } = new TransformParameter[4];
        private IMovementHub movementHub;
        
        public override void Connect(Channel channel)
        {
            movementHub = StreamingHubClient.Connect<IMovementHub, IMovementReceiver>(channel, this);
        }

        void IMovementReceiver.Move(PositionParameter positionParams)
        {
            var index = positionParams.index;
            ref readonly var cache = ref Parameters[index];
            Parameters[index] = new TransformParameter(in cache, positionParams.position);
        }

        void IMovementReceiver.Rotate(RotationParameter rotationParams)
        {
            var index = rotationParams.index;
            ref readonly var cache = ref Parameters[index];
            Parameters[index] = new TransformParameter(in cache, rotationParams.rotation);
        }
        
        public async UniTask Move(PositionParameter positionParameter) => await movementHub.MoveAsync(positionParameter);
        public async UniTask Rotation(RotationParameter rotationParameter) => await movementHub.RotateAsync(rotationParameter);
    }
}
