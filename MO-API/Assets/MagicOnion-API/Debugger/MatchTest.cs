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
            Debug.Log("done");
            return UniTask.CompletedTask;
        }

        public override UniTask DisConnect()
        {
            flag = false;
            return base.DisConnect();
        }

        //一定時間待って、人が入ってこなかったら更新をかける。
        private void Awake()
        {
            requireMatch
                .OnClickAsObservable()
                .Subscribe(async x =>
                    {
                        var roomName = await matchMakeService.RequireMatch(PlayerInfo.Instance.PlayerIdentifier);
                        Debug.Log(roomName);
                        await ObserveNewMatch();
                    });
        }

        private async UniTask ObserveNewMatch()
        {
            var data = await matchMakeService.NewMatch();
                
            await data
                  .ResponseStream
                  .ForEachAsync(async roomName =>
                  {
                      Debug.Log($"RoomName :{roomName}");
                      await matchMakeService.RegisterMatch(roomName);
                  });
        }
    }
}
