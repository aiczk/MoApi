using UnityEngine;

namespace MagicOnion.API.Job
{
    public readonly struct TransformParameter
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        
        public TransformParameter(in TransformParameter copy, Vector3 position)
        {
            Position = position;
            Rotation = copy.Rotation;
        }
        
        public TransformParameter(in TransformParameter copy, Quaternion rotation)
        {
            Position = copy.Position;
            Rotation = rotation;
        }
    }
}