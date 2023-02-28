using System;
using Game.Engine.Data;
using UnityEngine;

namespace Game.Engine.Models
{
    [Serializable]
    public class BoardModel : IBoardModel
    {
        [field: SerializeField] public IntVector2 Dimensions { get; private set; }
        [field: SerializeField] public float GridSize { get; private set; }

        public BoardModel(IntVector2 dimensions, float gridSize)
        {
            Dimensions = dimensions;
            GridSize = gridSize;
        }
    }
}