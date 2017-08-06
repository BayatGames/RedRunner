using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.TerrainGeneration
{

	[System.Serializable]
	public struct BackgroundLayer
	{
		
		public string name;
		public BackgroundBlock[] Blocks;
		public BackgroundBlock LastBlock;
		public float CurrentX;
		public float PreviousX;

		public void Reset ()
		{
			CurrentX = 0f;
			PreviousX = 0f;
			LastBlock = null;
		}

	}

}