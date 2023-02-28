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
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private PopupManager _popupManager;
        [SerializeField] private CameraManager _cameraManager;

        [Space]
        [SerializeField] private Board _board;

        [Space]
        [SerializeField] private GameplayAssets _gameplayAssets;

        private IBoardModel _boardModel;
        private ILevelModel _levelModel;

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
            _boardModel = new BoardModel(new IntVector2(4, 4), 1);
            _levelModel = new LevelModel();

            _levelModel.Load();
        }

        private void LoadLevel()
        {
            _poolManager.Initialize(_gameplayAssets, _viewManager);
            _board.Initialize(_boardModel);
            _viewManager.Initialize(_levelModel, _gameplayAssets, _poolManager, _popupManager);
            _gameManager.Initialize(_boardModel, _board, _cameraManager);
        }
    }
}