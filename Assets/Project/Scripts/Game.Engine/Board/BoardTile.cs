using DG.Tweening;
using UnityEngine;

namespace Game.Engine.Board
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoardTile : MonoBehaviour
    {
        [SerializeField]
        private Material tileMaterial;

        [SerializeField]
        private Material filledMaterial;

        [SerializeField]
        private Renderer tileRenderer;

        private Vector3 position;
        private Tweener shakeTweener;

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
            shakeTweener?.Kill();
        }

        public void SetState(BoardTileStatus newStatus)
        {
            if (newStatus == BoardTileStatus.Filled)
            {
                if (tileRenderer != null && filledMaterial != null)
                {
                    tileRenderer.sharedMaterial = filledMaterial;
                }
            }
            else if (newStatus == BoardTileStatus.Empty)
            {
                if (tileRenderer != null && tileMaterial != null)
                {
                    tileRenderer.sharedMaterial = tileMaterial;
                }
            }
        }

        private void Setup()
        {
            position = transform.localPosition;
        }

        public void ResizeCollider()
        {
            var bounds = tileRenderer.bounds;
            var boxCollider = GetComponent<BoxCollider>();
            var size = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
            boxCollider.size = size;
        }

        private void DoShake()
        {
            transform.localPosition = position;

            shakeTweener.Kill();
            shakeTweener = transform
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