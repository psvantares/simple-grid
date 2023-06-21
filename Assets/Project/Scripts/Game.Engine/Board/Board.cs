using System;
using Cysharp.Threading.Tasks;
using Game.Engine.Data;
using Game.Engine.Models;
using UnityEngine;

namespace Game.Engine.Board
{
    [RequireComponent(typeof(BoxCollider))]
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField]
        private GameObject tilesParent;

        [SerializeField]
        private BoardTile boardTilePrefab;

        [SerializeField]
        private IntVector2 dimensions;

        [SerializeField]
        private float gridSize = 1;

        private float invGridSize;
        private bool[,] availableCells;
        private BoardTile[,] tiles;

        public Transform Transform => transform;

        public void Initialize(IBoardModel boardModel)
        {
            dimensions = boardModel.Dimensions;
            gridSize = boardModel.GridSize;
            availableCells = new bool[dimensions.X, dimensions.Y];
            invGridSize = 1 / gridSize;

            ResizeCollider();
            Setup().Forget();
        }

        public IntVector2 WorldToGrid(Vector3 worldLocation, IntVector2 sizeOffset)
        {
            var localLocation = transform.InverseTransformPoint(worldLocation);

            localLocation *= invGridSize;

            var offset = new Vector3(sizeOffset.X * 0.5f, 0.0f, sizeOffset.Y * 0.5f);
            localLocation -= offset;

            var xPos = Mathf.RoundToInt(localLocation.x);
            var yPos = Mathf.RoundToInt(localLocation.z);

            return new IntVector2(xPos, yPos);
        }

        public Vector3 GridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset)
        {
            var localPos = new Vector3(gridPosition.X + (sizeOffset.X * 0.5f), 0, gridPosition.Y + (sizeOffset.Y * 0.5f)) * gridSize;
            return transform.TransformPoint(localPos);
        }

        public Bounds GetBounds()
        {
            var boxCollider = GetComponent<BoxCollider>();
            var center = boxCollider.center;
            var size = boxCollider.size;
            var bounds = new Bounds(center, size);

            return bounds;
        }

        public BoardStatus Fits(IntVector2 gridPosition, IntVector2 size)
        {
            if ((size.X > dimensions.X) || (size.Y > dimensions.Y))
            {
                return BoardStatus.OutOfBounds;
            }

            var extents = gridPosition + size;

            if ((gridPosition.X < 0) || (gridPosition.Y < 0) || (extents.X > dimensions.X) || (extents.Y > dimensions.Y))
            {
                return BoardStatus.OutOfBounds;
            }

            for (var y = gridPosition.Y; y < extents.Y; y++)
            {
                for (var x = gridPosition.X; x < extents.X; x++)
                {
                    if (availableCells[x, y])
                    {
                        return BoardStatus.Overlaps;
                    }
                }
            }

            return BoardStatus.Fits;
        }

        public void Occupy(IntVector2 gridPosition, IntVector2 size)
        {
            var extents = gridPosition + size;

            if ((size.X > dimensions.X) || (size.Y > dimensions.Y))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Given dimensions do not fit in our grid");
            }

            if ((gridPosition.X < 0) || (gridPosition.Y < 0) || (extents.X > dimensions.X) || (extents.Y > dimensions.Y))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Given footprint is out of range of our grid");
            }

            for (var y = gridPosition.Y; y < extents.Y; y++)
            {
                for (var x = gridPosition.X; x < extents.X; x++)
                {
                    availableCells[x, y] = true;

                    if (tiles != null && tiles[x, y] != null)
                    {
                        tiles[x, y].SetState(BoardTileStatus.Filled);
                    }
                }
            }
        }

        public void Clear(IntVector2 gridPosition, IntVector2 size)
        {
            var extents = gridPosition + size;

            if ((size.X > dimensions.X) || (size.Y > dimensions.Y))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Given dimensions do not fit in our grid");
            }

            if ((gridPosition.X < 0) || (gridPosition.Y < 0) || (extents.X > dimensions.X) || (extents.Y > dimensions.Y))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Given footprint is out of range of our grid");
            }

            for (var y = gridPosition.Y; y < extents.Y; y++)
            {
                for (var x = gridPosition.X; x < extents.X; x++)
                {
                    availableCells[x, y] = false;

                    if (tiles != null && tiles[x, y] != null)
                    {
                        tiles[x, y].SetState(BoardTileStatus.Empty);
                    }
                }
            }
        }

        private void ResizeCollider()
        {
            var boxCollider = GetComponent<BoxCollider>();
            var size = new Vector3(dimensions.X, 0, dimensions.Y) * gridSize;

            boxCollider.size = size;
            boxCollider.center = size * 0.5f;
        }

        private async UniTask Setup()
        {
            tiles = new BoardTile[dimensions.X, dimensions.Y];

            for (var y = 0; y < dimensions.Y; y++)
            {
                for (var x = 0; x < dimensions.X; x++)
                {
                    var targetPos = GridToWorld(new IntVector2(x, y), new IntVector2(1, 1));
                    targetPos.y += 0.01f;
                    var tile = Instantiate(boardTilePrefab, tilesParent.transform, true);
                    var tr = tile.transform;
                    tr.position = targetPos;
                    tr.localRotation = Quaternion.identity;

                    tiles[x, y] = tile;

                    tile.SetState(BoardTileStatus.Empty);
                    await tile.DoShow().ToUniTask();
                    tile.ResizeCollider();
                }
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gridSize <= 0)
            {
                Debug.LogError("Negative or zero grid size is invalid");
                gridSize = 1;
            }

            if (dimensions.X <= 0 || dimensions.Y <= 0)
            {
                Debug.LogError("Negative or zero grid dimensions are invalid");
                dimensions = new IntVector2(Mathf.Max(dimensions.X, 1), Mathf.Max(dimensions.Y, 1));
            }

            ResizeCollider();
        }

        private void OnDrawGizmos()
        {
            var prevCol = Gizmos.color;
            Gizmos.color = Color.cyan;

            var originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (var y = 0; y < dimensions.Y; y++)
            {
                for (var x = 0; x < dimensions.X; x++)
                {
                    var position = new Vector3((x + 0.5f) * gridSize, 0, (y + 0.5f) * gridSize);
                    Gizmos.DrawWireCube(position, new Vector3(gridSize, 0, gridSize));
                }
            }

            Gizmos.matrix = originalMatrix;
            Gizmos.color = prevCol;

            var center = transform.TransformPoint(new Vector3(gridSize * dimensions.X * 0.5f, 1, gridSize * dimensions.Y * 0.5f));
            Gizmos.DrawIcon(center, "build_zone.png", true);
        }
#endif
    }
}