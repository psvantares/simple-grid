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
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image IconImage { get; private set; }

        private Tweener _clickTweener;

        private readonly ISubject<Unit> _click = new Subject<Unit>();
        private readonly CompositeDisposable _disposable = new();

        public IObservable<Unit> OnClick => _click;

        private void Start()
        {
            Button.OnClickAsObservable().Subscribe(Click).AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _clickTweener?.Kill();
            _disposable.Clear();
        }

        // Events

        private void Click(Unit unit)
        {
            IconImage.transform.localScale = Vector3.one;

            _clickTweener.Kill();
            _clickTweener = IconImage.transform
                .DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.25f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => _click.OnNext(Unit.Default));
        }
    }
}