using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Fralle.Core
{
	public class ToggleIK : MonoBehaviour
	{
		ChainIKConstraint chainIKConstraint;

		void Awake()
		{
			chainIKConstraint = GetComponentInParent<ChainIKConstraint>();
		}

		public void Toggle(bool enabled = true)
		{
			chainIKConstraint.weight = enabled ? 1 : 0;
		}
	}
}
