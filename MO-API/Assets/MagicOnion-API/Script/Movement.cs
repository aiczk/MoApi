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
        public TransformData[] Parameters { get; } = new TransformData[4];
        private IMovementHub movementHub;
        
        public override void Connect(Channel channel)
        {
            movementHub = StreamingHubClient.Connect<IMovementHub, IMovementReceiver>(channel, this);
        }

        void IMovementReceiver.Move(PositionParameter positionParams)
        {
            var index = positionParams.Index;
            ref var cache = ref Parameters[index];
            cache = new TransformData(in cache, positionParams.Position);
        }

        void IMovementReceiver.Rotate(RotationParameter rotationParams)
        {
            var index = rotationParams.Index;
            ref var cache = ref Parameters[index];
            cache = new TransformData(in cache, rotationParams.Rotation);
        }
        
        public async UniTask Move(PositionParameter positionParameter) => await movementHub.MoveAsync(positionParameter);
        public async UniTask Rotation(RotationParameter rotationParameter) => await movementHub.RotateAsync(rotationParameter);
    }
}
