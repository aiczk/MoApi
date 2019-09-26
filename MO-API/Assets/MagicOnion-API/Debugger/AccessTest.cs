using Info;
using MagicOnion.API;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class AccessTest : MonoBehaviour
    {
        [SerializeField] private Button join = default, leave = default;
        
        private Matching matching;
        private Access access;

        private string roomName;
        
        private void Awake()
        {
            access = GetComponent<Access>();
            matching = GetComponent<Matching>();

            join
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    roomName = await matching.Require();
                    await matching.Join(roomName);
                    await access.Join(roomName, PlayerInfo.Instance.PlayerIdentifier);
                });

            leave
                .OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    await access.Leave();
                    await matching.Leave(roomName);
                });

            access
                .JoinAsObservable
                .Subscribe(async player =>
                {
                    Debug.Log($"{player.name}が{roomName}に入室しました。");
                    var currentCount = await matching.Count(roomName);
                    Debug.Log($"現在部屋にいる人数は{currentCount.ToString()}人です。");
                });

            access
                .LeaveAsObservable
                .Subscribe(async player =>
                {
                    Debug.Log($"{player.name}が{roomName}を退室しました。");
                    var currentCount = await matching.Count(roomName);
                    Debug.Log($"現在部屋にいる人数は{currentCount.ToString()}人です。");
                });
        }
    }
}
