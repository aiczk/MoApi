using Script.ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Script.ECS.System
{
    public class ScoreSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var killCount = new NativeArray<int>(4, Allocator.TempJob);
            
            var writeScoreJob = new WriteScoreJob
            {
                WriteCount = killCount
            };
            
            var readScoreJob = new ReadScoreJob
            {
                ReadCount = killCount
            };
            
            var writeHandle = writeScoreJob.Schedule(this);
            var readHandle = readScoreJob.Schedule(this, writeHandle);
            JobHandle.ScheduleBatchedJobs();
            return readHandle;
        }

        [BurstCompile]
        private struct WriteScoreJob : IJobForEach<Enemy, Dead>
        {
            [WriteOnly] public NativeArray<int> WriteCount;

            public void Execute([ReadOnly] ref Enemy enemy, [ReadOnly] ref Dead dead)
            {
                if (!enemy.IsDead)
                    return;

                var killedBy = dead.WhoKilled;
                WriteCount[killedBy]++;
            }
        }

        [BurstCompile]
        private struct ReadScoreJob : IJobForEach<PlayerIdentifier>
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
