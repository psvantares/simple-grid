using Game.Engine.Data;
using Game.Engine.Models;
using UnityEngine;

namespace Game.Engine.Board
{
    public interface IBoard
    {
        void Initialize(IBoardModel boardModel);
        Transform Transform { get; }
        IntVector2 WorldToGrid(Vector3 worldPosition, IntVector2 sizeOffset);
        Vector3 GridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset);
        Bounds GetBounds();
        BoardStatus Fits(IntVector2 gridPosition, IntVector2 size);
        void Occupy(IntVector2 gridPosition, IntVector2 size);
        void Clear(IntVector2 gridPosition, IntVector2 size);
    }

    public static class BoardExtensions
    {
        public static Vector3 Snap(this IBoard board, Vector3 worldPosition, IntVector2 sizeOffset)
        {
            return board.GridToWorld(board.WorldToGrid(worldPosition, sizeOffset), sizeOffset);
        }
    }
}