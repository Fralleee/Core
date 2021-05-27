using UnityEngine;

namespace Fralle.Core.AI
{
	public class StateController : MonoBehaviour, IStateController
	{
		public State currentState;
		public State remainState;

		[HideInInspector] public float stateTimeElapsed;

		void Update()
		{			
			currentState.UpdateState(this);
		}

		void OnDrawGizmos()
		{
			if (currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(transform.position, 2f);
			}
		}

		public void TransitionToState(State nextState)
		{
			if (nextState != remainState)
			{
				currentState = nextState;
				OnExitState();
			}
		}

		public bool CheckIfCountDownElapsed(float duration)
		{
			stateTimeElapsed += Time.deltaTime;
			return (stateTimeElapsed >= duration);
		}

		private void OnExitState()
		{
			stateTimeElapsed = 0;
		}
	}
}
