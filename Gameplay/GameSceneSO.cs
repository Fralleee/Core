using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Gameplay
{
  [CreateAssetMenu(menuName = "Core/GameScene")]
  public class GameSceneSo : ScriptableObject
  {
    [FormerlySerializedAs("SceneName")] [Header("Information")]
    public string sceneName;
    [FormerlySerializedAs("ShortDescription")] public string shortDescription;

    [FormerlySerializedAs("Music")] [Header("Sounds")]
    public AudioClip music;

  }
}
