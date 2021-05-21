﻿using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class BillboardSizeCamera : MonoBehaviour
	{
		Camera mainCamera;

		[SerializeField] float maxDistance = 50f;
		[SerializeField] Vector2 scaleLimits = new Vector2(0.5f, 1.5f);
		[SerializeField] Vector2 yOffsetLimits = new Vector2(0.5f, 1.5f);

		void Start()
		{
			mainCamera = Camera.main;
		}

		void LateUpdate()
		{
			if (!mainCamera)
				return;

			UpdateSize();
		}

		void UpdateSize()
		{
			float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
			float yPositionOffset = Mathf.Lerp(yOffsetLimits.x, yOffsetLimits.y, distance / maxDistance);
			transform.position = transform.position.With(y: yPositionOffset);

			float scale = Mathf.Lerp(scaleLimits.x, scaleLimits.y, distance / maxDistance);
			transform.localScale = Vector3.one * scale;
		}
	}
}
