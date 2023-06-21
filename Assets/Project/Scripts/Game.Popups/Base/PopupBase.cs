using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Game.Popups.Base
{
    public class PopupBase : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject Root { get; private set; }

        protected readonly ISubject<Unit> CloseSubject = new Subject<Unit>();

        private readonly ISubject<Unit> showSubject = new Subject<Unit>();
        private readonly ISubject<Unit> hideSubject = new Subject<Unit>();

        public IObservable<Unit> OnClose => CloseSubject;
        public IObservable<Unit> OnShow => showSubject;
        public IObservable<Unit> OnHide => hideSubject;

        public void Initialize()
        {
        }

        public async UniTask Show()
        {
            await UniTask.NextFrame();
            Root.SetActive(true);

            showSubject.OnNext(Unit.Default);
        }

        public async UniTask Hide()
        {
            await UniTask.NextFrame();
            Destroy(gameObject);
            await Resources.UnloadUnusedAssets();

            hideSubject.OnNext(Unit.Default);
        }
    }
}