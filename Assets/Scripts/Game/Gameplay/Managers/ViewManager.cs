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
        [field: SerializeField] public GameplayView GameplayView { get; private set; }
        [field: SerializeField] public SidebarView SidebarView { get; private set; }

        private PoolManager _poolManager;
        private PopupManager _popupManager;

        private readonly List<IController> _controllers = new();

        public void Initialize(ILevelModel levelModel, GameplayAssets gameplayAssets, PoolManager poolManager, PopupManager popupManager)
        {
            _poolManager = poolManager;
            _popupManager = popupManager;

            _controllers.Add(new SidebarController(gameplayAssets, poolManager.SpawnViewPool, levelModel.SpawnButtonData));

            Subscribes();
        }

        public void Clear()
        {
            _controllers.ForEach(x => x.Dispose());
            _controllers.Clear();
        }

        private void Subscribes()
        {
            GameplayView.SettingsButton.OnClick.Subscribe((_ => OnSettings().Forget()));
        }

        // Events

        private async UniTask OnSettings()
        {
            const PopupType popupType = PopupType.Settings;
            var popup = await _popupManager.Show(popupType);

            async void Close(Unit _)
            {
                await _popupManager.Hide(popupType);
            }

            popup.OnClose.Subscribe(Close);
        }
    }
}