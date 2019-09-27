using Info;
using MagicOnion.API;
using ServerShared.MessagePackObject;
using UniRx;
using UnityEngine;

namespace Debugger
{
    public class PlayerController : MonoBehaviour
    {
        private Movement movement;
        private PlayerBehaviour behaviour;
        
        private PositionParameter positionParam = new PositionParameter();
        private RotationParameter rotationParam = new RotationParameter();

        private void Awake()
        {
            var system = GameObject.FindGameObjectWithTag("System_Online");
            movement = system.GetComponent<Movement>();
            behaviour = system.GetComponent<PlayerBehaviour>();
            
            var playerIndex = GetComponent<IdentifierComponent>().index;
            GetComponent<Renderer>().material.color = Color.red;

            positionParam.Index = playerIndex;
            rotationParam.Index = playerIndex;
            
            transform
                .ObserveEveryValueChanged(x => x.position)
                .Skip(1)
                .Subscribe(async position =>
                {
                    positionParam.Position = position;
                    await movement.Move(positionParam);
                });

            transform
                .ObserveEveryValueChanged(x => x.rotation)
                .Skip(1)
                .Subscribe(async rotation =>
                {
                    rotationParam.Rotation = rotation;
                    await movement.Rotation(rotationParam);
                });
        }
    }
}
