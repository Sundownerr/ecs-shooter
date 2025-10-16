using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Data;
using Game.Systems;
using UnityEngine;

namespace Game.Features
{
    public class UiService
    {
        private readonly AddressablesService _addressablesService;
        private readonly GameOverUiProvider _gameOverUi;
        private readonly FPSInput _input;
        private readonly PlayerUIProvider _playerUi;
        private readonly RuntimeData _runtimeData;
        private readonly GameConfig _gameConfig;
        private readonly PlayerScoreUiProvider _scoreUi;
        private readonly StartMenuUiProvider _startMenu;
        private readonly StaticData _staticData;

        public UiService(StaticData staticData,
                         RuntimeData runtimeData,
                         GameConfig gameConfig,
                         FPSInput input,
                         AddressablesService addressablesService)
        {
            _staticData = staticData;
            _runtimeData = runtimeData;
            _gameConfig = gameConfig;
            _input = input;
            _addressablesService = addressablesService;
            _startMenu = _staticData.StartMenuUi;
            _playerUi = _staticData.PlayerUi;
            _gameOverUi = _staticData.GameOverUi;
            _scoreUi = _staticData.ScoreUi;
        }

        public void Initialize()
        {
            InitializeStartMenu();
            InitializeGameOverUi();

            _playerUi.gameObject.SetActive(!_gameConfig.StartFromMenu);
            _startMenu.gameObject.SetActive(_gameConfig.StartFromMenu);
            _scoreUi.Construct(_gameConfig);
        }

        public PlayerUIProvider PlayerUi() => _playerUi;

        private void InitializeGameOverUi()
        {
            _gameOverUi.RetryButton.onClick.AddListener(() => {
                LevelsEcsUtility.NotifyLoadingNewLevel(_runtimeData.CurrentLevelIndex);
                PlayerEcsUtility.DestroyPlayer();
                _gameOverUi.Wrapper.SetActive(false);
                _playerUi.gameObject.SetActive(true);
                _staticData.GameOverUi.Wrapper.SetActive(false);

                _input.Enable();
                Cursor.lockState = CursorLockMode.Locked;
            });

            _gameOverUi.ReturnToMenuButton.onClick.AddListener(() => {
                LevelsEcsUtility.NotifyLevelUnloaded();
                PlayerEcsUtility.DestroyPlayer();
                _addressablesService.LoadSceneAsync(_gameConfig.StartMenu);
                _gameOverUi.Wrapper.SetActive(false);
                _playerUi.gameObject.SetActive(false);
                _startMenu.gameObject.SetActive(true);

                _input.Disable();
                Cursor.lockState = CursorLockMode.None;
            });
        }

        public void ActivateGameOverUi()
        {
            _gameOverUi.Wrapper.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            _input.Disable();
        }

        public void UpdateYellowCubesCount(int yellowCubes)
        {
            _playerUi.YellowCubesCount.text = yellowCubes.ToString();
        }
        
        private void InitializeStartMenu()
        {
            _startMenu.LevelSelectionDropdown.ClearOptions();

            List<string> levelNames = _gameConfig.Levels.Select(x => x.Name).ToList();
            _startMenu.LevelSelectionDropdown.AddOptions(levelNames);

            _startMenu.PlayButton.onClick.AddListener(() => {
                StaticStash.RequestCreateLevel.CreateRequest(new Request_CreateLevel {
                    Index = _startMenu.LevelSelectionDropdown.value,
                });

                _startMenu.gameObject.SetActive(false);
                _playerUi.gameObject.SetActive(true);

                _input.Enable();
                Cursor.lockState = CursorLockMode.Locked;
            });
        }

        public PlayerScoreUiProvider ScoreUi() => _scoreUi;
    }
}