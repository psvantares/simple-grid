using System;

namespace Game.Gameplay.Data
{
    [Serializable]
    public class SpawnButtonData
    {
        public readonly int Level;

        public SpawnButtonData(int level)
        {
            Level = level;
        }
    }
}