using Unity.Entities;

namespace Script.ECS.Component
{
    public struct Enemy : IComponentData
    {
        public int Health;
    }
}