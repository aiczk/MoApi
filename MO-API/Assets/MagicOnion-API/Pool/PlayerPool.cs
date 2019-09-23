using Info;
using UniRx.Toolkit;
using UnityEngine;

namespace Pool
{
    public class PlayerPool : ObjectPool<IdentifierComponent>
    {
        private readonly IdentifierComponent prefab;
        
        public PlayerPool(IdentifierComponent prefab) => this.prefab = prefab;
        protected override IdentifierComponent CreateInstance() => Object.Instantiate(prefab);
    }
}
