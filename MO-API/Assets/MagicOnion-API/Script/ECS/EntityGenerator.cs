using _Script.Application.Utility.Base;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicOnion.API.ECS
{
    public class EntityGenerator : SingletonMonoBehaviour<EntityGenerator>
    {
        [SerializeField] 
        private RenderMesh renderMesh = default;
        
        private EntityManager manager;
        private EntityArchetype archetype;
        private Entity sharedEntity;

        private void Start()
        {
            InitEntity();

            for (var i = 0; i < 10; i++)
            {
                var instantiate = manager.Instantiate(sharedEntity);

                manager.SetComponentData(instantiate, new Translation {Value = Random.insideUnitSphere * 5f});
                manager.SetComponentData(instantiate, new Rotation {Value = quaternion.identity});
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
                ComponentType.ReadOnly<RenderMesh>()
            ); 
            
            sharedEntity = manager.CreateEntity(archetype);
            manager.SetSharedComponentData(sharedEntity, renderMesh);
        }
    }
}
