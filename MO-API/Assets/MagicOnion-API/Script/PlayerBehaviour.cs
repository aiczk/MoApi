using System;
using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx;
using UniRx.Async;

namespace MagicOnion.API
{
    public class PlayerBehaviour : ChannelBehaviour, IPlayerBehaviourReceiver
    {
        public IObservable<DroppedItem> DropAsObservable => drop.Share();
        public IObservable<DroppedItem> GetAsObservable => get.Share();
        
        private Subject<DroppedItem> drop = new Subject<DroppedItem>()
                                    ,get = new Subject<DroppedItem>();
        
        private IPlayerBehaviourHub playerBehaviourHub;
        
        public override void Connect(Channel channel)
        {
            playerBehaviourHub = StreamingHubClient.Connect<IPlayerBehaviourHub, IPlayerBehaviourReceiver>(channel, this);
        }

        void IPlayerBehaviourReceiver.Drop(DroppedItem droppedItem) => drop.OnNext(droppedItem);
        void IPlayerBehaviourReceiver.Get(DroppedItem droppedItem) => get.OnNext(droppedItem);

        public async UniTask Drop(DroppedItem droppedItem) => await playerBehaviourHub.DropAsync(droppedItem);
        public async UniTask Get(DroppedItem droppedItem) => await playerBehaviourHub.GetAsync(droppedItem);
    }
}
