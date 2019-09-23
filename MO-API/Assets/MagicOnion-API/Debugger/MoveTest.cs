using System;
using Grpc.Core;
using Info;
using MagicOnion.API;
using MagicOnion.Client;
using Pool;
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
        [SerializeField] private IdentifierComponent prefab = default;
        
        private TransformAccessArray transforms;
        private Movement movement;
        private PlayerPool playerPool;

        private void Awake()
        {
            movement = GetComponent<Movement>();
            playerPool = new PlayerPool(prefab);
            transforms = new TransformAccessArray(4, 2);

            for (var i = 0; i < 4; i++)
            {
                var rent = playerPool.Rent();
                rent.index = i;
                transforms.Add(rent.transform);
            }
        }

        private void OnDestroy()
        {
            transforms.Dispose();
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

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
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
