using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Physics = Script.ECS.Component.Physics;

namespace Script.ECS.System
{
    public class PhysicsSystem : JobComponentSystem
    {
        private EntityQuery entityQuery;
        
        protected override void OnCreate()
        {
            entityQuery = GetEntityQuery
            (
                ComponentType.ReadWrite<Physics>(),
                ComponentType.ReadWrite<Translation>()
            );
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var physicsJob = new PhysicJob
            {
                BulletArchetypeChunkComponentType = GetArchetypeChunkComponentType<Bullet>(),
                PhysicsArchetypeChunkComponentType = GetArchetypeChunkComponentType<Physics>(),
                TranslationArchetypeChunkComponentType = GetArchetypeChunkComponentType<Translation>(),
                DeltaTime = Time.deltaTime,
                Time = Time.realtimeSinceStartup
            };

            var physicHandle = physicsJob.Schedule(entityQuery,inputDeps);
            return physicHandle;
        }
        
        [BurstCompile]
        private struct PhysicJob : IJobChunk
        {
            public ArchetypeChunkComponentType<Physics> PhysicsArchetypeChunkComponentType;
            public ArchetypeChunkComponentType<Bullet> BulletArchetypeChunkComponentType;
            [WriteOnly] public ArchetypeChunkComponentType<Translation> TranslationArchetypeChunkComponentType;
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public float Time;

            public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var physicsArray = chunk.GetNativeArray(PhysicsArchetypeChunkComponentType);
                var translationArray = chunk.GetNativeArray(TranslationArchetypeChunkComponentType);
                var bulletArray = chunk.GetNativeArray(BulletArchetypeChunkComponentType);
                
                var physicsPtr = (Physics*) physicsArray.GetUnsafePtr();
                var translationPtr = (Translation*) translationArray.GetUnsafePtr();
                var bulletPtr = (Bullet*) bulletArray.GetUnsafePtr();
                
                for (var i = 0; i < physicsArray.Length; ++i, ++physicsPtr, ++translationPtr, ++bulletPtr)
                {
                    var tempPosition = physicsPtr->CurrentPosition;
                    var dif = physicsPtr->CurrentPosition - physicsPtr->CachedPosition;
                    
                    translationPtr->Value =
                        physicsPtr->CurrentPosition +=
                             dif + physicsPtr->Force * DeltaTime * DeltaTime / physicsPtr->Mass;
                    
                    physicsPtr->CachedPosition = tempPosition;

                    bulletPtr->FallUntil = Time;
                }
            }
        }
    }
}