using System.Linq;
using Game.Gameplay.Entity;
using Game.Gameplay.View;
using UnityEngine;

namespace Game.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "GameplayAssets", menuName = "ScriptableObject/GameplayAssets", order = 0)]
    public class GameplayAssets : ScriptableObject
    {
        [field: Header("PREFABS:")]
        [field: SerializeField]
        public SpawnView SpawnView { get; private set; }

        [field: Header("DATA:")]
        [field: SerializeField]
        public Figure[] Figures { get; private set; }

        public Figure GetFigure(int level)
        {
            return Figures.Where((t, i) => i == level).FirstOrDefault();
        }
    }
}