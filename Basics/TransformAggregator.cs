using Fralle.PingTap;
using System.Linq;
using UnityEngine;

public class TransformAggregator : MonoBehaviour
{
	Transformer masterTransformer;
	Transformer[] transformers;

	Vector3 initPosition;
	Quaternion initRotation;

	void Awake()
	{
		initPosition = transform.localPosition;
		initRotation = transform.localRotation;

		Transformer[] tempTransformers = GetComponents<Transformer>();
		transformers = tempTransformers.Where(x => !x.master).ToArray();
		masterTransformer = tempTransformers.FirstOrDefault(x => x.master);
	}

	void Update()
	{
		Calculate();
	}

	void Calculate()
	{
		Vector3 combinedPosition = Vector3.zero;
		Quaternion combinedRotation = Quaternion.identity;

		foreach (Transformer transformer in transformers)
		{
			transformer.Calculate();
			combinedPosition += transformer.GetPosition();
			combinedRotation *= transformer.GetRotation();
		}

		if (masterTransformer)
		{
			masterTransformer.Calculate();
			transform.position = masterTransformer.GetPosition();
			transform.localPosition += combinedPosition;
			transform.localRotation *= combinedRotation;
		}
		else
		{
			transform.localPosition = initPosition + combinedPosition;
			transform.localRotation = initRotation * combinedRotation;
		}
	}
}
