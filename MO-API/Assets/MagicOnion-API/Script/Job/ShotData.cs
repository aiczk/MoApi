using ServerShared.MessagePackObject;
using UnityEngine;

namespace MagicOnion.API.Job
{
    public readonly struct ShotData
    {
        public readonly int Index;
        public readonly Vector3 Direction;

        public ShotData(int index, Vector3 direction)
        {
            this.Index = index;
            this.Direction = direction;
        }

        public ShotData(ShotParameter copy)
        {
            Index = copy.Index;
            Direction = copy.Direction;
        }
    }
}