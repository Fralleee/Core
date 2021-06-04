using Fralle.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	public class TeamController : MonoBehaviour
	{
		public Layer Self;
		public Layer Hitbox;
		public LayerMask Hostiles;
		public LayerMask Neutrals;
		public LayerMask AllyProjectiles;
		public LayerMask HostileProjectiles;
		public LayerMask Hitboxes;
		public LayerMask AttackLayerMask;

		[ContextMenu("Setup")]
		public void Setup()
		{
			foreach (var col in GetComponentsInChildren<Collider>())
			{
				if (col.enabled)
					col.gameObject.layer = Hitbox.LayerIndex;
			}

			gameObject.layer = Self.LayerIndex;
		}
	}
}
