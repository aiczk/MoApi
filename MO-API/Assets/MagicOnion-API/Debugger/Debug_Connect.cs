using MagicOnion.API;
using MessagePack.Resolvers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger
{
    public class Debug_Connect : MonoBehaviour
    {
        [SerializeField] private Button connect = default, disconnect = default, reset = default;

        private void Awake()
        {
            connect
                .OnClickAsObservable()
                .Subscribe(async x =>
                {
                    Debug.Log("Connect"); 
                    var channel = await ChannelInfo.Connect("localhost:10000");
                    ConnectorInfo.Connect(channel);
                });

            disconnect
                .OnClickAsObservable()
                .Subscribe(async x =>
                {
                    Debug.Log("DisConnect");
                    await ConnectorInfo.DisConnect();
                    await ChannelInfo.DisConnect();
                });

            reset
                .OnClickAsObservable()
                .Where(x => !ChannelInfo.IsConnecting)
                .Subscribe(x =>
                {
                    Debug.Log("Reset");
                    ChannelInfo.ResetChannel();
                });
        }
    }
}
