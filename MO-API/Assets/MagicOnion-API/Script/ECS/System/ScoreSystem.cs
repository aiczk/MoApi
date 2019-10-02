using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using NotImplementedException = System.NotImplementedException;

namespace Script.ECS.System
{
    public class ScoreSystem : JobComponentSystem
    {
        private EntityQuery deadQuery;
        
        protected override void OnCreate()
        {
            deadQuery = GetEntityQuery(ComponentType.ReadOnly<Dead>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var killCount = new NativeArray<int>(4, Allocator.TempJob);
            
            var writeKillCountJob = new WriteCountJob
            {
                DeadArchetypeChunkComponentType = GetArchetypeChunkComponentType<Dead>(true),
                KillCounts = killCount
            };
            
            var readKillCountJob = new ReadCountJob
            {
                ReadCount = killCount
            };

            var writeHandle = writeKillCountJob.Schedule(deadQuery);
            var readHandle = readKillCountJob.Schedule(this, writeHandle);
            JobHandle.ScheduleBatchedJobs();
            return readHandle;
        }

        [BurstCompile]
        private struct WriteCountJob : IJobChunk
        {
            [ReadOnly] public ArchetypeChunkComponentType<Dead> DeadArchetypeChunkComponentType;
            [WriteOnly] public NativeArray<int> KillCounts;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var deadArray = chunk.GetNativeArray(DeadArchetypeChunkComponentType);

                for (var i = 0; i < deadArray.Length; i++)
                {
                    var killedBy = deadArray[i].WhoKilled;
                    KillCounts[killedBy]++;
                }
            }
        }

        [BurstCompile]
        private struct ReadCountJob : IJobForEach<PlayerIdentifier>
        {
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<int> ReadCount;
            
            public void Execute(ref PlayerIdentifier playerIdentifier)
            {
                var index = playerIdentifier.Index;
                playerIdentifier.KillCount += ReadCount[index];
            }
        }
    }
}
