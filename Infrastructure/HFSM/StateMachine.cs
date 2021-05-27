using System;
using System.Collections.Generic;
using System.Linq;

namespace Fralle.Core.HFSM
{
	public class StateMachine
	{
		public IState CurrentState;

		readonly Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
		List<Transition> currentTransitions = new List<Transition>();
		readonly List<Transition> anyTransitions = new List<Transition>();

		static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

		public void Tick()
		{
			Transition transition = GetTransition();
			if (transition != null)
				SetState(transition.To);

			CurrentState?.OnLogic();
		}

		public void SetState(IState state)
		{
			if (state == CurrentState)
				return;

			CurrentState?.OnExit();
			CurrentState = state;

			transitions.TryGetValue(CurrentState.GetType(), out currentTransitions);
			if (currentTransitions == null)
				currentTransitions = EmptyTransitions;

			CurrentState.OnEnter();
		}

		public void AddTransition(IState from, IState to, Func<bool> predicate)
		{
			if (!this.transitions.TryGetValue(from.GetType(), out List<Transition> transitions))
			{
				transitions = new List<Transition>();
				this.transitions[from.GetType()] = transitions;
			}

			transitions.Add(new Transition(to, predicate));
		}

		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			anyTransitions.Add(new Transition(state, predicate));
		}

		Transition GetTransition()
		{
			Transition transition = anyTransitions.FirstOrDefault(t => t.Condition());
			return transition ?? currentTransitions.FirstOrDefault(t => t.Condition());
		}
	}
}
