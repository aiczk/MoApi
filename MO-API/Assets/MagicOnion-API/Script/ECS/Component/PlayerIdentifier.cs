﻿using Unity.Entities;

namespace Script.ECS.Component
{
    public struct PlayerIdentifier : IComponentData
    {
        public int Index;
    }
}