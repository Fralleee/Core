using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	public class AISensoryMemory
	{
		public List<AIMemory> memories = new List<AIMemory>();

		public void UpdateSenses(AISensor sensor)
		{
			for (int i = 0; i < sensor.Objects.Count; i++)
			{
				GameObject target = sensor.Objects[i];
				RefreshMemory(sensor.gameObject, target);
			}
		}

		public void RefreshMemory(GameObject agent, GameObject target)
		{
			AIMemory memory = FetchMemory(target);
			memory.gameObject = target;
			memory.position = target.transform.position;
			memory.direction = target.transform.position - agent.transform.position;
			memory.distance = memory.direction.magnitude;
			memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
			memory.lastSeen = Time.time;
		}

		public AIMemory FetchMemory(GameObject gameObject)
		{
			AIMemory memory = memories.Find(x => x.gameObject == gameObject);
			if (memory == null)
			{
				memory = new AIMemory();
				memories.Add(memory);
			}

			return memory;
		}

		public void ForgetMemories(float olderThan)
		{
			memories.RemoveAll(m => m.Age > olderThan);
			memories.RemoveAll(m => !m.gameObject);
		}
	}
}
