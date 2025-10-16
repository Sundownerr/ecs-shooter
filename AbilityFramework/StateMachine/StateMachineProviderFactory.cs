using System;
using System.Collections.Generic;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.StateMachineComponents;
using Scellecs.Morpeh;

namespace Game
{
    public static class StateMachineProviderFactory
    {
        public static Entity Create(this StateMachineProvider stateMachineProvider, Entity user, World world)
        {
            var stateMachineEntity = stateMachineProvider.Initialize(world);
            stateMachineProvider.Created = true;

            var transitionEntities = new Entity[stateMachineProvider.Transitions.Count];
            var enterActionEntities = new Entity[stateMachineProvider.EnterActions.Count];

            StaticStash.ParentEntity.Set(stateMachineEntity, new ParentEntity {Entity = user,});

            StaticStash.StateMachine.Set(stateMachineEntity, new StateMachine {
                Transitions = transitionEntities,
                EnterActions = enterActionEntities,
            });

            for(var i = 0; i < stateMachineProvider.Transitions.Count; i++) {
                var transitionConfig = stateMachineProvider.Transitions[i];

                var transitionEntity = world.CreateEntity();
                transitionEntities[i] = transitionEntity;

                StaticStash.Transition.Set(transitionEntity, new Transition {StateMachine = stateMachineEntity,});
                StaticStash.TransitionFrom.Set(transitionEntity, new TransitionFrom {State = transitionConfig.From,});
                StaticStash.TransitionTo.Set(transitionEntity, new TransitionTo {State = transitionConfig.To,});
                StaticStash.ParentEntity.Set(transitionEntity, new ParentEntity {Entity = stateMachineEntity,});

                if (transitionConfig.HaveExitTime())
                    StaticStash.TransitionExitTime.Set(transitionEntity,
                        new TransitionExitTime {Duration = transitionConfig.ExitTime,});

                if (transitionConfig.HaveCondition()) {
                    var conditionsCount = transitionConfig.NewConditions.Count;

                    StaticStash.ForwardConditions.Set(transitionEntity,
                        new ForwardConditions {List = new Entity[conditionsCount],});
                    StaticStash.ForwardConditionsToMeet.Set(transitionEntity,
                        new ForwardConditionsToMeet {Total = conditionsCount,});

                    for(var j = 0; j < transitionConfig.NewConditions.Count; j++) {
                        var forwardCondition = transitionConfig.NewConditions[j];
                        forwardCondition.CreateEntity(world, AbilityConditionFor.Forward, transitionEntity, user,
                            user, j);
                    }
                }

                if (transitionConfig.HaveAction())
                    AddActions(transitionConfig.Actions, world, transitionEntity, user);
            }

            for(var i = 0; i < stateMachineProvider.EnterActions.Count; i++) {
                var enterActionsConfig = stateMachineProvider.EnterActions[i];

                var enterActionEntity = world.CreateEntity();
                enterActionEntities[i] = enterActionEntity;

                StaticStash.EnterAction.Set(enterActionEntity, new EnterAction {State = enterActionsConfig.From,});
                StaticStash.ParentEntity.Set(enterActionEntity, new ParentEntity {Entity = stateMachineEntity,});

                AddActions(enterActionsConfig.Actions, world, enterActionEntity, user);
            }

            StaticStash.ChangeState.Set(stateMachineEntity,
                new ChangeState {NextState = stateMachineProvider.States[0],});
            StaticStash.StateMachineExitTime.Add(stateMachineEntity);

            return stateMachineEntity;
        }

        private static void AddActions(List<TransitionActionsConfig> actions,
                                       World world,
                                       Entity parentEntity,
                                       Entity userEntity)
        {
            foreach (var transitionAction in actions) {
                switch (transitionAction.Type) {
                    case TransitionActionType.Action: {
                        transitionAction.Actions.AddTo(parentEntity, userEntity, userEntity, world);
                        break;
                    }

                    case TransitionActionType.AbilityProvider: {
                        var abilitiesList = new List<Entity>(transitionAction.AbilityProviders.Count);

                        foreach (var abilityProvider in transitionAction.AbilityProviders)
                            abilitiesList.Add(abilityProvider.Create(userEntity, world));

                        StaticStash.Abilities.Set(parentEntity, new AbilitiesList {List = abilitiesList,});
                        break;
                    }

                    case TransitionActionType.NpcAction:
                        var npcActionsList = new List<Entity>();

                        foreach (var npcAction in transitionAction.NpcActions)
                            npcAction.AddTo(userEntity, world, npcActionsList);

                        StaticStash.TransitionNpcActions.Set(parentEntity,
                            new TransitionNpcActions {List = npcActionsList,});
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}