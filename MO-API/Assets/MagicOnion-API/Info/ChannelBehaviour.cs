using Grpc.Core;
using UniRx.Async;
using UnityEngine;

namespace Info
{
    public class ChannelBehaviour : MonoBehaviour, IConnector
    {
        private void Start() => ConnectorInfo.Register(this);

        public virtual UniTask Connect(Channel channel) => UniTask.CompletedTask;
        public virtual UniTask DisConnect() => UniTask.CompletedTask;
        public virtual UniTask Dispose() => UniTask.CompletedTask;
    }
}
