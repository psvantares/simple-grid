using Game.Core.Components.Buttons;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class GameplayView : MonoBehaviour
    {
        [field: SerializeField] public SimpleButton SettingsButton { get; private set; }
    }
}