using Fralle.Core.Infrastructure;
using UnityEngine;

namespace Fralle.Core.Audio
{
  public class AudioSourcePool : PollingPool<AudioSource>
  {
    public AudioSourcePool(AudioSource prefab) : base(prefab)
    {
    }

    protected override bool IsActive(AudioSource component)
    {
      return component.isPlaying;
    }

    public AudioSource GetSource() => Get();
  }
}
