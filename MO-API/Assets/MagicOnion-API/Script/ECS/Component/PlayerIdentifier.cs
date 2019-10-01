using Unity.Entities;
using UnityEngine;

namespace Script.ECS.Component
{
    public struct PlayerIdentifier : IComponentData
    {
        public int Index;
        public int Score;
    }
}
