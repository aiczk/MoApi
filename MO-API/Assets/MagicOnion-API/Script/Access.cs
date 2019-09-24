using System;
using Grpc.Core;
using Info;
using MagicOnion.API;
using MagicOnion.Client;
using Pool;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using ServerShared.Unary;
using UniRx;
using UniRx.Async;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Access : ChannelBehaviour, IAccessControlReceiver
    {
        public IObservable<PlayerIdentifier> JoinAsObservable => join.Share();
        public IObservable<PlayerIdentifier> LeaveAsObservable => leave.Share();
        public IObservable<int> PlayerJoinAsObservable => pio.Share();
        
        private Subject<PlayerIdentifier> join = new Subject<PlayerIdentifier>();
        private Subject<PlayerIdentifier> leave = new Subject<PlayerIdentifier>();
        private Subject<int> pio = new Subject<int>();
        private IAccessControlService accessControlService;
        private IAccessControlHub accessControlHub;
        private bool isJoin;
        
        public override void Connect(Channel channel)
        {
            accessControlHub = StreamingHubClient.Connect<IAccessControlHub, IAccessControlReceiver>(channel, this);
            accessControlService = MagicOnionClient.Create<IAccessControlService>(channel);
        }
        
        public override async UniTask DisConnect() => await accessControlHub.DisposeAsync();

        void IAccessControlReceiver.Join(PlayerIdentifier playerIdentifier) => join.OnNext(playerIdentifier);
        void IAccessControlReceiver.Leave(PlayerIdentifier playerIdentifier) => leave.OnNext(playerIdentifier);

        public async UniTask Join(int index, string roomName, PlayerIdentifier playerIdentifier)
        {
            if (isJoin)
                return;

            await accessControlHub.JoinAsync(roomName, playerIdentifier);
            pio.OnNext(index);
            isJoin = true;
        }

        public async UniTask Leave()
        {
            if(!isJoin)
                return;

            await accessControlHub.LeaveAsync();
            isJoin = false;
        }
        
        public async UniTask<PlayerIdentifier[]> TeamMate() => await accessControlService.GetCurrentTeamMate();
    }
}
