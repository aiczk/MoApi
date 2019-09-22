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
        private string roomName = default;
    
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
                    roomName = await matchMakeService.RequireMatch();
                    Debug.Log(roomName);
                });

            click
                .Subscribe(async x =>
                {
                    var observeNew = ObserveNewMatch();
                    var observeUpdate = ObserveUpdateMatch();

                    await UniTask.WhenAll(observeNew, observeUpdate);
                });

            joinMatch
                .OnClickAsObservable()
                .Subscribe(async x => await matchMakeService.JoinMatch(roomName));

            leaveMatch
                .OnClickAsObservable()
                .Subscribe(async x => await matchMakeService.LeaveMatch());
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

        private async UniTask ObserveUpdateMatch()
        {
            var data = await matchMakeService.UpdateMatch();

            await data
                  .ResponseStream
                  .ForEachAsync(async newMatchData =>
                  {
                      Debug.Log($"UpdateMatchName :{newMatchData.roomName} Count :{newMatchData.count.ToString()}");
                      await matchMakeService.RegisterMatch(newMatchData);
                  });
        }
    }
}
