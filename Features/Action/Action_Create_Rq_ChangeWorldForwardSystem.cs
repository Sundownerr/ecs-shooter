using EcsMagic.CommonComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_Create_Rq_ChangeWorldForwardSystem : ISystem
    {
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<Request_ChangeWorldForward> _rqChangeWorldForward;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CreateRequest_ChangeWorldForward, Active>();
            _active = World.GetStash<Active>();
            _rqChangeWorldForward = World.GetStash<Request_ChangeWorldForward>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                _rqChangeWorldForward.Add(World.CreateEntity());
                _active.Remove(entity);
            }
        }
    }
}