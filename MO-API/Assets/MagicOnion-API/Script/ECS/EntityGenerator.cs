using Script.ECS.Component;
using UniRx.Async;
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

            for (var i = 0; i < 10; i++)
            {
                var instance = manager.Instantiate(sharedEntity);
                
                manager.SetComponentData(instance, new Translation {Value = (float3)Random.insideUnitSphere * 5f});
                manager.SetComponentData(instance, new Rotation {Value = Random.rotation});
                manager.SetComponentData(instance, new LifeTime{Value = 5f});
            }
        }

        private void InitEntity()
        {
            manager = World.Active.EntityManager;
            
            archetype = manager.CreateArchetype
            (
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<LocalToWorld>(),
                ComponentType.ReadOnly<RenderMesh>(),
                ComponentType.ReadWrite<LifeTime>()
            );
            
            sharedEntity = manager.CreateEntity(archetype);
            manager.SetSharedComponentData(sharedEntity, renderMesh);
        }
    }
}
