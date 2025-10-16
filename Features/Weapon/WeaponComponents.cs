using Scellecs.Morpeh;

namespace Game
{
    public struct WeaponAmmo : IComponent
    {
        public int CurrentClipAmmo;
        public int MaxClipAmmo;
        public float ReloadTimeSeconds;
    }

    public struct Reloading : IComponent
    {
        public float RemainingTime;
    }
}