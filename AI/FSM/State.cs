using UnityEngine;

namespace Fralle.Core.AI
{
	[CreateAssetMenu(menuName = "AI/State")]
	public class State : ScriptableObject
	{
		public Action[] actions;
		public Transition[] transitions;
		public Color sceneGizmoColor = Color.grey;

		public void UpdateState(IStateController controller)
		{
			DoActions(controller);
			CheckTransitions(controller);
		}

		private void DoActions(IStateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].Act(controller);
			}
		}

		private void CheckTransitions(IStateController controller)
		{
			for (int i = 0; i < transitions.Length; i++)
			{
				bool decisionSucceeded = transitions[i].decision.Decide(controller);

				if (decisionSucceeded)
				{
					controller.TransitionToState(transitions[i].trueState);
				}
				else
				{
					controller.TransitionToState(transitions[i].falseState);
				}
			}
		}

	}
}
