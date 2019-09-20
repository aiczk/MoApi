﻿using Grpc.Core;
using UniRx.Async;
using UnityEngine;

#pragma warning disable 1998

namespace Info
{
    public class ChannelInfo : MonoBehaviour
    {
        public static Channel channel { get; private set; }
        public static bool IsConnecting
        {
            get
            {
                if (channel == null)
                    return false;
                                
                return channel != null && channel.State == ChannelState.Ready;
            }
        }

        public static async UniTask<Channel> Connect(string listen)
        {
            if(IsConnecting)
                return channel;
            
            channel = new Channel(listen, ChannelCredentials.Insecure);
            
            await channel.ConnectAsync();

            return channel;
        }

        public static async UniTask DisConnect()
        {
            if(!IsConnecting)
                return;

            //bug 鯖ごと死ぬからダメ。
            //await channel.ShutdownAsync();
        }

        public static void ResetChannel() => channel = null;
    }
}
