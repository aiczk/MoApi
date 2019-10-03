using System;
using System.Runtime.CompilerServices;
using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
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
                ComponentType.ReadWrite<Translation>(),
                ComponentType.Exclude<Bullet>()
            );
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var physicsArchetype = GetArchetypeChunkComponentType<Physics>();
            
            var physicsJob = new PhysicJob
            {
                PhysicsArchetypeChunkComponentType = physicsArchetype,
                TranslationArchetypeChunkComponentType = GetArchetypeChunkComponentType<Translation>(),
                DeltaTime = Time.deltaTime
            };
            
            var forceJob = new ForceJob
            {
                PhysicsArchetypeChunkComponentType = physicsArchetype
            };
            
            var physicHandle = physicsJob.Schedule(entityQuery,inputDeps);
            var forceHandle = forceJob.Schedule(entityQuery, physicHandle);
            
            return forceHandle;
        }
        
        [BurstCompile]
        private struct PhysicJob : IJobChunk
        {
            public ArchetypeChunkComponentType<Physics> PhysicsArchetypeChunkComponentType;
            public ArchetypeChunkComponentType<Translation> TranslationArchetypeChunkComponentType;
            [ReadOnly] public float DeltaTime;

            public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var physicsArray = chunk.GetNativeArray(PhysicsArchetypeChunkComponentType);
                var translationArray = chunk.GetNativeArray(TranslationArchetypeChunkComponentType);

                var physicsPtr = (Physics*) physicsArray.GetUnsafePtr();
                var translationPtr = (Translation*) translationArray.GetUnsafePtr();

                for (var i = 0; i < physicsArray.Length; ++i, ++physicsPtr, ++translationPtr)
                {
                    var tempPosition = physicsPtr->CurrentPosition;
                    var calc = physicsPtr->CurrentPosition - physicsPtr->CachedPosition;
                    
                    translationPtr->Value =
                        physicsPtr->CurrentPosition +=
                             calc + physicsPtr->Force * DeltaTime * DeltaTime / physicsPtr->Mass;

                    physicsPtr->CachedPosition = tempPosition;
                }
            }
        }

        [BurstCompile]
        private struct ForceJob : IJobChunk
        {
            public ArchetypeChunkComponentType<Physics> PhysicsArchetypeChunkComponentType;
            
            public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var physicsArray = chunk.GetNativeArray(PhysicsArchetypeChunkComponentType);
                var physicsPtr = (Physics*) physicsArray.GetUnsafePtr();

                for (var i = 0; i < physicsArray.Length; ++i, ++physicsPtr)
                {
                    if (math.all(physicsPtr->Force <= 0f))
                    {
                        physicsPtr->Force = float3.zero;
                        physicsPtr->CachedPosition = physicsPtr->CurrentPosition;
                        return;
                    }
                    
                    physicsPtr->Force -= 0.01f;
                }
            }
        }
    }
}