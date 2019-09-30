using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

namespace Script.ECS.System
{
    public class BRGTEST : MonoBehaviour
    {
        private BatchRendererGroup batchRendererGroup;

        private void Awake()
        {
            batchRendererGroup = new BatchRendererGroup(OnPerformCulling);
        }

        private JobHandle OnPerformCulling(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext)
        {
            throw new NotImplementedException();
        }
    }
}
