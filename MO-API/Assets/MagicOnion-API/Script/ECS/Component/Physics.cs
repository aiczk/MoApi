using Unity.Entities;
using Unity.Mathematics;

namespace Script.ECS.Component
{
    public struct Physics : IComponentData
    {
        public float Power;
        public float3 CachedPosition;
        public float3 CurrentPosition;
    }
}