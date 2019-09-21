using Grpc.Core;
using Info;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class AccessTest : ChannelBehaviour,IAccessControlReceiver
    {
        [SerializeField] private Button join = default, leave = default;

        private IAccessControlHub accessControlHub;
        
        public override UniTask Connect(Channel channel)
        {
            accessControlHub = StreamingHubClient.Connect<IAccessControlHub, IAccessControlReceiver>(channel, this);
            return UniTask.CompletedTask;
        }

        private void Awake()
        {
            join
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    await accessControlHub.JoinAsync("TestRoom", PlayerInfo.Instance.PlayerIdentifier);
                });

            leave
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    await accessControlHub.LeaveAsync();
                });

        }

        void IAccessControlReceiver.Join(PlayerIdentifier playerIdentifier)
        {
            Debug.Log($"{playerIdentifier.name}が入室。");
        }

        void IAccessControlReceiver.Leave(PlayerIdentifier playerIdentifier)
        {
            Debug.Log($"{playerIdentifier.name}が退室。");
        }
    }
}
