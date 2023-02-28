using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Popups.Base;
using UnityEngine;

namespace Game.Popups
{
    public class PopupManager : MonoBehaviour, IPopupManager<PopupBase>
    {
        [field: SerializeField] public Transform Container { get; private set; }

        private const string RootDirectory = "Popups";

        private readonly Dictionary<PopupType, PopupBase> _popups = new();

        public async UniTask<PopupBase> Show(PopupType popupType)
        {
            if (_popups.ContainsKey(popupType))
            {
                return _popups[popupType];
            }

            var asset = await GetAssetAsync<PopupBase>(popupType);
            var popup = Instantiate(asset, Container, false);
            popup.Initialize();
            await popup.Show();

            _popups.Add(popupType, popup);

            return popup;
        }

        public async UniTask Hide(PopupType popupType)
        {
            if (_popups.ContainsKey(popupType))
            {
                var popup = _popups[popupType];
                await popup.Hide();
                _popups.Remove(popupType);
            }
        }

        private static async UniTask<T> GetAssetAsync<T>(PopupType popupType) where T : PopupBase
        {
            var path = $"{RootDirectory}/{popupType.ToString()}";
            var asset = await Resources.LoadAsync<T>(path).ToUniTask();
            return (T)asset;
        }
    }
}