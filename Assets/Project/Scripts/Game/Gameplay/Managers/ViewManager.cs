using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Gameplay.Configs;
using Game.Gameplay.Controllers;
using Game.Gameplay.Models;
using Game.Gameplay.View;
using Game.Popups;
using Game.Popups.Base;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Managers
{
    public class ViewManager : MonoBehaviour
    {
        [field: SerializeField]
        public GameplayView GameplayView { get; private set; }

        [field: SerializeField]
        public SidebarView SidebarView { get; private set; }

        private PoolManager poolManager;
        private PopupManager popupManager;

        private readonly List<IController> controllers = new();

        public void Initialize(ILevelModel levelModel, GameplayAssets gameplayAssets, PoolManager poolManager, PopupManager popupManager)
        {
            this.poolManager = poolManager;
            this.popupManager = popupManager;

            controllers.Add(new SidebarController(gameplayAssets, poolManager.SpawnViewPool, levelModel.SpawnButtonData));

            Subscribes();
        }

        public void Clear()
        {
            controllers.ForEach(x => x.Dispose());
            controllers.Clear();
        }

        private void Subscribes()
        {
            GameplayView.SettingsButton.OnClick.Subscribe((_ => OnSettings().Forget()));
        }

        // Events

        private async UniTask OnSettings()
        {
            const PopupType popupType = PopupType.Settings;
            var popup = await popupManager.Show(popupType);

            async void Close(Unit _)
            {
                await popupManager.Hide(popupType);
            }

            popup.OnClose.Subscribe(Close);
        }
    }
}