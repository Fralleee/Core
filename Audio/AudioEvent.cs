using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Core.Audio
{
  [CreateAssetMenu(menuName = "Audio/Event")]
  public class AudioEvent : ScriptableObject
  {
    public AudioClip[] clips;

    public Vector2 volumeRange;
    public Vector2 pitchRange;

    public int playCount = 1;
    public float playDelay = 1f;

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
