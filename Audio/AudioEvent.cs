using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Fralle.Core.Audio
{
  [CreateAssetMenu(menuName = "Audio/Event")]
  public class AudioEvent : ScriptableObject
  {
    [FormerlySerializedAs("Clips")] public AudioClip[] clips;

    [FormerlySerializedAs("VolumeRange")] public Vector2 volumeRange;
    [FormerlySerializedAs("PitchRange")] public Vector2 pitchRange;

    [FormerlySerializedAs("PlayCount")] public int playCount = 1;
    [FormerlySerializedAs("PlayDelay")] public float playDelay = 1f;

    public void Play(AudioSource source)
    {
      if (clips.Length == 0)
        return;

      source.clip = clips[Random.Range(0, clips.Length)];
      source.volume = volumeRange.GetValueBetween();
      source.pitch = pitchRange.GetValueBetween();
      source.Play();
    }
  }
}
