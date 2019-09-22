using Grpc.Core;
using Info;
using MagicOnion;
using MagicOnion.Client;
using ServerShared.MessagePackObject;
using ServerShared.Unary;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class MatchTest : ChannelBehaviour
    {
        [SerializeField] private Button requireMatch = default, joinMatch = default, leaveMatch = default;
        
        private IMatchMakeService matchMakeService;
        private string matchName = default;
        private bool isJoin;
        
        public override UniTask Connect(Channel channel)
        {
            matchMakeService = MagicOnionClient.Create<IMatchMakeService>(channel);
            return UniTask.CompletedTask;
        }

        //一定時間待って、人が入ってこなかったら更新をかける。
        private void Awake()
        {
            var require = requireMatch.OnClickAsObservable().Share();

            require
                .Subscribe(async x =>
                { 
                    matchName = await matchMakeService.RequireMatch();
                    Debug.Log(matchName);
                });

            joinMatch
                .OnClickAsObservable()
                .Where(x => !isJoin)
                .Subscribe(async x =>
                {
                    await matchMakeService.JoinMatch(matchName);
                    isJoin = true;
                });

            leaveMatch
                .OnClickAsObservable()
                .Where(x => isJoin)
                .Subscribe(async x =>
                {
                    await matchMakeService.LeaveMatch(matchName);
                    isJoin = false;
                });
        }
    }
}
