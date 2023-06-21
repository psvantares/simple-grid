using UnityEngine;

namespace Game.Gameplay.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [field: SerializeField]
        public Camera GameCamera { get; private set; }

        [field: SerializeField]
        public float Angle { get; private set; } = 45;

        [field: SerializeField]
        public float AngleY { get; private set; } = 45;

        [field: SerializeField]
        public float Radius { get; private set; } = 30;

        [field: SerializeField]
        public float Padding { get; private set; } = 1;

        public void SetPosition(Bounds bounds, Vector3 position)
        {
            var tr = GameCamera.transform;
            var angle = Angle * Mathf.Deg2Rad;
            var angleY = AngleY * Mathf.Deg2Rad;
            var x = position.x + Mathf.Cos(angle) * Mathf.Cos(angleY) * Radius;
            var z = position.z + Mathf.Cos(angle) * Mathf.Sin(angleY) * Radius;
            var y = position.y + Mathf.Sin(angle) * Radius;

            var target = new Vector3(x, y, z);
            var targetDirection = position - target;
            var rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            tr.position = target;
            tr.rotation = rotation;

            var width = Mathf.Pow(bounds.size.x, 2) + Mathf.Pow(bounds.size.z, 2);
            var orthographicSize = 0.5f * Mathf.Sqrt(width) / GameCamera.aspect;

            GameCamera.orthographicSize = orthographicSize + Padding;
        }

        public Vector2 GetWorldPoint(Vector2 screenPoint)
        {
            return GameCamera.ScreenToWorldPoint(screenPoint);
        }
    }
}