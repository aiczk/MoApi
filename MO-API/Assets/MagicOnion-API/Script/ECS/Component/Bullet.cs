using Unity.Entities;
using Unity.Mathematics;

namespace Script.ECS.Component
{
    public struct Bullet : IComponentData
    {
        public float3 Direction;
        public boolean IsTouch;
        public int Power;
    }
}