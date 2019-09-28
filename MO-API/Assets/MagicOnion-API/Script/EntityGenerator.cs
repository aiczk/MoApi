using _Script.Application.Utility.Base;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Script
{
    public class EntityGenerator : SingletonMonoBehaviour<EntityGenerator>
    {
        [SerializeField] 
        private RenderMesh renderMesh;
        
        private EntityManager manager;
        private EntityArchetype archetype;
        private Entity sharedEntity;

        private void Awake()
        {
            manager = World.Active.GetOrCreateManager<EntityManager>();
            archetype = manager.CreateArchetype
            (
                ComponentType.Create<LocalToWorld>(),
                ComponentType.Create<Position>(),
                ComponentType.ReadOnly<RenderMesh>()
            );

            sharedEntity = manager.CreateEntity(archetype);
            manager.SetSharedComponentData(sharedEntity, renderMesh); 
        }
    }
}
