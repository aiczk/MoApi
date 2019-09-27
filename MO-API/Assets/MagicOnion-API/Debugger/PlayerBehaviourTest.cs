using System;
using MagicOnion.API;
using MagicOnion.API.Job;
using UniRx;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Debugger
{
    //todo nativeQueueで弾の管理をする。
    //ECS使ってもいいかも...?
    public class PlayerBehaviourTest : MonoBehaviour
    {
        private PlayerBehaviour playerBehaviour;
        
        private void Awake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();

            playerBehaviour
                .DropAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .GetAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .ShotAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .ChangeEquipmentAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .RegisterWeaponAsObservable
                .Subscribe(x =>
                {
                    
                });
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private readonly struct BehaviourJob : IJobParallelFor
        {
            [ReadOnly]
            private readonly NativeArray<PlayerBehaviourData> parameters;
            
            void IJobParallelFor.Execute(int index)
            {
                
            }

            public BehaviourJob(NativeArray<PlayerBehaviourData> parameters) => this.parameters = parameters;
        }
    }
}
