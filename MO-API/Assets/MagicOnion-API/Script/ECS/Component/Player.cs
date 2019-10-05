using Unity.Entities;
using UnityEngine;

namespace Script.ECS.Component
{
    public struct Player : IComponentData
    {
        public int Index;
        public int KillCount;
    }
}
