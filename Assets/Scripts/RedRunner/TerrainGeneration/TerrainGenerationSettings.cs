using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Settings used for the terain generator
namespace RedRunner.TerrainGeneration
{
    // Establishes terrain generation settings class established
	[CreateAssetMenu (menuName = "Create Terrain Generator Settings")]
	public class TerrainGenerationSettings : ScriptableObject
	{

		[SerializeField]
		protected float m_LevelLength = 200f;
		[SerializeField]
		protected int m_StartBlocksCount = 1;
		[SerializeField]
		protected int m_MiddleBlocksCount = -1;
		[SerializeField]
		protected int m_EndBlocksCount = 1;
		[SerializeField]
		protected Block[] m_StartBlocks;                // Start Block Block.cs array variabl
        [SerializeField]
		protected Block[] m_MiddleBlocks;               // Middle Block Block.cs array variabl
        [SerializeField]
		protected Block[] m_EndBlocks;                  // End Block Block.cs array variable
		[SerializeField]
		protected BackgroundLayer[] m_BackgroundLayers; // Background Layer BackgroundLayer.cs array variable

        // Getter setting for Level Length 
		public float LevelLength {
			get {
				return m_LevelLength;
			}
		}

        // Getter setting for Start Block Count
		public int StartBlocksCount {
			get {
				return m_StartBlocksCount;
			}
		}

        // Getter setting for Middle Block Count
		public int MiddleBlocksCount {
			get {
				return m_MiddleBlocksCount;
			}
		}

        // Getter for End Block Count
		public int EndBlocksCount {
			get {
				return m_EndBlocksCount;
			}
		}

        // Get Start Blocks array element
		public Block[] StartBlocks {
			get {
				return m_StartBlocks;
			}
		}

        // Get Middle Blocks array element
		public Block[] MiddleBlocks {
			get {
				return m_MiddleBlocks;
			}
		}

        // Get End Blocks array element
		public Block[] EndBlocks {
			get {
				return m_EndBlocks;
			}
		}

        // Get Background Layer array element
		public BackgroundLayer[] BackgroundLayers {
			get {
				return m_BackgroundLayers;
			}
		}

	}

}