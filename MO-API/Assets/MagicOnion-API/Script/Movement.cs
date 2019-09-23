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
    public class Movement : ChannelBehaviour,IPlayerParameterReceiver
    {
        public TransformParameter[] parameters { get; } = new TransformParameter[4];
        
        private IPlayerParameterHub playerParameterHub;
        
        public override void Connect(Channel channel)
        {
            playerParameterHub = StreamingHubClient.Connect<IPlayerParameterHub, IPlayerParameterReceiver>(channel, this);
        }

        void IPlayerParameterReceiver.Move(PositionParameter positionParams)
        {
            var index = positionParams.index;
            ref readonly var cache = ref parameters[index];
            parameters[index] = new TransformParameter(in cache, positionParams.position);
        }

        void IPlayerParameterReceiver.Rotate(RotationParameter rotationParams)
        {
            var index = rotationParams.index;
            ref readonly var cache = ref parameters[index];
            parameters[index] = new TransformParameter(in cache, rotationParams.rotation);
        }

        public async UniTask Move(PositionParameter positionParameter)
        {
            await playerParameterHub.MoveAsync(positionParameter);
        }

        public async UniTask Rotation(RotationParameter rotationParameter)
        {
            await playerParameterHub.RotateAsync(rotationParameter);
        }
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
