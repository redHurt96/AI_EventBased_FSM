using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.ReactiveFiniteStateMachine
{
    public class StateMachine<TTrigger> 
        where TTrigger : Enum
    {
        private IState _current => _states[CurrentState];
        public Type CurrentState { get; private set; }

        private Dictionary<Type, IState> _states;
        private List<ITransition<TTrigger>> _transitions = new();

        public StateMachine<TTrigger> AddStates(IState startState, params IState[] states)
        {
            _states = states
                .Concat(new[] { startState })
                .ToDictionary(x => x.GetType(), y => y);
            
            CurrentState = startState.GetType();
            
            return this;
        }

        public StateMachine<TTrigger> AddTransition(Type from, Type to, TTrigger trigger)
        {
            _transitions.Add(new Transition<TTrigger>(from, to, trigger));
            return this;
        }

        public StateMachine<TTrigger> Run()
        {
            EnterCurrent();
            return this;
        }

        public void Update()
        {
            UpdateCurrentState();
        }

        public void HandleTrigger(TTrigger trigger)
        {
            ITransition<TTrigger> transition = _transitions
                .FirstOrDefault(x => x.CanTranslate(CurrentState, trigger));
            
            if (transition != null) 
                ChangeState(transition);
        }
        
        private void ChangeState(ITransition<TTrigger> transition)
        {
            ExitCurrent();
            SelectNext(transition);
            EnterCurrent();
        }
        
        private void UpdateCurrentState()
        {
            if (_current is IUpdateState updatable)
                updatable.OnUpdate();
        }
        
        private void ExitCurrent()
        {
            if (_current is IExitState exitState)
                exitState.OnExit();   
        }
        
        private void SelectNext(ITransition<TTrigger> byTransition)
        {
            Debug.Log($"{CurrentState.Name} -> <color=green>{byTransition.TargetState.Name}</color>");
            
            CurrentState = byTransition.TargetState;
        }
        
        private void EnterCurrent()
        {
            if (_current is IEnterState enterState)
                enterState.OnEnter();
        }
    }
}
