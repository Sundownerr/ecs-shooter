using System.Collections;
using Game.Data;
using Game.Features;
using Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class EntryPoint : MonoBehaviour
    {
        public static bool StartedFromEntryPoint;

        [InlineProperty] [HideLabel] [SerializeField]
        private StaticData _staticData;

        [Space]
        [Space]
        [InlineEditor] [HideLabel] [SerializeField]
        private GameConfig _gameConfig;

        private IEnumerator Start()
        {
            StartedFromEntryPoint = true;
            NavMesh.avoidancePredictionTime = 0.25f;
            NavMesh.pathfindingIterationsPerFrame = 5000;
            QualitySettings.vSyncCount = _gameConfig.VSyncCount;
            Application.targetFrameRate = _gameConfig.TargetFps;

            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_staticData.DdolWrapper);

            var runtimeData = new RuntimeData();
            var input = new FPSInput();
            var addressablesService = new AddressablesService();
            var uiManager = new UiService(_staticData, runtimeData, _gameConfig, input, addressablesService);
            var runtimeDataService = new RuntimeDataService(runtimeData, _staticData, _gameConfig);
            var damageInstanceService = new DamageInstanceService();
            var playerScoreService = new PlayerScoreService(_gameConfig);

            var serviceLocator = new ServiceLocator()
                    .Register(uiManager)
                    .Register(runtimeDataService)
                    .Register(addressablesService)
                    .Register(damageInstanceService)
                    .Register(playerScoreService)
                ;

            var dataLocator = new DataLocator()
                    .Register(runtimeData)
                    .Register(_staticData)
                    .Register(_gameConfig)
                ;

            var ecs = GetComponent<Ecs>();
            var ecsFeatures = new EcsFeatures(input, dataLocator, serviceLocator);

            input.Enable();
            runtimeDataService.InitializeData();
            uiManager.Initialize();

            yield return new WaitForEndOfFrame();

            ecs.Initialize(ecsFeatures);

            if (!_gameConfig.StartFromMenu)
                Cursor.lockState = CursorLockMode.Locked;
            else
                addressablesService.LoadSceneAsync(_gameConfig.StartMenu);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ClearOnReload() =>
            StartedFromEntryPoint = false;
    }
}