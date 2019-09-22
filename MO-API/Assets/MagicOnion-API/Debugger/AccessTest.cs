using Grpc.Core;
using Info;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class AccessTest : MonoBehaviour
    {
        [SerializeField] private Button join = default, leave = default;

        private Access access;
        
        private void Awake()
        {
            access = GetComponent<Access>();
            
            join
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    await access.Join("TestRoom", PlayerInfo.Instance.PlayerIdentifier);
                });

            leave
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    await access.Leave();
                });

            access
                .JoinAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.name}が入室しました。");
                });

            access
                .LeaveAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.name}が退室しました。");
                });
        }
    }
}
