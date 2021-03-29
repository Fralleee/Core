using UnityEngine;

namespace Fralle.Core.Infrastructure
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T instance;

		public static T Instance
		{
			get
			{
				if (instance != null)
					return instance;

				instance = (T)FindObjectOfType(typeof(T));

				if (instance != null)
					return instance;

				var singletonObject = new GameObject();
				instance = singletonObject.AddComponent<T>();
				singletonObject.name = typeof(T).ToString() + " (Singleton)";

				DontDestroyOnLoad(singletonObject);

				return instance;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>")]
		protected virtual void Awake()
		{
			if (instance != null && instance != this as T)
			{
				Destroy(gameObject);
				return;
			}

			instance = this as T;
		}
	}
}
