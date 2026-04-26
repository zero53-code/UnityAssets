using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero53.Fsm
{
    [Serializable]
    public class StateMachine
    {
        [SerializeReference] private List<IState> states = new();
        [SerializeReference] private List<IStateTransition> transitions = new();
        // private readonly Dictionary<IState, List<IStateTransition>> _transitionMap = new();

        public IState currentState { get; private set; }

        public void AddState(IState state)
        {
            if (states.Contains(state)) return;
            
            states.Add(state);
        }

        public IStateTransition AddTransition(IStateTransition transition)
        {
            if (transition is null) throw new ArgumentNullException(nameof(transition));
            if (transitions.Contains(transition)) return transition;
            
            transitions.Add(transition);
            
            return transition;
        }

        public IStateTransition AddTransition(IState from, IState to, TransitionCondition condition)
        {
            return AddTransition(new SingleStateTransition(from, to, condition));
        }

        public IStateTransition AddTransition(TransitionFromCondition fromCondition, IState to, TransitionCondition condition)
        {
            return new FromConditionStateTransition(fromCondition, to, condition);
        }

        public void Transfer(IState targetState)
        {
            if (ReferenceEquals(currentState, targetState)) return;
            if (!states.Contains(targetState)) return;
            
            currentState?.OnExit();
            
            currentState = targetState;
            currentState.OnEnter();
        }

        public void Transfer<T>() where T : IState
        {
            foreach (var state in states)
            {
                if (state is not T targetState) continue;
                
                Transfer(targetState);
                return;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (currentState == null) return;
            currentState.OnUpdate(deltaTime);
            
            // if (!_transitionMap.TryGetValue(currentState, out var transitionList)) return;
            // foreach (var transition in transitionList)
            // {
            //     if (!transition.Condition()) continue;
            //     Transfer(transition.to);
            //     return;
            // }

            foreach (var transition in transitions)
            {
                if (!transition.FromCondition(currentState)) continue;
                if (!transition.Condition()) continue;
                Transfer(transition.to);
                return;
            }
        }
    }
}