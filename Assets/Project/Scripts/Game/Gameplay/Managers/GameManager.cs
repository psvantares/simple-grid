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
        [SerializeField]
        private Transform gamePlane;

        [SerializeField]
        private LayerMask boardMask;

        [SerializeField]
        private LayerMask ghostMask;

        [SerializeField]
        private float sphereCastRadius = 1;

        private IBoardModel boardModel;
        private IBoard board;
        private CameraManager cameraManager;
        private Camera gameCamera;

        private FigureGhost currentFigureGhost;
        private IntVector2 gridPosition;

        private bool ghostPossible;

        private readonly CompositeDisposable disposable = new();

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
            this.boardModel = boardModel;
            this.board = board;
            this.cameraManager = cameraManager;
            gameCamera = this.cameraManager.GameCamera;

            SetCameraPosition();
            SetPlanePosition();
            Subscribe();
        }

        private void Subscribe()
        {
            GameEvents.DraggedOff.Subscribe(OnDraggedOff).AddTo(disposable);
        }

        private void SetCameraPosition()
        {
            var gridSize = boardModel.GridSize;
            var dimensions = boardModel.Dimensions;
            var position = new Vector3(gridSize * dimensions.X * 0.5f, 0, gridSize * dimensions.Y * 0.5f);
            var center = transform.TransformPoint(position);
            var bound = board.GetBounds();

            cameraManager.SetPosition(bound, center);
        }

        private void SetPlanePosition()
        {
            var gridSize = boardModel.GridSize;
            var dimensions = boardModel.Dimensions;
            var position = new Vector3(gridSize * dimensions.X * 0.5f, -0.5f, gridSize * dimensions.Y * 0.5f);
            var center = transform.TransformPoint(position);

            gamePlane.position = center;
        }

        private void MoveGhost(Vector3 mousePosition)
        {
            if (currentFigureGhost == null)
            {
                return;
            }

            var ray = gameCamera.ScreenPointToRay(mousePosition);
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
            Physics.SphereCast(ray, sphereCastRadius, out var hit, float.MaxValue, ghostMask);

            if (hit.collider == null)
            {
                return;
            }

            ghostPossible = false;
            currentFigureGhost.Show();
            currentFigureGhost.Move(hit.point, hit.collider.transform.rotation, ghostPossible);
        }

        private void BoardRaycast(Ray ray, ref RaycastHit? raycastHit)
        {
            if (Physics.Raycast(ray, out var hit, float.MaxValue, boardMask))
            {
                raycastHit = hit;
            }
        }

        private void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            gridPosition = board.WorldToGrid(raycast.point, currentFigureGhost.Controller.Dimensions);

            var fits = board.Fits(gridPosition, currentFigureGhost.Controller.Dimensions);
            var worldPosition = board.GridToWorld(gridPosition, currentFigureGhost.Controller.Dimensions);

            currentFigureGhost.Show();
            ghostPossible = fits == BoardStatus.Fits;
            currentFigureGhost.Move(worldPosition, board.Transform.rotation, ghostPossible);
        }

        private void TryOccupy()
        {
            if (currentFigureGhost != null && ghostPossible)
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
            if (currentFigureGhost == null)
            {
                return;
            }

            Destroy(currentFigureGhost.gameObject);

            currentFigureGhost = null;
        }

        private void SetupFigureGhost(Figure data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            currentFigureGhost = Instantiate(data.FigureGhostPrefab);
            currentFigureGhost.Initialize(data);
            currentFigureGhost.Hide();
        }

        // Events

        private void OnDraggedOff(Figure data)
        {
            CancelFigureGhost();
            SetupFigureGhost(data);
        }
    }
}