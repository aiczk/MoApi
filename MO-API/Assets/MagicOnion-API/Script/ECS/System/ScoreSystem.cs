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
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var killCount = new NativeArray<int>(4, Allocator.TempJob);
            
            var writeKillCountJob = new WriteCountJob
            {
                WriteCount = killCount
            };
            
            var readKillCountJob = new ReadCountJob
            {
                ReadCount = killCount
            };

            var writeHandle = writeKillCountJob.Schedule(this);
            var readHandle = readKillCountJob.Schedule(this, writeHandle);
            JobHandle.ScheduleBatchedJobs();
            return readHandle;
        }

        [BurstCompile]
        private struct WriteCountJob : IJobForEach<Dead>
        {
            [WriteOnly] public NativeArray<int> WriteCount;
            
            public void Execute([ReadOnly] ref Dead dead)
            {
                var killedBy = dead.WhoKilled;
                WriteCount[killedBy]++;
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
