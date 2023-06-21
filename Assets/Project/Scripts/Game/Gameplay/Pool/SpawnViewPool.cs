using Game.Gameplay.View;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Pool
{
    public class SpawnViewPool : ObjectPool<SpawnView>
    {
        private readonly SpawnView entity;
        private readonly Transform parent;

        public SpawnViewPool(SpawnView entity, Transform parent)
        {
            this.entity = entity;
            this.parent = parent;
        }

        public SpawnView Rent(Transform parent)
        {
            var item = Rent();
            item.transform.SetParent(parent, false);

            return item;
        }

        protected override SpawnView CreateInstance()
        {
            var instance = Object.Instantiate(entity, parent, false);
            return instance;
        }
    }
}