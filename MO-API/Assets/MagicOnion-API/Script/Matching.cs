using System;
using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Unary;
using UniRx;
using UniRx.Async;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Matching : ChannelBehaviour
    {
        public IObservable<int> PlayerIndexAsObservable => playerIndex.Share();
        
        private Subject<int> playerIndex = new Subject<int>();
        private IMatchMakeService matchMakeService;
        private bool isJoin;
        
        public override void Connect(Channel channel)
        {
            matchMakeService = MagicOnionClient.Create<IMatchMakeService>(channel);
        }

        public async UniTask<string> Require() => await matchMakeService.RequireMatch();

        public async UniTask Join(string matchName)
        {
            if(isJoin)
                return;
            
            isJoin = true;
            
            var index = await matchMakeService.JoinMatch(matchName);
            playerIndex.OnNext(index);
        }

        public async UniTask Leave(string matchName)
        {
            if(!isJoin)
                return;
            
            await matchMakeService.LeaveMatch(matchName);
            isJoin = false;
        }

        public async UniTask<int> Count(string matchName)
        {
            if (!isJoin)
                return 0;

            return await matchMakeService.MatchCount(matchName);
        }
    }
}
