using Fralle.Core.Extensions;
using System.Collections;
using UnityEngine;

namespace Fralle.Core.Audio
{
	public class SpawnSounds : MonoBehaviour
	{
		public GameObject PrefabSound;

		public int SpawnCount = 1;

		public float SpawnDelay = 1f;

		public bool DestroyWhenDone = true;

		[Range(0.01f, 10f)] public float PitchRandomMultiplier = 1f;

		void Awake()
		{
			Spawn();
		}

		public void Spawn()
		{
			if (SpawnDelay > 0)
				StartCoroutine(SpawnSingle(SpawnDelay));
			else
				SpawnSingle();
		}

		IEnumerator SpawnSingle(float time)
		{
			for (var i = 0; i < SpawnCount; i++)
			{
				SpawnSingle();
				yield return new WaitForSeconds(time);
			}
		}

		void SpawnSingle()
		{
			var sound = Instantiate(PrefabSound, transform.position, Quaternion.identity);
			var source = sound.GetComponent<AudioSource>();

			Debug.Log($"Playing sound: {sound}");
			source.volume = 0.25f;

			if (!PitchRandomMultiplier.EqualsWithTolerance(1f))
			{
				if (Random.value < .5)
					source.pitch *= Random.Range(1 / PitchRandomMultiplier, 1);
				else
					source.pitch *= Random.Range(1, PitchRandomMultiplier);
			}

			if (!DestroyWhenDone)
				return;

			var life = source.clip.length / source.pitch;
			Destroy(sound, life);
		}
	}
}
