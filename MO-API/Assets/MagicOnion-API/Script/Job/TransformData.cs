using UnityEngine;

namespace MagicOnion.API.Job
{
    public readonly struct TransformData
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        
        public TransformData(in TransformData copy, Vector3 position)
        {
            Position = position;
            Rotation = copy.Rotation;
        }
        
        public TransformData(in TransformData copy, Quaternion rotation)
        {
            Position = copy.Position;
            Rotation = rotation;
        }
    }
}