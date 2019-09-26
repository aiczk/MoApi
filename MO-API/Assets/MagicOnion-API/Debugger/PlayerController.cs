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
        private PositionParameter positionParam = new PositionParameter();
        private RotationParameter rotationParam = new RotationParameter();

        private void Awake()
        {
            movement = GameObject.FindGameObjectWithTag("System_Online").GetComponent<Movement>();
            var playerIndex = GetComponent<IdentifierComponent>().index;
            GetComponent<Renderer>().material.color = Color.red;

            positionParam.index = playerIndex;
            rotationParam.index = playerIndex;
            
            transform
                .ObserveEveryValueChanged(x => x.position)
                .Subscribe(async position =>
                {
                    positionParam.position = position;
                    await movement.Move(positionParam);
                });

            transform
                .ObserveEveryValueChanged(x => x.rotation)
                .Subscribe(async rotation =>
                {
                    rotationParam.rotation = rotation;
                    await movement.Rotation(rotationParam);
                });
        }
    }
}
