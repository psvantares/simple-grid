using Game.Gameplay.View;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Pool
{
    public class SpawnViewPool : ObjectPool<SpawnView>
    {
        private readonly SpawnView _entity;
        private readonly Transform _parent;

        public SpawnViewPool(SpawnView entity, Transform parent)
        {
            _entity = entity;
            _parent = parent;
        }

        public SpawnView Rent(Transform parent)
        {
            var item = Rent();
            item.transform.SetParent(parent, false);

            return item;
        }

        protected override SpawnView CreateInstance()
        {
            var instance = Object.Instantiate(_entity, _parent, false);
            return instance;
        }
    }
}