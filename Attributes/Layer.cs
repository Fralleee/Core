using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	[System.Serializable]
	public class Layer
	{
		[SerializeField]
		private int layerIndex = 0;
		public int LayerIndex
		{
			get { return layerIndex; }
		}

		public void Set(int index)
		{
			if (index > 0 && index < 32)
			{
				layerIndex = index;
			}
		}

		public int Mask => 1 << layerIndex;
	}
}
