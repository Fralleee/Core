using System;

namespace Fralle.Core.HFSM
{
	public interface IState<T>
	{
		T identifier { get; }
		void OnEnter();
		void OnLogic();
		void OnExit();
	}
}
