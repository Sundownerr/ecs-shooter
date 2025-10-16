using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public abstract class WeaponShootingComponent : MonoBehaviour
    {
        protected WeaponProvider _weaponProvider;
        protected World _world;

        public void Initialize(WeaponProvider weaponProvider, World world)
        {
            _world = world;
            _weaponProvider = weaponProvider;
        }
    }
}