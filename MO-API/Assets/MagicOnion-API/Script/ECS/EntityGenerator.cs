using Script.ECS.Component;
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
        private RenderMesh renderMesh = default;
        
        private EntityManager manager;
        private EntityArchetype archetype;
        private Entity sharedEntity;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitEcs()
        {
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            PlayerLoopHelper.Initialize(ref playerLoop);
        }

        private void Start()
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
            manager.SetSharedComponentData(sharedEntity, renderMesh);
        }

        private void GenerateBullet(float lifeTime = 1f)
        {
            var instance = manager.Instantiate(sharedEntity);
                
            manager.SetComponentData(instance, new Translation {Value = (float3)Random.insideUnitSphere * 5f});
            manager.SetComponentData(instance, new Rotation {Value = Random.rotation});
            manager.AddComponentData(instance, new LifeTime {Value = lifeTime});
            manager.AddComponentData(instance, new Bullet {Direction = Random.rotation.eulerAngles.normalized});
        }
    }
}
