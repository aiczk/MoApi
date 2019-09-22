using System.Collections.Generic;
using Grpc.Core;
using UniRx.Async;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace MagicOnion.API
{
    public interface IConnector
    {
        void Connect(Channel channel);
        UniTask DisConnect();
        UniTask Dispose();
    }
    
    public class ConnectorInfo : MonoBehaviour
    {
        private static HashSet<IConnector> connectors = new HashSet<IConnector>();
        private static bool isJoin, isConnect;
        
        private async UniTask OnApplicationQuit()
        {
            var disposes = connectors.Select(x => x.Dispose());
            await UniTask.WhenAll(disposes);
        }

        public static void Connect(Channel channel)
        {
            if(isConnect)
                return;

            foreach (var connector in connectors)
            {
                connector.Connect(channel);
            }
            
            Debug.Log("connect done");
            isConnect = true;
        }

        public static async UniTask DisConnect()
        {
            if(!isConnect)
                return;
            
            var disConnects = connectors.Select(x => x.DisConnect());
            await UniTask.WhenAll(disConnects);
            Debug.Log("disconnect done");

            isConnect = false;
        }

        public static void Register(IConnector connector)
        {
            var result = connectors.Add(connector);

            if (result && ChannelInfo.IsConnecting)
                connector.Connect(ChannelInfo.channel);
        }

        public static void UnRegister(IConnector connector)
        {
            if (connectors.Remove(connector) && ChannelInfo.IsConnecting)
                connector.DisConnect();
        }
    }
}
