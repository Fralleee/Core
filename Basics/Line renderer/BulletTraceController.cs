using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTraceController : MonoBehaviour
{
	[SerializeField] float fadeoutTime = 0.2f;

	LineRenderer lineRenderer;

	float fadeoutTimer;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void Trace(Vector3 start, Vector3 end)
	{
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, end);
		lineRenderer.enabled = true;
		fadeoutTimer = fadeoutTime;
	}

	void Update()
	{
		if (fadeoutTimer <= 0)
			lineRenderer.enabled = false;

		fadeoutTimer -= Time.deltaTime;
	}
}
