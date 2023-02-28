using System;
using Game.Engine.Board;
using Game.Engine.Data;
using Game.Engine.Models;
using Game.Gameplay.Entity;
using Game.Gameplay.Events;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform _gamePlane;
        [SerializeField] private LayerMask _boardMask;
        [SerializeField] private LayerMask _ghostMask;
        [SerializeField] private float _sphereCastRadius = 1;

        private IBoardModel _boardModel;
        private IBoard _board;
        private CameraManager _cameraManager;
        private Camera _gameCamera;

        private FigureGhost _currentFigureGhost;
        private IntVector2 _gridPosition;

        private bool _ghostPossible;

        private readonly CompositeDisposable _disposable = new();

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveGhost(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                TryOccupy();
            }
        }

        public void Initialize(IBoardModel boardModel, IBoard board, CameraManager cameraManager)
        {
            _boardModel = boardModel;
            _board = board;
            _cameraManager = cameraManager;
            _gameCamera = _cameraManager.GameCamera;

            SetCameraPosition();
            SetPlanePosition();
            Subscribe();
        }

        private void Subscribe()
        {
            GameEvents.DraggedOff.Subscribe(OnDraggedOff).AddTo(_disposable);
        }

        private void SetCameraPosition()
        {
            var gridSize = _boardModel.GridSize;
            var dimensions = _boardModel.Dimensions;
            var position = new Vector3(gridSize * dimensions.x * 0.5f, 0, gridSize * dimensions.y * 0.5f);
            var center = transform.TransformPoint(position);
            var bound = _board.GetBounds();

            _cameraManager.SetPosition(bound, center);
        }

        private void SetPlanePosition()
        {
            var gridSize = _boardModel.GridSize;
            var dimensions = _boardModel.Dimensions;
            var position = new Vector3(gridSize * dimensions.x * 0.5f, -0.5f, gridSize * dimensions.y * 0.5f);
            var center = transform.TransformPoint(position);

            _gamePlane.position = center;
        }

        private void MoveGhost(Vector3 mousePosition)
        {
            if (_currentFigureGhost == null)
            {
                return;
            }

            var ray = _gameCamera.ScreenPointToRay(mousePosition);
            RaycastHit? raycastHit = null;

            BoardRaycast(ray, ref raycastHit);

            if (raycastHit != null)
            {
                MoveGhostWithRaycastHit(raycastHit.Value);
            }
            else
            {
                MoveGhostOntoWorld(ray);
            }
        }

        private void MoveGhostOntoWorld(Ray ray)
        {
            Physics.SphereCast(ray, _sphereCastRadius, out var hit, float.MaxValue, _ghostMask);

            if (hit.collider == null)
            {
                return;
            }

            _ghostPossible = false;
            _currentFigureGhost.Show();
            _currentFigureGhost.Move(hit.point, hit.collider.transform.rotation, _ghostPossible);
        }

        private void BoardRaycast(Ray ray, ref RaycastHit? raycastHit)
        {
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _boardMask))
            {
                raycastHit = hit;
            }
        }

        private void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            _gridPosition = _board.WorldToGrid(raycast.point, _currentFigureGhost.Controller.Dimensions);

            var fits = _board.Fits(_gridPosition, _currentFigureGhost.Controller.Dimensions);
            var worldPosition = _board.GridToWorld(_gridPosition, _currentFigureGhost.Controller.Dimensions);

            _currentFigureGhost.Show();
            _ghostPossible = fits == BoardStatus.Fits;
            _currentFigureGhost.Move(worldPosition, _board.Transform.rotation, _ghostPossible);
        }

        private void TryOccupy()
        {
            if (_currentFigureGhost != null && _ghostPossible)
            {
                // occupy
            }
            else
            {
                CancelFigureGhost();
            }
        }

        private void CancelFigureGhost()
        {
            if (_currentFigureGhost == null)
            {
                return;
            }

            Destroy(_currentFigureGhost.gameObject);

            _currentFigureGhost = null;
        }

        private void SetupFigureGhost(Figure data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _currentFigureGhost = Instantiate(data.FigureGhostPrefab);
            _currentFigureGhost.Initialize(data);
            _currentFigureGhost.Hide();
        }

        // Events

        private void OnDraggedOff(Figure data)
        {
            CancelFigureGhost();
            SetupFigureGhost(data);
        }
    }
}