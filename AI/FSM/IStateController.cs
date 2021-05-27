namespace Fralle.Core.AI
{
	public interface IStateController
	{
		void TransitionToState(State nextState);
	}
}
