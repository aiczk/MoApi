using System.Collections.Generic;
using Grpc.Core;
using UniRx.Async;
using UnityEngine;

namespace Info
{
    public interface IConnector
    {
        UniTask Connect(Channel channel);
        UniTask DisConnect();
        UniTask Dispose();
    }
    
    public class ConnectorInfo : MonoBehaviour
    {
        private static HashSet<IConnector> disConnectors = new HashSet<IConnector>();
        private static bool isJoin, isConnect;
        
        private async UniTask OnApplicationQuit()
        {
            var disposes = disConnectors.Select(x => x.Dispose());
            await UniTask.WhenAll(disposes);
        }

        public static async UniTask Connect(Channel channel)
        {
            if(isConnect)
                return;
            
            var connects = disConnectors.Select(x => x.Connect(channel));
            await UniTask.WhenAll(connects);
            Debug.Log("connect done");

            isConnect = true;
        }

        public static async UniTask DisConnect()
        {
            if(!isConnect)
                return;
            
            var disConnects = disConnectors.Select(x => x.DisConnect());
            await UniTask.WhenAll(disConnects);
            Debug.Log("disconnect done");

            isConnect = false;
        }

        public static void Register(IConnector connector)
        {
            var result = disConnectors.Add(connector);

            if (result && ChannelInfo.IsConnecting)
                connector.Connect(ChannelInfo.channel);
        }

        public static void UnRegister(IConnector connector)
        {
            if (disConnectors.Remove(connector) && ChannelInfo.IsConnecting)
                connector.DisConnect();
        }
    }
}
