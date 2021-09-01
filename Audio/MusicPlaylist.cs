using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Audio
{
  [CreateAssetMenu(menuName = "Audio/MusicPlaylist")]
  public class MusicPlaylist : ScriptableObject
  {
    [FormerlySerializedAs("Songs")] public List<AudioClip> songs;
  }
}
