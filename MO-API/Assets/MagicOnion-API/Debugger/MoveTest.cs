using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using UnityEngine;

namespace Debugger
{
    public class MoveTest : MonoBehaviour
    {
        private Movement movement;

        private void Awake()
        {
            movement = GetComponent<Movement>();
        }
    }
}
