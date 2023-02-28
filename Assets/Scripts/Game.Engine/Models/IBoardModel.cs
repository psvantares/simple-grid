using Game.Engine.Data;

namespace Game.Engine.Models
{
    public interface IBoardModel
    {
        IntVector2 Dimensions { get; }
        float GridSize { get; }
    }
}