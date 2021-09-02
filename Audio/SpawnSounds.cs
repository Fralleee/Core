﻿using System.Collections;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class SpawnSounds : MonoBehaviour
  {
    public GameObject prefabSound;

    public int spawnCount = 1;

    public float spawnDelay = 1f;

    public bool destroyWhenDone = true;

    [Range(0.01f, 10f)] public float pitchRandomMultiplier = 1f;

    void Awake()
    {
      Spawn();
    }

    public void Spawn()
    {
      if (spawnDelay > 0)
        StartCoroutine(SpawnSingle(spawnDelay));
      else
        SpawnSingle();
    }

    IEnumerator SpawnSingle(float time)
    {
      for (int i = 0; i < spawnCount; i++)
      {
        SpawnSingle();
        yield return new WaitForSeconds(time);
      }
    }

    void SpawnSingle()
    {
      GameObject sound = Instantiate(prefabSound, transform.position, Quaternion.identity);
      AudioSource source = sound.GetComponent<AudioSource>();

      Debug.Log($"Playing sound: {sound}");
      source.volume = 0.25f;

      if (!pitchRandomMultiplier.EqualsWithTolerance(1f))
      {
        if (Random.value < .5)
          source.pitch *= Random.Range(1 / pitchRandomMultiplier, 1);
        else
          source.pitch *= Random.Range(1, pitchRandomMultiplier);
      }

      if (!destroyWhenDone)
        return;

      float life = source.clip.length / source.pitch;
      Destroy(sound, life);
    }
  }
}
