using System.Collections.Generic;
using Game.Gameplay.Configs;
using Game.Gameplay.Data;
using Game.Gameplay.Entity;
using Game.Gameplay.Events;
using Game.Gameplay.Pool;

namespace Game.Gameplay.Controllers
{
    public class SidebarController : IController
    {
        public SidebarController(GameplayAssets gameplayAssets, SpawnViewPool pool, IEnumerable<SpawnButtonData> data)
        {
            foreach (var spawnButtonData in data)
            {
                var button = pool.Rent();
                var level = spawnButtonData.Level;
                var figure = gameplayAssets.GetFigure(level);
                
                button.Initialize(level, figure);
                button.OnSelectFigure += OnSelectFigure;
            }
        }

        public void Dispose()
        {
        }

        private static void OnSelectFigure(Figure data)
        {
            GameEvents.DraggedOffSubject.OnNext(data);
        }
    }
}