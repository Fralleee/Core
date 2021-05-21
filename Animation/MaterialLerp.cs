using UnityEngine;

namespace Fralle.Core.Animation
{
	public class MaterialLerp : MonoBehaviour
	{
		public Material Mat1;
		public Material Mat2;
		public Material Mat3;
		public Material Mat4;
		public Material Mat5;

		public float Lerptime;

		float q;
		float w;
		float e;
		float r;
		Renderer rend;

		void Start()
		{
			rend = GetComponent<Renderer>();

			rend.material = Mat1;
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Q))
			{

				rend.material.Lerp(Mat1, Mat2, q);
				q += Time.deltaTime / Lerptime;
			}

			if (Input.GetKey(KeyCode.W))
			{
				rend.material.Lerp(Mat2, Mat3, w);
				w += Time.deltaTime / Lerptime;
			}

			if (Input.GetKey(KeyCode.E))
			{
				rend.material.Lerp(Mat3, Mat4, e);
				e += Time.deltaTime / Lerptime;
			}

			if (Input.GetKey(KeyCode.R))
			{
				rend.material.Lerp(Mat4, Mat5, r);
				r += Time.deltaTime / Lerptime;
			}

		}
	}
}
