using UnityEngine;

namespace Fralle.Core.AI
{
	[CreateAssetMenu(menuName = "AI/State")]
	public class State : ScriptableObject
	{
		public Action[] actions;
		public Transition[] transitions;
		public Color sceneGizmoColor = Color.grey;

		public void EnterState(IStateController controller)
		{
			for (int i = 0; i < actions.Length; i++) {
				actions[i] = Instantiate(actions[i]);
				actions[i].OnEnter(controller);
			}
		}

		public void UpdateState(IStateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].Tick(controller);

			CheckTransitions(controller);
		}

		public void ExitState(IStateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].OnExit(controller);
		}

		void CheckTransitions(IStateController controller)
		{
			for (int i = 0; i < transitions.Length; i++)
			{
				bool decisionSucceeded = transitions[i].decision.Decide(controller);
				if (decisionSucceeded)
				{
					if (!transitions[i].changeIfFalse)
					{
						controller.TransitionToState(transitions[i].state);
						break;
					}
				}
				else
				{
					if (transitions[i].changeIfFalse)
					{
						controller.TransitionToState(transitions[i].state);
						break;
					}
				}
			}
		}

	}
}
