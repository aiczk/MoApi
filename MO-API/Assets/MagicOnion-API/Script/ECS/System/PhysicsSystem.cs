using System;
using System.Runtime.CompilerServices;
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
            entityQuery = GetEntityQuery(ComponentType.ReadWrite<Physics>(), ComponentType.ReadWrite<Translation>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var physicsJob = new PhysicJob
            {
                PhysicsArchetypeChunkComponentType = GetArchetypeChunkComponentType<Physics>(),
                TranslationArchetypeChunkComponentType = GetArchetypeChunkComponentType<Translation>(),
                DeltaTime = Time.deltaTime
            };

            return physicsJob.Schedule(entityQuery, inputDeps);
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
                    //todo test
                    if (physicsPtr->CurrentPosition.y <= 0f)
                    {
                        physicsPtr->Force = Vector3.up * 9.8f;
                    }
                    
                    if(physicsPtr->CurrentPosition.y > 5f)
                    {
                        physicsPtr->Force = Vector3.down * 9.8f;
                    }

                    var tempPosition = physicsPtr->CurrentPosition;
                    var calc = physicsPtr->CurrentPosition - physicsPtr->CachedPosition;
                    
                    translationPtr->Value =
                        physicsPtr->CurrentPosition +=
                             calc + physicsPtr->Force * DeltaTime * DeltaTime / physicsPtr->Mass;

                    physicsPtr->CachedPosition = tempPosition;
                }
            }
        }
    }
}