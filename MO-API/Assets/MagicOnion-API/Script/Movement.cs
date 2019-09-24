using System.Collections.Generic;
using Grpc.Core;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx.Async;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Movement : ChannelBehaviour,IMovementReceiver
    {
        public TransformParameter[] parameters { get; } = new TransformParameter[4];
        private IMovementHub movementHub;
        
        public override void Connect(Channel channel)
        {
            movementHub = StreamingHubClient.Connect<IMovementHub, IMovementReceiver>(channel, this);
        }

        void IMovementReceiver.Move(PositionParameter positionParams)
        {
            var index = positionParams.index;
            ref readonly var cache = ref parameters[index];
            parameters[index] = new TransformParameter(in cache, positionParams.position);
        }

        void IMovementReceiver.Rotate(RotationParameter rotationParams)
        {
            var index = rotationParams.index;
            ref readonly var cache = ref parameters[index];
            parameters[index] = new TransformParameter(in cache, rotationParams.rotation);
        }
        
        public async UniTask Move(PositionParameter positionParameter) => await movementHub.MoveAsync(positionParameter);
        public async UniTask Rotation(RotationParameter rotationParameter) => await movementHub.RotateAsync(rotationParameter);
    }
    
    public readonly struct TransformParameter
    {
        public readonly Vector3 position;
        public readonly Quaternion rotation;
        
        public TransformParameter(in TransformParameter copy, Vector3 position)
        {
            this.position = position;
            rotation = copy.rotation;
        }
        
        public TransformParameter(in TransformParameter copy, Quaternion rotation)
        {
            position = copy.position;
            this.rotation = rotation;
        }
    }
}
