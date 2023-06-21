using Game.Gameplay.Configs;
using Game.Gameplay.Pool;
using UnityEngine;

namespace Game.Gameplay.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public SpawnViewPool SpawnViewPool { get; private set; }

        public void Initialize(GameplayAssets gameplayAssets, ViewManager viewManager)
        {
            SpawnViewPool = new SpawnViewPool(gameplayAssets.SpawnView, viewManager.SidebarView.Container);
        }

        public void Clear()
        {
            SpawnViewPool.Clear();
        }
    }
}