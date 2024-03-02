using System;
using System.Collections.Generic;

namespace General
{
    public class StateMachine
    {
        private readonly Dictionary<Type, List<Transition>> _transitions;
        private readonly List<Transition> _anyTransitions;

        private IState _currentState;
        private List<Transition> _currentTransitions = new();

        private static readonly List<Transition> EmptyTransitions = new(0);

        private StateMachine(IState startState, Dictionary<Type, List<Transition>> transitions,
            List<Transition> anyTransitions)
        {
            _transitions = transitions;
            _anyTransitions = anyTransitions;
            SetState(startState);
        }

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            _currentState?.Tick();
        }

        private void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            _currentState.OnEnter();
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }

        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        public interface IState
        {
            void Tick();
            void OnEnter();
            void OnExit();
        }

        public class Builder
        {
            private readonly IState _startState;
            private readonly Dictionary<Type, List<Transition>> _transitions = new();
            private readonly List<Transition> _anyTransitions = new();

            public Builder(IState startState)
            {
                _startState = startState;
            }

            public Builder AddTransition(IState from, IState to, Func<bool> condition)
            {
                if (!_transitions.TryGetValue(from.GetType(), out var transitions))
                {
                    transitions = new List<Transition>();
                    _transitions[from.GetType()] = transitions;
                }

                transitions.Add(new Transition(to, condition));

                return this;
            }

            public Builder AddAnyTransition(IState state, Func<bool> condition)
            {
                _anyTransitions.Add(new Transition(state, condition));
                return this;
            }

            public StateMachine Build()
            {
                return new StateMachine(_startState, _transitions, _anyTransitions);
            }
        }
    }
}