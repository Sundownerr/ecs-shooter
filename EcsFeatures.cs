using System.Collections.Generic;
using Scellecs.Morpeh;
using SDW.EcsMagic.Triggers;

namespace Game.Features
{
    public class EcsFeatures
    {
        private readonly DataLocator _dataLocator;
        private readonly FPSInput _input;
        private readonly ServiceLocator _serviceLocator;

        public EcsFeatures(FPSInput input,
                           DataLocator dataLocator,
                           ServiceLocator serviceLocator)
        {
            _input = input;
            _dataLocator = dataLocator;
            _serviceLocator = serviceLocator;
        }

        public void AddTo(World world)
        {
            var features = new List<Feature> {
                new EcsPipelineStartFeature(),

                new InitializationFeature(_dataLocator, _serviceLocator),

                new WorldPositionFeature(),

                new TimerFeature(),
                new GravityFeature(),
                new TargetingFeature(),

                new DeathFeature(),

                new ParticleOnDeathFeature(_dataLocator),

                new LevelFeature(_dataLocator, _serviceLocator),

                new PlayerFactoryFeature(_dataLocator),
                new PlayerInputFeature(_input),
                new PlayerAbilitiesFeature(),

                new PlayerMovementFeature(),
                new PlayerScoreFeature(_serviceLocator),
                // new PlayerFlyingFeature(),

                new WeaponFeature(_dataLocator, _serviceLocator),
                new PlayerWeaponChangeFeature(),

                new ResetConditionsFeature(),

                new AbilityInitialStateFeature(),
                new AbilityTargetFeature(),

                new ConditionsFeature(),

                new StateMachineFeature(),
                new AbilityStatesFeature(),
             
                new ActionUsageFeature(),

                new NpcActionFeature(),

                new ActionConfigFeature(),
                new InitializeActionsFeature(),

                new ActionFeature(_dataLocator, _serviceLocator),

                new CompleteActionFeature(),
                new ActionCancelFeature(),

                new AbilityUsingToInitialStateFeature(),

                new LerpMovementFeature(),

                new EnemySpawnFeature(),
                new NpcFactoryFeature(),
                new NpcMovementFeature(),

                new DamagableFeature(),

                // new DelayedUpdateFeature(),
                new DamageFeature(_serviceLocator),

                new PlayerResourcesFeature(),

                new PlayerUiFeature(_serviceLocator),

                // new GameOverFeature(_input),

                new ChangeWorldFeature(_dataLocator),

                new MarkToDestroyFeature(),
                new DestroyEntityFeature(),

                new SpawnerLandingPointsFeature(_dataLocator),
                new ClearTriggerFeature(),
                new EcsPipelineEndFeature(),
            };

            AddFeaturesInWorld(world, features);
        }

        private static void AddFeaturesInWorld(World world, List<Feature> features)
        {
            for(var i = 0; i < features.Count; i++)
                world.AddSystemsGroup(i, features[i].AddTo(world));
        }
    }
}