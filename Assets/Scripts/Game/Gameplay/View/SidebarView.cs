using UnityEngine;

namespace Game.Gameplay.View
{
    public class SidebarView : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        public Transform Container => _container;
    }
}