using Grpc.Core;
using UniRx.Async;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class ChannelBehaviour : MonoBehaviour, IConnector
    {
        private void Start() => ConnectorInfo.Register(this);

        public virtual void Connect(Channel channel){}
        public virtual UniTask DisConnect() => UniTask.CompletedTask;
        public virtual UniTask Dispose() => UniTask.CompletedTask;
    }
}
