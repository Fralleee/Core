using UnityEngine;

namespace Fralle.Core.AI
{
	public abstract class Action : ScriptableObject
	{
		public abstract void OnEnter(IStateController ctrl);
		public abstract void Tick(IStateController ctrl);
		public abstract void OnExit(IStateController ctrl);
	}
}
