using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Popups.Base;
using UnityEngine;

namespace Game.Popups
{
    public class PopupManager : MonoBehaviour, IPopupManager<PopupBase>
    {
        [field: SerializeField]
        public Transform Container { get; private set; }

        private const string ROOT_DIRECTORY = "Popups";

        private readonly Dictionary<PopupType, PopupBase> popups = new();

        public async UniTask<PopupBase> Show(PopupType popupType)
        {
            if (popups.TryGetValue(popupType, out var show))
            {
                return show;
            }

            var asset = await GetAssetAsync<PopupBase>(popupType);
            var popup = Instantiate(asset, Container, false);
            popup.Initialize();
            await popup.Show();

            popups.Add(popupType, popup);

            return popup;
        }

        public async UniTask Hide(PopupType popupType)
        {
            if (popups.ContainsKey(popupType))
            {
                var popup = popups[popupType];
                await popup.Hide();
                popups.Remove(popupType);
            }
        }

        private static async UniTask<T> GetAssetAsync<T>(PopupType popupType) where T : PopupBase
        {
            var path = $"{ROOT_DIRECTORY}/{popupType.ToString()}";
            var asset = await Resources.LoadAsync<T>(path).ToUniTask();
            return (T)asset;
        }
    }
}