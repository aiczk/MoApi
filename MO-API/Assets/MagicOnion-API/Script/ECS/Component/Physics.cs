using System;
using Unity.Entities;
using Unity.Mathematics;
// ReSharper disable InconsistentNaming

namespace Script.ECS.Component
{
    [Serializable]
    public struct Physics : IComponentData
    {
        public float Mass;
        public float3 Force;
        public float3 CachedPosition;
        public float3 CurrentPosition;
    }
}