using Grpc.Core;
using MagicOnion.API;
using MagicOnion.Client;
using ServerShared.Hub;
using ServerShared.MessagePackObject;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Debugger
{
    public class MoveTest : MonoBehaviour
    {
        private TransformAccessArray transforms;
        private Movement movement;

        private void Awake()
        {
            movement = GetComponent<Movement>();
            transforms = new TransformAccessArray(4);
        }

        private void Update()
        {
            var movementParameters = movement.parameters;
            var transformParameters = new NativeArray<TransformParameter>(movementParameters, Allocator.Temp);
            var transformJob = new TransformJob(transformParameters);
            var transformJobHandle = transformJob.Schedule(transforms);
            
            JobHandle.ScheduleBatchedJobs();
            transformJobHandle.Complete();
        }

        [BurstCompile(FloatPrecision.Low,FloatMode.Fast)]
        private readonly struct TransformJob : IJobParallelForTransform
        {
            private readonly NativeArray<TransformParameter> parameters;
            
            void IJobParallelForTransform.Execute(int index, TransformAccess transform)
            {
                transform.position = parameters[index].position;
                transform.rotation = parameters[index].rotation;
            }

            public TransformJob(NativeArray<TransformParameter> parameters)
            {
                this.parameters = parameters;
            }
        }
    }
}
