using System;
using Game.Gameplay.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Gameplay.View
{
    public class SpawnView : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _text;

        private bool _isPointerDown;
        private Figure _figure;

        public event Action<Figure> OnSelectFigure;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isPointerDown) return;

            _isPointerDown = false;
            OnSelectFigure?.Invoke(_figure);
        }

        public void Initialize(int level, Figure figure)
        {
            _figure = figure;

            SetLevel(level);
        }

        private void SetLevel(int level)
        {
            _text.text = $"{level}";
        }
    }
}