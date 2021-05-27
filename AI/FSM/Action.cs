using UnityEngine;

namespace Fralle.Core.AI
{
	public abstract class Action : ScriptableObject
	{
		public abstract void Act(IStateController controller);
	}
}
