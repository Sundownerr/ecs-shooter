using Game.AbilityComponents;
using Game.Components;
using Game.StateMachineComponents;
using Game.Systems;
using Game.WeaponComponents;
using SDW.EcsMagic.Triggers;

namespace Game.Features
{
    public class DestroyEntityFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new RemoveDeadEnemyFromListSystem());

            System(new DestroyProviderActivatorsSystem());
            
            System(new DestroyTargetingPartsSystem());
            System(new DestroyForwardConditionsSystem());
            System(new DestroyCooldownPartsSystem());
            System(new DestroyInstantActionsSystem());
            System(new DestroyCancelActionsSystem());
            System(new DestroyCancelConditionsSystem());
            System(new DestroyUsagePartsSystem());
            System(new DestroyUsageProgressSystem());
            
            System(new DestroyTransitionNpcActionsSystem());

            System(new DestroyEntities<Weapon, WillBeDestroyed>());
            System(new DestroyAbilitySystem());
            System(new DestroyEntities<Transition, WillBeDestroyed>());
            System(new DestroyEntities<EnterAction, WillBeDestroyed>());
            System(new DestroyNpcSystem());
            System(new DestroyPlayerSystem());
            System(new DestroyEntities<WorldTriggerTag>());
            System(new DestroyDamagableSystem());
            System(new DestroyEntities<StateMachine, WillBeDestroyed>());
            System(new DestroyEntities<AbilityTag, WillBeDestroyed>());
            System(new DestroyEntities<EnemySpawner, WillBeDestroyed>());
            System(new DestroyEntities<CurrentLevel, WillBeDestroyed>());
            System(new DestroyEntities<EntityCreatedDuringGameplay, WillBeDestroyed>());
        }
    }
}