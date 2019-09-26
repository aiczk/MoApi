using System;
using MagicOnion.API;
using UnityEngine;

namespace Debugger
{
    public class PlayerBehaviourTest : MonoBehaviour
    {
        private PlayerBehaviour playerBehaviour;
        
        private void Awake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
    }
}
