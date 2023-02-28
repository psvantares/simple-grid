using UnityEngine;

namespace Game.Gameplay.Entity
{
    [RequireComponent(typeof(Collider))]
    public class FigureGhost : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private Material _invalidPositionMaterial;
        [SerializeField] private float _dampSpeed = 0.075f;

        private MeshRenderer[] _meshRenderers;
        private Vector3 _defaultPosition;
        private Vector3 _targetPosition;
        private bool _validPosition;

        public Figure Controller { get; private set; }

        private void Update()
        {
            var currentPos = transform.position;

            if (Vector3.SqrMagnitude(currentPos - _targetPosition) > 0.0001f)
            {
                currentPos = Vector3.SmoothDamp(currentPos, _targetPosition, ref _defaultPosition, _dampSpeed);
                transform.position = currentPos;
            }
            else
            {
                _defaultPosition = Vector3.zero;
            }
        }

        public void Initialize(Figure figure)
        {
            _meshRenderers = GetComponentsInChildren<MeshRenderer>();
            Controller = figure;

            _defaultPosition = Vector3.zero;
            _validPosition = false;
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
            _defaultPosition = Vector3.zero;
            _validPosition = false;
        }

        public void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        {
            _targetPosition = worldPosition;

            if (!_validPosition)
            {
                _validPosition = true;
                transform.position = _targetPosition;
            }

            transform.rotation = rotation;

            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.sharedMaterial = validLocation ? _material : _invalidPositionMaterial;
            }
        }
    }
}