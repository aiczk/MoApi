using System;
using Script.ECS.Component;
using ServerShared.Utility;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicOnion.API.ECS
{
    public class EntityGenerator : MonoBehaviour
    {
        [SerializeField] 
        private RenderMesh bulletMesh = default;
        
        private EntityManager manager;
        private EntityArchetype archetype;
        private Entity sharedEntity;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitEcs()
        {
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            PlayerLoopHelper.Initialize(ref playerLoop);
        }

        private void Awake()
        {
            InitEntity();
            
            this.UpdateAsObservable()
                .Subscribe(x =>
                {
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        GenerateBullet();
                    }
                });

            for (var i = 0; i < 4; i++) 
                GeneratePlayerScore(i);
        }

        private void InitEntity()
        {
            manager = World.Active.EntityManager;

            archetype = manager.CreateArchetype
            (
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<LocalToWorld>(), 
                ComponentType.ReadOnly<RenderMesh>()
            );
            
            sharedEntity = manager.CreateEntity(archetype);
            manager.SetSharedComponentData(sharedEntity, bulletMesh);
        }

        private void GenerateBullet(float lifeTime = 1f)
        {
            var instance = manager.Instantiate(sharedEntity);
                
            manager.SetComponentData(instance, new Translation {Value = (float3)Random.insideUnitSphere * 5f});
            manager.SetComponentData(instance, new Rotation {Value = Random.rotation});
            manager.AddComponentData(instance, new LifeTime {Value = lifeTime});
            manager.AddComponentData(instance, new Bullet
            {
                Direction = Random.rotation.eulerAngles.normalized,
                Power = WeaponParser(WeaponType.Rifle),
                IsTouch = false
            });
        }

        //todo 誰が敵を殺したかでスコアを増減させる。
        private void GeneratePlayerScore(int index)
        {
            var playerArchetype = manager.CreateArchetype(ComponentType.ReadWrite<PlayerIdentifier>());
            var entity = manager.CreateEntity(playerArchetype);

            manager.SetComponentData(entity, new PlayerIdentifier {Index = index, KillCount = 0});
        }

        private void GenerateEnemy()
        {
            var enemyArchetype = manager.CreateArchetype(ComponentType.ReadWrite<Enemy>());
            var entity = manager.CreateEntity(enemyArchetype);

            manager.SetComponentData(entity, new Enemy
            {
                Health = 100
            });
        }

        private static int WeaponParser(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    return 10;
                case WeaponType.Rifle:
                    return 30;
                case WeaponType.SMG:
                    return 15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }
    }
}
