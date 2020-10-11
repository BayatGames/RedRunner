using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that is called by TerrainGenerator.cs which handles the background layer
namespace RedRunner.TerrainGeneration
{

	[System.Serializable]
	public struct BackgroundLayer
	{
		
		public string name;
		public BackgroundBlock[] Blocks;    // Creates a Background Block array object to hold all of the level blocks
		public BackgroundBlock LastBlock;   // Creates a Background Block object to define the last block
		public float CurrentX;
		public float PreviousX;

        // Sets the background block to default position on reset
		public void Reset ()
		{
			CurrentX = 0f;
			PreviousX = 0f;
			LastBlock = null;
		}

	}

}