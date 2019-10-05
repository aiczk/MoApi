using Unity.Entities;
using Unity.Mathematics;

namespace Script.ECS.Component
{
    public struct Bullet : IComponentData
    {
        public boolean IsTouch;
        public int Power;
        public float FallUntil;
    }
}