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
        private bool flag = true;
    
        public override UniTask Connect(Channel channel)
        {
            matchMakeService = MagicOnionClient.Create<IMatchMakeService>(channel);
            return UniTask.CompletedTask;
        }

        public override UniTask DisConnect()
        {
            flag = false;
            return base.DisConnect();
        }

        private async UniTask Awake()
        {
            await UniTask.WaitUntil(() => matchMakeService == null);

            while (flag)
            {
                var data = await matchMakeService.NewMatch();
                
                await data
                      .ResponseStream
                      .ForEachAsync(async matchData =>
                      {
                          Debug.Log($"RoomName :{matchData.Item1} Count :{matchData.Item2.ToString()}");
                          await matchMakeService.RegisterMatch(matchData);
                      });
            }
        }

        private void Start()
        {
            requireMatch
                .OnClickAsObservable()
                .Subscribe(async x =>
                    {
                        await matchMakeService.RequireMatch(new PlayerIdentifier
                        {
                            id = "TEST",
                            name = "TEST PLAYER"
                        });
                    });
        }
    }
}
