﻿using System;
using _Server.Script.Utility;
using Grpc.Core;
using Grpc.Core.Logging;
using MagicOnion.Redis;
using MagicOnion.Server;
using MessagePack.Resolvers;

namespace _Server
{
    internal static class Program
    {
        private const string host = "0.0.0.0";
        public static Server server { get; private set; }

        private static void Main()
        {
            GrpcEnvironment.SetLogger(new ConsoleLogger());
            
            server = new Server
            {
                Services =
                {
                    MagicOnionEngine.BuildServerServiceDefinition(true)
                },
                Ports =
                {
                    new ServerPort(host, 10000, ServerCredentials.Insecure),
                }
            };
            
            server.Start();

            Console.ReadLine();
        }

    }
}