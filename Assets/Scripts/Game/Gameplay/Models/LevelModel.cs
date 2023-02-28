using System;
using Game.Gameplay.Data;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Models
{
    [Serializable]
    public class LevelModel : ILevelModel
    {
        [field: SerializeField] public SpawnButtonData[] SpawnButtonData { get; private set; }

        private const string Key = "LevelModel";

        public void Copy(LevelModel state)
        {
            if (state != null)
            {
                SpawnButtonData = state.SpawnButtonData ?? new SpawnButtonData[3];
            }
            else
            {
                Clear();
            }
        }

        public void Clear()
        {
            SpawnButtonData = new SpawnButtonData[3];
            SpawnButtonData[0] = new SpawnButtonData(0);
            SpawnButtonData[1] = new SpawnButtonData(1);
            SpawnButtonData[2] = new SpawnButtonData(2);
        }

        public void Load()
        {
            Copy(Storage.Load<LevelModel>(Key));
        }

        public void Save()
        {
            var model = new LevelModel();
            model.Copy(this);
            Storage.Save(model, Key);
        }
    }
}