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
                    Debug.Log($"{x.RandomIndex.ToString()}が置かれました。");
                });

            playerBehaviour
                .GetAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.RandomIndex.ToString()}が取得されました。");
                });

            playerBehaviour
                .ShotAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.Index.ToString()}が射撃しました。");
                });

            playerBehaviour
                .ChangeEquipmentAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.Index.ToString()}が装備を変更しました。");
                });

            playerBehaviour
                .RegisterWeaponAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"{x.Index.ToString()}が武器を変更しました。");
                });
        }
    }
}
