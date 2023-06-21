using System;
using Game.Gameplay.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Gameplay.View
{
    public class SpawnView : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField]
        private TMP_Text text;

        private bool isPointerDown;
        private Figure figure;

        public event Action<Figure> OnSelectFigure;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isPointerDown) return;

            isPointerDown = false;
            OnSelectFigure?.Invoke(figure);
        }

        public void Initialize(int level, Figure figure)
        {
            this.figure = figure;

            SetLevel(level);
        }

        private void SetLevel(int level)
        {
            text.text = $"{level}";
        }
    }
}