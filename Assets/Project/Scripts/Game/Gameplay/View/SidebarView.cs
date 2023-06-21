using UnityEngine;

namespace Game.Gameplay.View
{
    public class SidebarView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        public Transform Container => container;
    }
}