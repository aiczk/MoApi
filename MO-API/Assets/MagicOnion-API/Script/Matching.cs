using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Unary;
using UniRx.Async;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public class Matching : ChannelBehaviour
    {
        private IMatchMakeService matchMakeService;
        private bool isJoin;
        
        public override void Connect(Channel channel)
        {
            matchMakeService = MagicOnionClient.Create<IMatchMakeService>(channel);
        }

        public async UniTask<string> Require() => await matchMakeService.RequireMatch();

        public async UniTask<int> Join(string matchName)
        {
            if(isJoin)
                return 0;
            
            isJoin = true;
            return await matchMakeService.JoinMatch(matchName);
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
