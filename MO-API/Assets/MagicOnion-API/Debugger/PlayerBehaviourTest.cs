using System;
using MagicOnion.API;
using MagicOnion.API.ECS;
using ServerShared.Utility;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace Debugger
{
    public class PlayerBehaviourTest : MonoBehaviour
    {
        private PlayerBehaviour playerBehaviour;
        private EntityGenerator entityGenerator;
        
        private void Awake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
            entityGenerator = GetComponent<EntityGenerator>();

            playerBehaviour
                .DropAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"アイテムID {x.RandomIndex.ToString()}が置かれました。");
                });

            playerBehaviour
                .GetAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"アイテムID {x.RandomIndex.ToString()}が取得されました。");
                });

            playerBehaviour
                .ShotAsObservable
                .Subscribe(x =>
                {
                    Debug.Log($"プレイヤー {x.Index.ToString()}が射撃しました。");
                    var forward = x.Rotation * Vector3.forward;
                    entityGenerator.GenerateBullet(x.Position, forward, WeaponType.Pistol, 5f);
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
