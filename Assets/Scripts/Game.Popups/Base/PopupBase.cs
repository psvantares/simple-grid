using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Game.Popups.Base
{
    public class PopupBase : MonoBehaviour
    {
        [field: SerializeField] public GameObject Root { get; private set; }

        protected readonly ISubject<Unit> CloseSubject = new Subject<Unit>();

        private readonly ISubject<Unit> _showSubject = new Subject<Unit>();
        private readonly ISubject<Unit> _hideSubject = new Subject<Unit>();

        public IObservable<Unit> OnClose => CloseSubject;
        public IObservable<Unit> OnShow => _showSubject;
        public IObservable<Unit> OnHide => _hideSubject;

        public void Initialize()
        {
        }

        public async UniTask Show()
        {
            await UniTask.NextFrame();
            Root.SetActive(true);

            _showSubject.OnNext(Unit.Default);
        }

        public async UniTask Hide()
        {
            await UniTask.NextFrame();
            Destroy(gameObject);
            await Resources.UnloadUnusedAssets();

            _hideSubject.OnNext(Unit.Default);
        }
    }
}