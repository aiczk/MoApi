using System.Runtime.CompilerServices;
using Script.ECS.Component;
using Unity.Mathematics;

namespace Script.ECS.Extension
{
    public static class PhysicsExtension
    {
        public static float3 CalculateVerlet(this in Physics physics,float deltaTime)
        {
            ref var phys = ref Unsafe.AsRef(physics);
            return phys.CurrentPosition - phys.CachedPosition + phys.Force * deltaTime * deltaTime / phys.Mass;
        }

    }
}
