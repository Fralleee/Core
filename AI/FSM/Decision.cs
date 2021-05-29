using UnityEngine;

namespace Fralle.Core.AI
{
	public abstract class Decision : ScriptableObject
	{
		public abstract bool Decide(IStateController ctrl);
	}
}
