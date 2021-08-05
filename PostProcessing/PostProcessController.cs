using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.Core
{
	public class PostProcessController : MonoBehaviour
	{
		public Volume AddProfile(VolumeProfile profile)
		{
			Volume newVolume = gameObject.AddComponent<Volume>();
			newVolume.weight = 0;
			newVolume.profile = profile;
			return newVolume;
		}
	}
}
