﻿using System;
using API_Server.Script.Utility;
using Grpc.Core;
using Grpc.Core.Logging;
using MagicOnion.Server;
using MessagePack.Resolvers;

namespace API_Server
{
    internal static class Program
    {
        private const string host = "0.0.0.0";
        public static Server server { get; private set; }

        private static void Main()
        {
            CompositeResolver.RegisterAndSetAsDefault
            (
                StandardResolverAllowPrivate.Instance,
                MessagePack.Unity.UnityResolver.Instance,
                BuiltinResolver.Instance,
                AttributeFormatterResolver.Instance,
// replace enum resolver
                DynamicEnumAsStringResolver.Instance,
                DynamicGenericResolver.Instance,
                DynamicUnionResolver.Instance,
                DynamicObjectResolver.Instance,
                PrimitiveObjectResolver.Instance,
// final fallback(last priority)
                StandardResolver.Instance
            );
            
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
                    new ServerPort(host, 10123, ServerCredentials.Insecure)
                }
            };
            
            server.Start();

            Console.ReadLine();
        }

    }
}