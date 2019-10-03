using Unity.Entities;
using Unity.Mathematics;

namespace Script.ECS.Component
{
    public struct Collision : IComponentData
    {
        public float Friction;
        public float Bounciness;
        public float3 Position;
        public quaternion Rotation;
        public CollisionType CollisionType;
        public float2 Size;
    }
    
    public enum CollisionType
    {
        Plane
    }
}