using UnityEngine;

namespace Game.Gameplay.Entity
{
    [RequireComponent(typeof(Collider))]
    public class FigureGhost : MonoBehaviour
    {
        [SerializeField]
        private Material material;

        [SerializeField]
        private Material invalidPositionMaterial;

        [SerializeField]
        private float dampSpeed = 0.075f;

        private MeshRenderer[] meshRenderers;
        private Vector3 defaultPosition;
        private Vector3 targetPosition;
        private bool validPosition;

        public Figure Controller { get; private set; }

        private void Update()
        {
            var currentPos = transform.position;

            if (Vector3.SqrMagnitude(currentPos - targetPosition) > 0.0001f)
            {
                currentPos = Vector3.SmoothDamp(currentPos, targetPosition, ref defaultPosition, dampSpeed);
                transform.position = currentPos;
            }
            else
            {
                defaultPosition = Vector3.zero;
            }
        }

        public void Initialize(Figure figure)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            Controller = figure;

            defaultPosition = Vector3.zero;
            validPosition = false;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            if (gameObject.activeSelf)
            {
                return;
            }

            gameObject.SetActive(true);
            defaultPosition = Vector3.zero;
            validPosition = false;
        }

        public void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        {
            targetPosition = worldPosition;

            if (!validPosition)
            {
                validPosition = true;
                transform.position = targetPosition;
            }

            transform.rotation = rotation;

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
            }
        }
    }
}