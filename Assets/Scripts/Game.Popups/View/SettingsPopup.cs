using Game.Core.Components.Buttons;
using Game.Popups.Base;
using UniRx;
using UnityEngine;

namespace Game.Popups.View
{
    public class SettingsPopup : PopupBase
    {
        [field: SerializeField] public SimpleButton CloseButton { get; private set; }

        private void Start()
        {
            CloseButton.OnClick.Subscribe(CloseSubject.OnNext);
        }
    }
}