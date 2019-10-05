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
using Collision = Script.ECS.Component.Collision;
using Physics = Script.ECS.Component.Physics;
using Random = UnityEngine.Random;

namespace MagicOnion.API.ECS
{
    public class EntityGenerator : MonoBehaviour
    {
        [SerializeField] private RenderMesh bulletMesh = default;
        
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
            
            //for (var i = 0; i < 4; i++) 
            //    GeneratePlayerScore(i);
            
            //GeneratePhysics();
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

        public void GenerateBullet(float3 position, float3 direction, WeaponType currentWeapon, float lifeTime)
        {
            var instance = manager.Instantiate(sharedEntity);
            
            manager.SetComponentData(instance, new Translation {Value = position});
            manager.SetComponentData(instance, new Rotation {Value = quaternion.identity});
            manager.AddComponentData(instance, new LifeTime {Value = lifeTime});
            manager.AddComponentData(instance, new Bullet
            {
                Power = WeaponParser(currentWeapon),
                IsTouch = false
            });
            manager.AddComponentData(instance, new Physics
            {
                CachedPosition = position,
                CurrentPosition = position,
                
                //よくある弾道にする  時間をphysicsにキャッシュさせて重力をかける
                Force = Vector3.Normalize(direction) * 9.8f,
                Mass = 1f
            });
        }

        //todo 誰が敵を殺したかでスコアを増減させる。
        private void GeneratePlayerScore(int index)
        {
            var playerArchetype = manager.CreateArchetype(ComponentType.ReadWrite<Player>());
            var entity = manager.CreateEntity(playerArchetype);

            manager.SetComponentData(entity, new Player {Index = index, KillCount = 0});
        }

        private void GeneratePhysics()
        {
            var instance = manager.Instantiate(sharedEntity);

            manager.SetComponentData(instance, new Translation {Value = float3.zero});
            manager.AddComponentData(instance, new Physics
            {
                CurrentPosition = Vector3.zero,
                CachedPosition = Vector3.zero,
                Force = Vector3.up * 9.8f,
                Mass = 3f
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
