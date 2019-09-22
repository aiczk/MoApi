using System;
using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx;
using UniRx.Async;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Access : ChannelBehaviour, IAccessControlReceiver
    {
        private Subject<PlayerIdentifier> join = new Subject<PlayerIdentifier>();
        private Subject<PlayerIdentifier> leave = new Subject<PlayerIdentifier>();
        
        private IAccessControlHub accessControlHub;
        private bool isJoin;
        
        public override void Connect(Channel channel)
        {
            accessControlHub = StreamingHubClient.Connect<IAccessControlHub, IAccessControlReceiver>(channel, this);
        }

        public override async UniTask DisConnect()
        {
            await accessControlHub.DisposeAsync();
        }

        void IAccessControlReceiver.Join(PlayerIdentifier playerIdentifier) => join.OnNext(playerIdentifier);
        void IAccessControlReceiver.Leave(PlayerIdentifier playerIdentifier) => leave.OnNext(playerIdentifier);

        public async UniTask Join(string roomName, PlayerIdentifier playerIdentifier)
        {
            if (isJoin)
                return;

            await accessControlHub.JoinAsync(roomName, playerIdentifier);
            isJoin = true;
        }

        public async UniTask Leave()
        {
            if(!isJoin)
                return;

            await accessControlHub.LeaveAsync();
            isJoin = false;
        }
        
        public IObservable<PlayerIdentifier> JoinAsObservable => join.Share();
        public IObservable<PlayerIdentifier> LeaveAsObservable => leave.Share();
    }
}
