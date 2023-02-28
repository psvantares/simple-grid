using DG.Tweening;
using UnityEngine;

namespace Game.Engine.Board
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private Material _tileMaterial;
        [SerializeField] private Material _filledMaterial;
        [SerializeField] private Renderer _tileRenderer;

        private Vector3 _position;
        private Tweener _shakeTweener;

        private void OnMouseEnter()
        {
            DoShake();
        }

        private void Start()
        {
            Setup();
        }

        private void OnDisable()
        {
            _shakeTweener?.Kill();
        }

        public void SetState(BoardTileStatus newStatus)
        {
            if (newStatus == BoardTileStatus.Filled)
            {
                if (_tileRenderer != null && _filledMaterial != null)
                {
                    _tileRenderer.sharedMaterial = _filledMaterial;
                }
            }
            else if (newStatus == BoardTileStatus.Empty)
            {
                if (_tileRenderer != null && _tileMaterial != null)
                {
                    _tileRenderer.sharedMaterial = _tileMaterial;
                }
            }
        }

        private void Setup()
        {
            _position = transform.localPosition;
        }

        public void ResizeCollider()
        {
            var bounds = _tileRenderer.bounds;
            var boxCollider = GetComponent<BoxCollider>();
            var size = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
            boxCollider.size = size;
        }

        private void DoShake()
        {
            transform.localPosition = _position;

            _shakeTweener.Kill();
            _shakeTweener = transform
                .DOShakePosition(0.25f, new Vector3(0, 0.1f, 0), 5, 0)
                .SetEase(Ease.OutQuad);
        }

        public Tweener DoShow()
        {
            transform.localScale = Vector3.zero;

            return transform
                .DOScale(1, 0.1f)
                .SetEase(Ease.OutBack);
        }
    }
}