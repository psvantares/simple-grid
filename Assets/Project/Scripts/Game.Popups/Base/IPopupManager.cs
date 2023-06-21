using Cysharp.Threading.Tasks;

namespace Game.Popups.Base
{
    public interface IPopupManager<T> where T : PopupBase
    {
        UniTask<T> Show(PopupType popupType);
        UniTask Hide(PopupType popupType);
    }
}