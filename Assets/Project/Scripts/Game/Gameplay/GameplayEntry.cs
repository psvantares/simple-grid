using Game.Engine.Board;
using Game.Engine.Data;
using Game.Engine.Models;
using Game.Gameplay.Configs;
using Game.Gameplay.Managers;
using Game.Gameplay.Models;
using Game.Popups;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntry : MonoBehaviour
    {
        [SerializeField]
        private ViewManager viewManager;

        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private PoolManager poolManager;

        [SerializeField]
        private PopupManager popupManager;

        [SerializeField]
        private CameraManager cameraManager;

        [Space]
        [SerializeField]
        private Board board;

        [Space]
        [SerializeField]
        private GameplayAssets gameplayAssets;

        private IBoardModel boardModel;
        private ILevelModel levelModel;

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            LoadModels();
            LoadLevel();
        }

        private void LoadModels()
        {
            boardModel = new BoardModel(new IntVector2(4, 4), 1);
            levelModel = new LevelModel();

            levelModel.Load();
        }

        private void LoadLevel()
        {
            poolManager.Initialize(gameplayAssets, viewManager);
            board.Initialize(boardModel);
            viewManager.Initialize(levelModel, gameplayAssets, poolManager, popupManager);
            gameManager.Initialize(boardModel, board, cameraManager);
        }
    }
}