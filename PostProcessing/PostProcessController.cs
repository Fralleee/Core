using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.Core
{
  public class PostProcessController : MonoBehaviour
  {
    public Volume AddProfile(VolumeProfile profile)
    {
      GameObject go = new GameObject(profile.name);
      go.transform.SetParent(transform);
      go.layer = gameObject.layer;

      Volume newVolume = go.AddComponent<Volume>();
      newVolume.weight = 0;
      newVolume.profile = profile;
      return newVolume;
    }
  }
}
