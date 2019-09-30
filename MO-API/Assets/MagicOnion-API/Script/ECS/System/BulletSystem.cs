using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Script.ECS.System
{
    public class BulletSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var bulletJob = new BulletJob();
            return bulletJob.Schedule(this, inputDeps);
        }
        
        [BurstCompile]
        private struct BulletJob : IJobForEach<Bullet,Translation>
        {
            public void Execute([ReadOnly] ref Bullet bullet, [WriteOnly] ref Translation translation)
            {
                translation.Value += bullet.Direction;
            }
        }
    }
}
