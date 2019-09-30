using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Script.ECS.System
{
    public class LifeTimeSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var lifeTimeJob = new LifeTimeJob
            {
                DeltaTime = Time.deltaTime,
                CommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            };

            var jobHandle = lifeTimeJob.Schedule(this, inputDeps);
            
            //JobからCommandBufferに書き込む場合、AddJobHandleForProducerを使用してそのJobをバッファーシステムの依存関係リストに注入する必要がある。
            endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }
        
        [BurstCompile]
        private struct LifeTimeJob : IJobForEachWithEntity<LifeTime>
        {
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, ref LifeTime lifeTime)
            {
                if (lifeTime.Value < 0.0f) 
                    CommandBuffer.DestroyEntity(index, entity);
                else
                    lifeTime.Value -= DeltaTime;
            }
        }
    }
}
