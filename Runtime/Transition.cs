using System;

namespace AI.ReactiveFiniteStateMachine
{
    public class Transition<TTrigger> : ITransition<TTrigger> where TTrigger : Enum
    {
        private readonly Type _from;
        private readonly TTrigger _trigger;

        public Type TargetState { get; }

        public Transition(Type from, Type to, TTrigger trigger)
        {
            _from = from;
            TargetState = to;
            _trigger = trigger;
        }

        public bool CanTranslate(Type fromState, TTrigger trigger) => 
            _from == fromState && trigger.Equals(_trigger);
    }
}