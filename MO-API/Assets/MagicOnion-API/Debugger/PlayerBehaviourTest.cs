using System;
using MagicOnion.API;
using UniRx;
using UnityEngine;

namespace Debugger
{
    public class PlayerBehaviourTest : MonoBehaviour
    {
        private PlayerBehaviour playerBehaviour;
        
        private void Awake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();

            playerBehaviour
                .DropAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .GetAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .ShotAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .ChangeEquipmentAsObservable
                .Subscribe(x =>
                {
                    
                });

            playerBehaviour
                .RegisterWeaponAsObservable
                .Subscribe(x =>
                {
                    
                });
        }
    }
}
