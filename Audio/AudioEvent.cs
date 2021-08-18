using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Core.Audio
{
  [CreateAssetMenu(menuName = "Audio/Event")]
  public class AudioEvent : ScriptableObject
  {
    public AudioClip[] Clips;

    public Vector2 VolumeRange;
    public Vector2 PitchRange;

    public int PlayCount = 1;
    public float PlayDelay = 1f;

    public void Play(AudioSource source)
    {
      if (Clips.Length == 0)
        return;

      source.clip = Clips[Random.Range(0, Clips.Length)];
      source.volume = VolumeRange.GetValueBetween();
      source.pitch = PitchRange.GetValueBetween();
      source.Play();
    }
  }
}
