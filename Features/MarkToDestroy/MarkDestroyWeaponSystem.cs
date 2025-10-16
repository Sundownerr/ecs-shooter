using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkDestroyWeaponSystem : ISystem
    {
        private Filter _filter;
        private Stash<Weapon> _weapon;
        private Stash<WeaponsList> _weapons;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WeaponsList, WillBeDestroyed>();
            _weapons = World.GetStash<WeaponsList>();
            _weapon = World.GetStash<Weapon>();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var weapons = ref _weapons.Get(entity);

                foreach (var weaponEntity in weapons.List) {
                    ref var weapon = ref _weapon.Get(weaponEntity);
                    
                   
                    _willBeDestroyed.Add(weaponEntity);
                }
            }
        }
    }
}