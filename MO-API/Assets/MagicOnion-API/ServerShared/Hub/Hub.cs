﻿using System.Threading.Tasks;
using MagicOnion;
using ServerShared.MessagePackObject;

namespace ServerShared.Hub
{
    public interface IAccessControlHub : IStreamingHub<IAccessControlHub, IAccessControlReceiver>
    {
        Task JoinAsync(string roomName, PlayerIdentifier playerIdentifier);
        Task LeaveAsync();
    }

    public interface IAccessControlReceiver
    {
        void Join(PlayerIdentifier playerIdentifier);
        void Leave(PlayerIdentifier playerIdentifier);
    }
    

    public interface IMovementHub : IStreamingHub<IMovementHub, IMovementReceiver>
    {
        Task MoveAsync(PositionParameter positionParams);
        Task RotateAsync(RotationParameter rotationParams);
    }

    public interface IMovementReceiver
    {
        void Move(PositionParameter positionParams);
        void Rotate(RotationParameter rotationParams);
    }
}
