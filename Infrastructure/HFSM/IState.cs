namespace Fralle.Core.HFSM
{
	public interface IState
	{
		void OnEnter();
		void OnLogic();
		void OnExit();
	}
}
