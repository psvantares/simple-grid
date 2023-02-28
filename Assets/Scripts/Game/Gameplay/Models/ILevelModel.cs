using Game.Gameplay.Data;

namespace Game.Gameplay.Models
{
    public interface ILevelModel : IModel
    {
        SpawnButtonData[] SpawnButtonData { get; }
    }
}