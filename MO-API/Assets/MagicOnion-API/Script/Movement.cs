using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.API.Job;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx.Async;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Movement : ChannelBehaviour,IMovementReceiver
    {
        public TransformData[] Parameters { get; } = new TransformData[4];
        private IMovementHub movementHub;
        
        public override void Connect(Channel channel) => 
            movementHub = StreamingHubClient.Connect<IMovementHub, IMovementReceiver>(channel, this);

        void IMovementReceiver.Move(PositionParameter positionParams)
        {
            var index = positionParams.Index;
            Parameters[index].Position = positionParams.Position;
        }

        void IMovementReceiver.Rotate(RotationParameter rotationParams)
        {
            var index = rotationParams.Index;
            Parameters[index].Rotation = rotationParams.Rotation;
        }
        
        public async UniTask Move(PositionParameter positionParameter) => await movementHub.MoveAsync(positionParameter);
        public async UniTask Rotation(RotationParameter rotationParameter) => await movementHub.RotateAsync(rotationParameter);
    }
}
