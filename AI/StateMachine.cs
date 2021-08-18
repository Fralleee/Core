using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Core.AI
{
  public class StateMachine<T>
  {
    public event Action<IState<T>> OnTransition = delegate { };

    public IState<T> CurrentState;
    public float currentStateTime;

    readonly Dictionary<T, List<Transition<T>>> transitions = new Dictionary<T, List<Transition<T>>>();
    List<Transition<T>> currentTransitions = new List<Transition<T>>();
    readonly List<Transition<T>> anyTransitions = new List<Transition<T>>();

    static readonly List<Transition<T>> EmptyTransitions = new List<Transition<T>>(0);

    public void OnLogic()
    {
      currentStateTime += Time.deltaTime;
      Transition<T> transition = GetTransition;
      if (transition != null)
        SetState(transition.To);

      CurrentState?.OnLogic();
    }

    public void SetState(IState<T> state)
    {
      if (state == CurrentState)
        return;

      CurrentState?.OnExit();
      CurrentState = state;

      transitions.TryGetValue(CurrentState.identifier, out currentTransitions);
      if (currentTransitions == null)
        currentTransitions = EmptyTransitions;

      CurrentState.OnEnter();
      currentStateTime = 0f;
      OnTransition(CurrentState);
    }

    public void AddTransition(IState<T> from, IState<T> to, Func<bool> predicate)
    {
      if (!this.transitions.TryGetValue(from.identifier, out List<Transition<T>> transitions))
      {
        transitions = new List<Transition<T>>();
        this.transitions[from.identifier] = transitions;
      }

      transitions.Add(new Transition<T>(to, predicate));
    }


    public void At(IState<T> to, IState<T> from, Func<bool> condition) => AddTransition(to, from, condition);

    public void AddAnyTransition(IState<T> state, Func<bool> predicate)
    {
      anyTransitions.Add(new Transition<T>(state, predicate));
    }

    Transition<T> GetTransition => anyTransitions.FirstOrDefault(t => t.Condition()) ?? currentTransitions.FirstOrDefault(t => t.Condition());
  }
}
