using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_CreateEnemySystem : ISystem
    {
        private Stash<AbilityCreateEnemy> _abilityCreateEnemy;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<Request_CreateNpc> _requestCreateNpc;
        private Stash<RotationFromConfig> _rotationFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityCreateEnemy, Active>();

            _abilityCreateEnemy = World.GetStash<AbilityCreateEnemy>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _active = World.GetStash<Active>();
            _requestCreateNpc = World.GetStash<Request_CreateNpc>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var createEnemy = ref _abilityCreateEnemy.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);

                _requestCreateNpc.CreateRequest(new Request_CreateNpc {
                    Config = createEnemy.Config.Enemy,
                    Position = positionFromConfig.Position,
                    Rotation = rotationFromConfig.Rotation,
                });

                _active.Remove(entity);
            }
        }
    }
}