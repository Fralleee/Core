using UnityEngine;

namespace Fralle.Core.Infrastructure
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T _instance;
		public static bool Destroyed;

		public static T Instance
		{
			get
			{
				if (Destroyed)
				{
					Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
					return null;
				}

				if (_instance != null)
					return _instance;

				_instance = (T)FindObjectOfType(typeof(T));

				if (_instance != null)
					return _instance;

				GameObject singletonObject = new GameObject();
				_instance = singletonObject.AddComponent<T>();
				singletonObject.name = typeof(T).ToString() + " (Singleton)";

				DontDestroyOnLoad(singletonObject);

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance != null && _instance != this as T)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this as T;
		}

		void OnApplicationQuit()
		{
			Destroyed = true;
		}

		protected virtual void OnDestroy()
		{
			Destroyed = true;
		}
	}
}
