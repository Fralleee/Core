using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	[ExecuteInEditMode]
	public class AITargetingSystem : MonoBehaviour
	{
		AISensoryMemory memory = new AISensoryMemory();
		AISensor sensor;

		[Header("Debug")]
		public Color targetColor = Color.red;
		public bool Debug;

		void Awake()
		{
			sensor = GetComponent<AISensor>();
		}

		void Update()
		{
			memory.UpdateSenses(sensor);
		}

		void OnDrawGizmos()
		{
			if (Debug)
			{
				Gizmos.color = targetColor;
				foreach (var m in memory.memories)
					Gizmos.DrawSphere(m.position, 0.2f);
			}
		}
	}
}
