using System;
using System.Collections.Generic;
using Ability.Identifications;
using Ability.Utilities;
using EcsMagic.Abilities;

namespace Ability
{
    public static class AbilityActionToTypeMap
    {
        public static List<(int, Dictionary<int, Type>)> Create() => new() {
            ActionId.Create.Group()
                .With<CreateEnemy_AbilityAction>(ActionId.Enemy)
                .With<CreateGameObject_AbilityAction>(ActionId.GameObject),

            ActionId.Destroy.Group()
                .With<DestroyGameObject_AbilityAction>(ActionId.GameObject)
                .With<DestroyUser_AbilityAction>(ActionId.User),

            ActionId.Move.Group()
                .With<MoveTransform_AbilityAction>(ActionId.Transform),

            ActionId.Rotate.Group()
                .With<RotateTransform_AbilityAction>(ActionId.Transform),

            ActionId.Damage.Group()
                .With<Damage_AbilityAction>(ActionId.Damage),

            ActionId.SetActive.Group()
                .With<SetGameObjectActive_AbilityAction>(ActionId.GameObject)
                .With<SetAbilityActivated_AbilityAction>(ActionId.Ability),

            ActionId.Play.Group()
                .With<Particle_AbilityAction>(ActionId.ParticleSystem)
                .With<StaticParticle_AbilityAction>(ActionId.StaticParticleSystem)
                .With<MMFeedback_AbilityAction>(ActionId.MMFPlayer),

            ActionId.FloatStats.Group()
                .With<ChangeFloatStat_AbilityAction>(ActionId.ChangeStatValue)
                .With<AddFloatStatModifier_AbilityAction>(ActionId.AddStatModifier)
                .With<RemoveFloatStatModifier_AbilityAction>(ActionId.RemoveStatModifier),

            ActionId.ReturnToPool.Group()
                .With<ReturnToPool_AbilityAction>(ActionId.GameObject),

            ActionId.NavMeshAgent.Group()
                .With<NavMeshAgent_AbilityAction>(ActionId.NavMeshAgent),

            ActionId.Animator.Group()
                .With<ChangeAnimatorSettings_AbilityAction>(ActionId.Animator),

            ActionId.ChangeComponentEnabled.Group()
                .With<ChangeComponentEnabled_AbilityAction>(ActionId.ComponentEnabled),

            ActionId.Resource.Group()
                .With<ChangeMana_AbilityAction>(ActionId.Mana)
                .With<ChangeYellowCubes_AbilityAction>(ActionId.YellowCubes)
                .With<RegenerateFullHealth_AbilityAction>(ActionId.RegenerateFullHealth)
                .With<RegenerateFullMana_AbilityAction>(ActionId.RegenerateFullMana),

            ActionId.Dash.Group()
                .With<Dash_AbilityAction>(ActionId.Dash),

            ActionId.LineRenderer.Group()
                .With<LineRenderer_AbilityAction>(ActionId.LineRenderer),

            ActionId.Rigidbody.Group()
                .With<Rigidbody_AbilityAction>(ActionId.Rigidbody),

            ActionId.Sprint.Group()
                .With<Sprint_AbilityAction>(ActionId.Sprint),

            ActionId.WeaponTrigger.Group()
                .With<WeaponTrigger_AbilityAction>(ActionId.WeaponTrigger),
            
            ActionId.ChangeWorldForward.Group()
                .With<ChangeWorldForward_AbilityAction>(ActionId.ChangeWorldForward),
        };
    }
}