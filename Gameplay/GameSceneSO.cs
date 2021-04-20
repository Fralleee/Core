using UnityEngine;

namespace Fralle.Core.Gameplay
{
	[CreateAssetMenu(menuName = "Core/GameScene")]
	public class GameSceneSo : ScriptableObject
	{
		[Header("Information")]
		public string SceneName;
		public string ShortDescription;

		[Header("Sounds")]
		public AudioClip Music;

	}
}
