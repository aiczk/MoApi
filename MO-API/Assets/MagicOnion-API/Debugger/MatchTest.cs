using Grpc.Core;
using Info;
using MagicOnion;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.MessagePackObject;
using ServerShared.Unary;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class MatchTest : MonoBehaviour
    {
        [SerializeField] private Button requireMatch = default, joinMatch = default, leaveMatch = default;

        private Matching matching;
        private string matchName = default;
        
        //一定時間待って、人が入ってこなかったら更新をかける。
        private void Awake()
        {
            matching = GetComponent<Matching>();
            var require = requireMatch.OnClickAsObservable().Share();

            require
                .Subscribe(async x =>
                {
                    matchName = await matching.Require();
                    Debug.Log(matchName);
                });

            joinMatch
                .OnClickAsObservable()
                .Subscribe(async x =>
                {
                    await matching.Join(matchName);
                });

            leaveMatch
                .OnClickAsObservable()
                .Subscribe(async x =>
                {
                    await matching.Leave(matchName);
                });
        }
    }
}
