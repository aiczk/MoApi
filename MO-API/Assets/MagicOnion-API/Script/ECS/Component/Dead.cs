using Unity.Entities;

namespace Script.ECS.Component
{
    public struct Dead : IComponentData
    {
        public int WhoKilled;
    }
}