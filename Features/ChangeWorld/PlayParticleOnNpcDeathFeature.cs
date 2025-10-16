using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Features
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayParticleOnNpcDeathFeature : ISystem
    {
        private readonly RuntimeData _runtimeData;

        private Filter _filter;
        private Stash<Npc> _npc;

        public PlayParticleOnNpcDeathFeature(DataLocator dataLocator)
        {
            _runtimeData = dataLocator.Get<RuntimeData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Npc, Dead, PlayParticleOnDeath>();
            _npc = World.GetStash<Npc>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var npc = ref _npc.Get(entity);

                _runtimeData.StaticParticles.WithId(npc.Config.DeathParticleId)
                    .EmitAt(npc.Instance.transform.position + npc.Config.DeathParticleOffset);
            }
        }
    }
}