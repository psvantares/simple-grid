using UnityEngine;

namespace Game.Gameplay.Entity
{
    public class FigureLevel : MonoBehaviour
    {
        [SerializeField]
        private FigureGhost figureGhostPrefab;

        private Figure figure;

        public FigureGhost FigureGhostPrefab => figureGhostPrefab;

        public void Initialize(Figure data)
        {
            figure = data;
        }
    }
}