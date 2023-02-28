using UnityEngine;

namespace Game.Gameplay.Entity
{
    public class FigureLevel : MonoBehaviour
    {
        [SerializeField] private FigureGhost _figureGhostPrefab;

        private Figure _figure;

        public FigureGhost FigureGhostPrefab => _figureGhostPrefab;

        public void Initialize(Figure data)
        {
            _figure = data;
        }
    }
}