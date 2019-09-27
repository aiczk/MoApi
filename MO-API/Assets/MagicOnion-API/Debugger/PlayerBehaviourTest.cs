using System;
using MagicOnion.API;
using MagicOnion.API.Job;
using ServerShared.MessagePackObject;
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
    }
}
