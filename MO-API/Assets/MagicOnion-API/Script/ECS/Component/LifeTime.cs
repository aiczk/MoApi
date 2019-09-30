using System;
using Unity.Entities;
using UnityEngine.Serialization;

namespace Script.ECS.Component
{
    [Serializable]
    public struct LifeTime : IComponentData
    {
        public float Value;
    }
}