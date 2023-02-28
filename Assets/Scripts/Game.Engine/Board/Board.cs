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
        [SerializeField] private GameObject _tilesParent;
        [SerializeField] private BoardTile _boardTilePrefab;
        [SerializeField] private IntVector2 _dimensions;
        [SerializeField] private float _gridSize = 1;

        private float _invGridSize;
        private bool[,] _availableCells;
        private BoardTile[,] _tiles;

        public Transform Transform => transform;

        public void Initialize(IBoardModel boardModel)
        {
            _dimensions = boardModel.Dimensions;
            _gridSize = boardModel.GridSize;
            _availableCells = new bool[_dimensions.x, _dimensions.y];
            _invGridSize = 1 / _gridSize;

            ResizeCollider();
            Setup().Forget();
        }

        public IntVector2 WorldToGrid(Vector3 worldLocation, IntVector2 sizeOffset)
        {
            var localLocation = transform.InverseTransformPoint(worldLocation);

            localLocation *= _invGridSize;

            var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
            localLocation -= offset;

            var xPos = Mathf.RoundToInt(localLocation.x);
            var yPos = Mathf.RoundToInt(localLocation.z);

            return new IntVector2(xPos, yPos);
        }

        public Vector3 GridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset)
        {
            var localPos = new Vector3(gridPosition.x + (sizeOffset.x * 0.5f), 0, gridPosition.y + (sizeOffset.y * 0.5f)) * _gridSize;
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
            if ((size.x > _dimensions.x) || (size.y > _dimensions.y))
            {
                return BoardStatus.OutOfBounds;
            }

            var extents = gridPosition + size;

            if ((gridPosition.x < 0) || (gridPosition.y < 0) || (extents.x > _dimensions.x) || (extents.y > _dimensions.y))
            {
                return BoardStatus.OutOfBounds;
            }

            for (var y = gridPosition.y; y < extents.y; y++)
            {
                for (var x = gridPosition.x; x < extents.x; x++)
                {
                    if (_availableCells[x, y])
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

            if ((size.x > _dimensions.x) || (size.y > _dimensions.y))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Given dimensions do not fit in our grid");
            }

            if ((gridPosition.x < 0) || (gridPosition.y < 0) || (extents.x > _dimensions.x) || (extents.y > _dimensions.y))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Given footprint is out of range of our grid");
            }

            for (var y = gridPosition.y; y < extents.y; y++)
            {
                for (var x = gridPosition.x; x < extents.x; x++)
                {
                    _availableCells[x, y] = true;

                    if (_tiles != null && _tiles[x, y] != null)
                    {
                        _tiles[x, y].SetState(BoardTileStatus.Filled);
                    }
                }
            }
        }

        public void Clear(IntVector2 gridPosition, IntVector2 size)
        {
            var extents = gridPosition + size;

            if ((size.x > _dimensions.x) || (size.y > _dimensions.y))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Given dimensions do not fit in our grid");
            }

            if ((gridPosition.x < 0) || (gridPosition.y < 0) || (extents.x > _dimensions.x) || (extents.y > _dimensions.y))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Given footprint is out of range of our grid");
            }

            for (var y = gridPosition.y; y < extents.y; y++)
            {
                for (var x = gridPosition.x; x < extents.x; x++)
                {
                    _availableCells[x, y] = false;

                    if (_tiles != null && _tiles[x, y] != null)
                    {
                        _tiles[x, y].SetState(BoardTileStatus.Empty);
                    }
                }
            }
        }

        private void ResizeCollider()
        {
            var boxCollider = GetComponent<BoxCollider>();
            var size = new Vector3(_dimensions.x, 0, _dimensions.y) * _gridSize;

            boxCollider.size = size;
            boxCollider.center = size * 0.5f;
        }

        private async UniTask Setup()
        {
            _tiles = new BoardTile[_dimensions.x, _dimensions.y];

            for (var y = 0; y < _dimensions.y; y++)
            {
                for (var x = 0; x < _dimensions.x; x++)
                {
                    var targetPos = GridToWorld(new IntVector2(x, y), new IntVector2(1, 1));
                    targetPos.y += 0.01f;
                    var tile = Instantiate(_boardTilePrefab, _tilesParent.transform, true);
                    var tr = tile.transform;
                    tr.position = targetPos;
                    tr.localRotation = Quaternion.identity;

                    _tiles[x, y] = tile;

                    tile.SetState(BoardTileStatus.Empty);
                    await tile.DoShow().ToUniTask();
                    tile.ResizeCollider();
                }
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gridSize <= 0)
            {
                Debug.LogError("Negative or zero grid size is invalid");
                _gridSize = 1;
            }

            if (_dimensions.x <= 0 || _dimensions.y <= 0)
            {
                Debug.LogError("Negative or zero grid dimensions are invalid");
                _dimensions = new IntVector2(Mathf.Max(_dimensions.x, 1), Mathf.Max(_dimensions.y, 1));
            }

            ResizeCollider();
        }

        private void OnDrawGizmos()
        {
            var prevCol = Gizmos.color;
            Gizmos.color = Color.cyan;

            var originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (var y = 0; y < _dimensions.y; y++)
            {
                for (var x = 0; x < _dimensions.x; x++)
                {
                    var position = new Vector3((x + 0.5f) * _gridSize, 0, (y + 0.5f) * _gridSize);
                    Gizmos.DrawWireCube(position, new Vector3(_gridSize, 0, _gridSize));
                }
            }

            Gizmos.matrix = originalMatrix;
            Gizmos.color = prevCol;

            var center = transform.TransformPoint(new Vector3(_gridSize * _dimensions.x * 0.5f, 1, _gridSize * _dimensions.y * 0.5f));
            Gizmos.DrawIcon(center, "build_zone.png", true);
        }
#endif
    }
}