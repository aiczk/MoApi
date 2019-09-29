using System;
using MagicOnion.API;
using UniRx;
using Unity.Collections;
using UnityEngine;

namespace Debugger
{
    //todo nativeQueueで弾の管理をする。 ECS使ってもいいかも?
    //todo 置かれたアイテムの管理はどうするか NativeList?
    //武器の変更/装備の変更/射撃/リロードはAnimation用。
    public class PlayerBehaviourTest : MonoBehaviour
    {
        private PlayerBehaviour playerBehaviour;
        //private NativeList<int> items = new NativeList<int>(Allocator.Persistent);
        
        private void Awake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();

            playerBehaviour
                .DropAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"アイテムID {x.RandomIndex.ToString()}が置かれました。");
                    //items.Add(x.RandomIndex);
                });

            playerBehaviour
                .GetAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"アイテムID {x.RandomIndex.ToString()}が取得されました。");
                    //items.RemoveAtSwapBack(x.RandomIndex);
                });

            playerBehaviour
                .ShotAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"プレイヤー {x.Index.ToString()}が射撃しました。");
                });

            playerBehaviour
                .ChangeEquipmentAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"プレイヤー {x.Index.ToString()}が装備を変更しました。");
                });

            playerBehaviour
                .RegisterWeaponAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"プレイヤー {x.Index.ToString()}が武器を変更しました。");
                });

            playerBehaviour
                .ReloadAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"プレイヤー {x.ToString()}がリロードしました。");
                });
        }

        private void OnDestroy()
        {
            //items.Dispose();
        }
    }
}
