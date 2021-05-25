using UnityEngine;

namespace Fralle.Core.Animation
{
	public class FadeoutLineRenderer : MonoBehaviour
	{
		public float FadeoutTime;

		float fadeTimer;
		LineRenderer lineRenderer;
		Color currentColor;

		void Awake()
		{
			lineRenderer = GetComponent<LineRenderer>();
			currentColor = lineRenderer.material.color;
			fadeTimer = FadeoutTime;
		}

		void OnEnable()
		{
			fadeTimer = FadeoutTime;
		}

		void Update()
		{
			fadeTimer -= Time.deltaTime;

			Color newColor = Color.Lerp(currentColor, new Color(1f, 1f, 1f, 0f), 1 - (fadeTimer / FadeoutTime));
			lineRenderer.material.color = newColor;
		}
	}
}
