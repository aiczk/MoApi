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
        [SerializeField] private Button requireMatch = default;
        
        private IMatchMakeService matchMakeService;
    
        public override UniTask Connect(Channel channel)
        {
            matchMakeService = MagicOnionClient.Create<IMatchMakeService>(channel);
            return UniTask.CompletedTask;
        }

        //一定時間待って、人が入ってこなかったら更新をかける。
        private void Awake()
        {
            var click =
            requireMatch
                .OnClickAsObservable()
                .Share();

            click
                .Subscribe(async x =>
                {
                    var roomName = await matchMakeService.RequireMatch();
                    Debug.Log(roomName);
                });

            click
                .Subscribe(async x => await ObserveNewMatch());
        }

        private async UniTask ObserveNewMatch()
        {
            var data = await matchMakeService.NewMatch();
                
            await data
                  .ResponseStream
                  .ForEachAsync(async matchData =>
                  {
                      Debug.Log($"NewMatchName :{matchData.roomName}");
                      await matchMakeService.RegisterMatch(matchData);
                  });
        }
    }
}
