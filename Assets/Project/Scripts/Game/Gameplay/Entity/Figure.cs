using Game.Engine.Board;
using Game.Engine.Data;
using UnityEngine;

namespace Game.Gameplay.Entity
{
    public class Figure : MonoBehaviour
    {
        [SerializeField]
        private FigureLevel[] levels;

        [SerializeField]
        private string figureName;

        [SerializeField]
        private IntVector2 dimensions;

        public FigureGhost FigureGhostPrefab => levels[0].FigureGhostPrefab;
        public IntVector2 Dimensions => dimensions;

        public int CurrentLevel { get; protected set; }
        public FigureLevel CurrentFigureLevel { get; protected set; }
        public IntVector2 GridPosition { get; private set; }
        public IBoard Board { get; private set; }

        public void Initialize(IBoard board, IntVector2 destination)
        {
            Board = board;
            GridPosition = destination;

            if (board != null)
            {
                var tr = transform;
                tr.position = Board.GridToWorld(destination, dimensions);
                tr.rotation = Board.Transform.rotation;
                board.Occupy(destination, dimensions);
            }

            SetLevel(0);
        }

        private void SetLevel(int level)
        {
            if (level < 0 || level >= levels.Length)
            {
                return;
            }

            CurrentLevel = level;

            if (CurrentFigureLevel != null)
            {
                Destroy(CurrentFigureLevel.gameObject);
            }

            CurrentFigureLevel = Instantiate(levels[CurrentLevel], transform);
            CurrentFigureLevel.Initialize(this);
        }
    }
}