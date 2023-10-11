using System;

namespace AI.ReactiveFiniteStateMachine
{
    public interface ITransition<in TTrigger> where TTrigger : Enum
    {
        bool CanTranslate(Type fromState, TTrigger trigger);
        Type TargetState { get; }
    }
}