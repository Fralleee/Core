using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	[ExecuteInEditMode]
	public class AITargetingSystem : MonoBehaviour
	{
		[Header("Configuration")]
		public float memorySpan = 4f;

		[Header("Weights")]
		public float distanceWeight = 1f;
		public float angleWeight = 1f;
		public float ageWeight = 1f;

		[Header("Debug")]
		public Color bestMemoryColor = Color.red;
		public Color memoryColor = Color.yellow;
		public bool Debug;

		AISensoryMemory memory = new AISensoryMemory();
		AIMemory bestMemory;
		AISensor sensor;

		public bool HasTarget => bestMemory != null;
		public GameObject Target => bestMemory.gameObject;
		public Vector3 TargetPosition => bestMemory.position;
		public bool TargetInSight => bestMemory.Age < 0.5f;
		public float TargetDistance => bestMemory.distance;

		void Awake()
		{
			sensor = GetComponent<AISensor>();
		}

		void Update()
		{
			memory.UpdateSenses(sensor);
			memory.ForgetMemories(memorySpan);
			EvaluateScores();
		}

		void EvaluateScores()
		{
			foreach (var memory in memory.memories)
			{
				memory.score = CalculateScore(memory);
				if (bestMemory == null || memory.score > bestMemory.score)
					bestMemory = memory;
			}
		}

		float Normalize(float value, float maxValue) => 1 - value / maxValue;

		float CalculateScore(AIMemory memory)
		{
			float distanceScore = Normalize(memory.distance, sensor.Distance) * distanceWeight;
			float angleScore = Normalize(memory.angle, sensor.Angle) * angleWeight;
			float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
			return distanceScore + angleScore + ageScore;
		}

		void OnDrawGizmos()
		{
			if (Debug)
			{
				float maxScore = float.MinValue;
				foreach (var memory in memory.memories)
				{
					maxScore = Mathf.Max(maxScore, memory.score);
				}

				foreach (var memory in memory.memories)
				{
					memoryColor.a = memory.score / maxScore;
					Gizmos.color = memoryColor;
					if (memory == bestMemory)
						Gizmos.color = bestMemoryColor;
					Gizmos.DrawSphere(memory.position, 0.25f);
				}
			}
		}
	}
}
