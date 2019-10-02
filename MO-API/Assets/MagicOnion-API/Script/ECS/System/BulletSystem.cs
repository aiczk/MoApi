using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.ECS.System
{
    public class BulletSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var bulletJob = new BulletJob
            {
                CommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            };

            var bulletJobHandle = bulletJob.Schedule(this, inputDeps);
            endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(bulletJobHandle);
            return bulletJobHandle;
        }
        
        [BurstCompile]
        private struct BulletJob : IJobForEachWithEntity<Bullet,Translation>
        {
            [ReadOnly] public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Bullet bullet, [WriteOnly] ref Translation translation)
            {
                if (bullet.IsTouch)
                {
                    CommandBuffer.DestroyEntity(index, entity);
                    return;
                }
                
                translation.Value += bullet.Direction;
            }
        }
    }
}
