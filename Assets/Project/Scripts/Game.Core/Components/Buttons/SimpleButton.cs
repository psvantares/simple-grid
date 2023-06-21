using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Components.Buttons
{
    [RequireComponent(typeof(Button))]
    public class SimpleButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button Button { get; private set; }

        [field: SerializeField]
        public Image IconImage { get; private set; }

        private Tweener clickTweener;

        private readonly ISubject<Unit> click = new Subject<Unit>();
        private readonly CompositeDisposable disposable = new();

        public IObservable<Unit> OnClick => click;

        private void Start()
        {
            Button.OnClickAsObservable().Subscribe(Click).AddTo(disposable);
        }

        private void OnDestroy()
        {
            clickTweener?.Kill();
            disposable.Clear();
        }

        // Events

        private void Click(Unit unit)
        {
            IconImage.transform.localScale = Vector3.one;

            clickTweener.Kill();
            clickTweener = IconImage.transform
                .DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.25f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => click.OnNext(Unit.Default));
        }
    }
}