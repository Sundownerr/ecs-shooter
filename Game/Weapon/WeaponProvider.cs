using Game.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class WeaponProvider : EntityProvider
    {
        public int Damage = 1;
        public float ShootInterval = 0.3f;
     
        public float DrawDuration = 0.1f;
        public float HideDuration = 0.1f;
        public LayerMask HitLayerMask;
        
        [Space]
        public float ReloadTime = 0.5f;
        public int MagazineSize = 10;
        
        [Space]
        public WeaponShootingComponent ShootingComponent;
       
        [Space]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnShootAbility;
      
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnHitAbility;
      
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnTriggerPulledAbility;
     
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnTriggerReleasedAbility;

        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnWeaponDraw;
       
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnWeaponHide;


        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        // [InlineEditor]
        public AbilityProvider[] OnReloadStart;

        
        
        public void DrawWeapon()
        {
            foreach (var abilityProvider in OnWeaponDraw)
                abilityProvider.Activate();
        }

        public void HideWeapon()
        {
            foreach (var abilityProvider in OnWeaponHide)
                abilityProvider.Activate();
        }

        public void PullTrigger()
        {
            if (!StaticStash.WeaponTriggerPulled.Has(Entity)) {
                StaticStash.WeaponTriggerPulled.Add(Entity);

                foreach (var abilityProvider in OnTriggerPulledAbility)
                    abilityProvider.Activate();

                foreach (var abilityProvider in OnTriggerReleasedAbility)
                    abilityProvider.Deactivate();

                // Debug.Log("PullTrigger");
            }
        }

        public void ReleaseTrigger()
        {
            if (StaticStash.WeaponTriggerPulled.Has(Entity)) {
                StaticStash.WeaponTriggerPulled.Remove(Entity);

                foreach (var abilityProvider in OnTriggerReleasedAbility)
                    abilityProvider.Activate();

                foreach (var abilityProvider in OnTriggerPulledAbility)
                    abilityProvider.Deactivate();

                // Debug.Log("ReleaseTrigger");
            }
        }
    }
}