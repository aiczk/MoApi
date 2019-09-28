using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using _Server.Script.Utility;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;
using ServerShared.Hub;
using ServerShared.MessagePackObject;

namespace _Server.Script.Hub
{
    public class AccessControlHub : StreamingHubBase<IAccessControlHub, IAccessControlReceiver>, IAccessControlHub
    {
        public static IObservable<string> JoinAsObservable => join.Share();
        public static IObservable<Unit> LeaveAsObservable => leave.Share();
        public static ICollection<PlayerIdentifier> Players { get; private set; }

        private static Subject<string> join = new Subject<string>();
        private static Subject<Unit> leave = new Subject<Unit>();
        
        private IGroup room;
        private IInMemoryStorage<PlayerIdentifier> storage;
        private PlayerIdentifier self;

        public async Task JoinAsync(string roomName, PlayerIdentifier playerIdentifier)
        {
            (room, storage) = await Group.AddAsync(roomName, playerIdentifier);

            self = playerIdentifier;
            BroadcastExceptSelf(room).Join(playerIdentifier);
            Players = storage.AllValues;
            
            join.OnNext(roomName);
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(Context);
            
            leave.OnNext(default);
            BroadcastExceptSelf(room).Leave(self);

            Players = null;
        }
    }
}