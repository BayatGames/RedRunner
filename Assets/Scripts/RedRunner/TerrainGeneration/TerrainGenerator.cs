using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

// Base class of the terrain generator. This contains all logic concerning how a
// level is created, how it is randomized, and what happens to it in different states.
namespace RedRunner.TerrainGeneration
{

	public abstract class TerrainGenerator : MonoBehaviour
	{
        // Setup and getter for terrain generator singleton
		private static TerrainGenerator m_Singleton;
		public static TerrainGenerator Singleton
		{
			get
			{
				return m_Singleton;
			}
		}

		protected Dictionary<Vector3, Block> m_Blocks;
		protected Dictionary<Vector3, BackgroundBlock> m_BackgroundBlocks;
		protected BackgroundLayer[] m_BackgroundLayers;     // Creates a Background Layer array object from BackgroundLayer.cs
		protected float m_PreviousX;
		protected float m_CurrentX;
		protected float m_FathestBackgroundX;
		[SerializeField]
		protected TerrainGenerationSettings m_Settings;     // Create a Terrain Generation Settings object from TerainGenerationSettings.cs
		protected int m_GeneratedStartBlocksCount;
		protected int m_GeneratedMiddleBlocksCount;
		protected int m_GeneratedEndBlocksCount;
		[SerializeField]
		protected float m_DestroyRange = 100f;
		[SerializeField]
		protected float m_GenerateRange = 100f;
		[SerializeField]
		protected float m_BackgroundGenerateRange = 200f;
		[SerializeField]
		protected Character m_Character;                    // Creates a character object that can be placed in the environment [Assets -> Scripts -> RedRunner -> Characters -> Character.cs]
		protected Block m_LastBlock;                        // Creates a Block object to find the last block
		protected BackgroundBlock m_LastBackgroundBlock;    // Creates a background Block object to define the last background block
		protected float m_RemoveTime = 0f;
		protected bool m_Reset = false;

        // Getter for previous X coordinate
		public float PreviousX
		{
			get
			{
				return m_PreviousX;
			}
		}

        // Getter for current X coordinate
		public float CurrentX
		{
			get
			{
				return m_CurrentX;
			}
		}

        // Terrain Generator settings getter ~> TerainGenerationSettings.cs
		public TerrainGenerationSettings Settings
		{
			get
			{
				return m_Settings;
			}
		}

        // Awake the Terrain & establishes background.
		protected virtual void Awake ()
		{
			if ( m_Singleton != null )
			{
				Destroy ( gameObject );
				return;
			}
			m_Singleton = this;
			m_Blocks = new Dictionary<Vector3, Block> ();
			m_BackgroundBlocks = new Dictionary<Vector3, BackgroundBlock> ();
			m_BackgroundLayers = new BackgroundLayer[m_Settings.BackgroundLayers.Length];
			for ( int i = 0; i < m_Settings.BackgroundLayers.Length; i++ )
			{
				m_BackgroundLayers [ i ] = m_Settings.BackgroundLayers [ i ];
			}
			GameManager.OnReset += Reset;
		}

        // Reset the Terrain to default positions and values
		protected virtual void Reset ()
		{
			m_Reset = true;
			RemoveAll ();
			m_CurrentX = 0f;
			m_LastBlock = null;
			m_LastBackgroundBlock = null;
			for ( int i = 0; i < m_BackgroundLayers.Length; i++ )
			{
				m_BackgroundLayers [ i ].Reset ();
			}
			m_FathestBackgroundX = 0f;
			m_Blocks.Clear ();
			m_BackgroundBlocks.Clear ();
			m_GeneratedStartBlocksCount = 0;
			m_GeneratedMiddleBlocksCount = 0;
			m_GeneratedEndBlocksCount = 0;
			m_Reset = false;
		}

        // When the Terrain layer is closed
		protected virtual void OnDestroy ()
		{
			m_Singleton = null;
		}

        // Update terrain if reset isnt called and there is still time. Call method ~> Generate()
		protected virtual void Update ()
		{
			if ( m_Reset )
			{
				return;
			}
			if ( m_RemoveTime < Time.time )
			{
				m_RemoveTime = Time.time + 5f;
				Remove ();
			}
			Generate ();
		}

        // Generate the terrains based on specific parameters.
		public virtual void Generate ()
		{
            // If your character has not reached the end of the level yet
			if ( m_CurrentX < m_Settings.LevelLength || m_Settings.LevelLength <= 0 )
			{
				bool isEnd = false, isStart = false, isMiddle = false;
				Block block = null;
				Vector3 current = new Vector3 ( m_CurrentX, 0f, 0f );
				float newX = 0f;

                // Determins what block you are currently for game status
				if ( m_GeneratedStartBlocksCount < m_Settings.StartBlocksCount || m_Settings.StartBlocksCount <= 0 )
				{
					isStart = true;
					block = ChooseFrom ( m_Settings.StartBlocks );
				}
				else if ( m_GeneratedMiddleBlocksCount < m_Settings.MiddleBlocksCount || m_Settings.MiddleBlocksCount <= 0 )
				{
					isMiddle = true;
					block = ChooseFrom ( m_Settings.MiddleBlocks );
				}
				else if ( m_GeneratedEndBlocksCount < m_Settings.EndBlocksCount || m_Settings.EndBlocksCount <= 0 )
				{
					isEnd = true;
					block = ChooseFrom ( m_Settings.EndBlocks );
				}

                // If you havent reached the last block yet, determin next block. Else, not.
				if ( m_LastBlock != null )
				{
					newX = m_CurrentX + m_LastBlock.Width;
				}
				else
				{
					newX = 0f;
				}

                // If there are still blocks ahead of player character (not reached the end), run this
				if ( block != null && ( m_LastBlock == null || newX < m_Character.transform.position.x + m_GenerateRange ) )
				{
					// Itterate specific block count.
                    if ( isStart )
					{
						if ( m_Settings.StartBlocksCount > 0 )
						{
							m_GeneratedStartBlocksCount++;
						}
					}
					else if ( isMiddle )
					{
						if ( m_Settings.MiddleBlocksCount > 0 )
						{
							m_GeneratedMiddleBlocksCount++;
						}
					}
					else if ( isEnd )
					{
						if ( m_Settings.EndBlocksCount > 0 )
						{
							m_GeneratedEndBlocksCount++;
						}
					}
                    // Call function ~> CreateBlock(Block, Vector3)
                    CreateBlock( block, current ); 
				}
			}

            // Update background layer and randomly generate elements based on location
			for ( int i = 0; i < m_BackgroundLayers.Length; i++ )
			{
				int random = Random.Range ( 0, 2 );
				bool generate = random == 1 ? true : false;
				if ( !generate )
				{
					continue;
				}
				Vector3 current = new Vector3 ( m_BackgroundLayers [ i ].CurrentX, 0f, 0f );
				BackgroundBlock block = ( BackgroundBlock )ChooseFrom ( m_BackgroundLayers [ i ].Blocks );
				float newX = 0f;
                // While the background layer is not the last block, else, background block.
				if ( m_BackgroundLayers [ i ].LastBlock != null )
				{
					newX = m_BackgroundLayers [ i ].CurrentX + m_BackgroundLayers [ i ].LastBlock.Width;
				}
				else
				{
					newX = 0f;
				}
                // While there are still blocks and is not the last block or the end of the level hasnt been reached
				if ( block != null && ( m_BackgroundLayers [ i ].LastBlock == null || newX < m_Character.transform.position.x + m_BackgroundGenerateRange ) )
				{
					CreateBackgroundBlock ( block, current, m_BackgroundLayers [ i ], i );
				}
			}
		}

        // Remove block from scene to help performance.
		public virtual void Remove ()
		{
			List<Block> blocksToRemove = new List<Block> ();
			foreach ( KeyValuePair<Vector3, Block> block in m_Blocks )
			{
				if ( block.Value.transform.position.x - m_CurrentX > m_DestroyRange )
				{
					blocksToRemove.Add ( block.Value );
				}
			}
			List<BackgroundBlock> backgroundBlocksToRemove = new List<BackgroundBlock> ();
			foreach ( KeyValuePair<Vector3, BackgroundBlock> block in m_BackgroundBlocks )
			{
				if ( block.Value.transform.position.x - m_FathestBackgroundX > m_DestroyRange )
				{
					backgroundBlocksToRemove.Add ( block.Value );
				}
			}
			for ( int i = 0; i < blocksToRemove.Count; i++ )
			{
				RemoveBlock ( blocksToRemove [ i ] );
			}
			for ( int i = 0; i < backgroundBlocksToRemove.Count; i++ )
			{
				RemoveBackgroundBlock ( backgroundBlocksToRemove [ i ] );
			}
		}

        // Remove all blocks from the scene
		public virtual void RemoveAll ()
		{
			List<Block> blocksToRemove = new List<Block> ();
			foreach ( KeyValuePair<Vector3, Block> block in m_Blocks )
			{
				blocksToRemove.Add ( block.Value );
			}
			List<BackgroundBlock> backgroundBlocksToRemove = new List<BackgroundBlock> ();
			foreach ( KeyValuePair<Vector3, BackgroundBlock> block in m_BackgroundBlocks )
			{
				backgroundBlocksToRemove.Add ( block.Value );
			}
			for ( int i = 0; i < blocksToRemove.Count; i++ )
			{
				RemoveBlock ( blocksToRemove [ i ] );
			}
			for ( int i = 0; i < backgroundBlocksToRemove.Count; i++ )
			{
				RemoveBackgroundBlock ( backgroundBlocksToRemove [ i ] );
			}
		}

        // Remove a specific block at indicated vector. Calls method ~> RemoveBlock(Block)
		public virtual void RemoveBlockAt ( Vector3 position )
		{
			RemoveBlock ( m_Blocks [ position ] );
		}

        // Remove specific block. Calls method ~> Destroy()
		public virtual void RemoveBlock ( Block block )
		{
			block.OnRemove ( this );
			Destroy ( m_Blocks [ block.transform.position ].gameObject );
			m_Blocks.Remove ( block.transform.position );
		}

        // Removes block from the background. Calls method ~> Destroy()
		public virtual void RemoveBackgroundBlock ( BackgroundBlock block )
		{
			block.OnRemove ( this );
			Destroy ( m_BackgroundBlocks [ block.transform.position ].gameObject );
			m_BackgroundBlocks.Remove ( block.transform.position );
		}

        // Creates a new block in the scene.
		public virtual bool CreateBlock ( Block blockPrefab, Vector3 position )
		{
			if ( blockPrefab == null )
			{
				return false;
			}
			blockPrefab.PreGenerate ( this );   // from Block.cs
			Block block = Instantiate<Block> ( blockPrefab, position, Quaternion.identity );
			m_PreviousX = m_CurrentX;
			m_CurrentX += block.Width;
			m_Blocks.Add ( position, block );
			blockPrefab.PostGenerate ( this );  // from Block.cs
			m_LastBlock = block;
			return true;
		}

        // Creates a new background block.
		public virtual bool CreateBackgroundBlock ( BackgroundBlock blockPrefab, Vector3 position, BackgroundLayer layer, int layerIndex )
		{
			if ( blockPrefab == null )
			{
				return false;
			}
			blockPrefab.PreGenerate ( this );   // from Block.cs
			position.z = blockPrefab.transform.position.z;
			position.y = blockPrefab.transform.position.y;
			BackgroundBlock block = Instantiate<BackgroundBlock> ( blockPrefab, position, Quaternion.identity );
			float width = Random.Range ( block.MinWidth, block.MaxWidth );  // from BackgroundBlock.cs
			m_BackgroundLayers [ layerIndex ].PreviousX = m_BackgroundLayers [ layerIndex ].CurrentX;
			m_BackgroundLayers [ layerIndex ].CurrentX += width;
			block.Width = width;
			m_BackgroundLayers [ layerIndex ].LastBlock = block;
			m_BackgroundBlocks.Add ( position, block );
			blockPrefab.PostGenerate ( this );  // from Block.cs
			if ( m_BackgroundLayers [ layerIndex ].CurrentX > m_FathestBackgroundX )
			{
				m_FathestBackgroundX = m_BackgroundLayers [ layerIndex ].CurrentX;
			}
			return true;
		}

        // Get character and places them in the scene.
		public Block GetCharacterBlock ()
		{
			Block characterBlock = null;
			foreach ( KeyValuePair<Vector3, Block> block in m_Blocks )
			{
				if ( block.Key.x <= m_Character.transform.position.x && block.Key.x + block.Value.Width > m_Character.transform.position.x )
				{
					characterBlock = block.Value;
					break;
				}
			}
			return characterBlock;
		}

        // Randomly selects block from array of possible blocks to choose from. Each block can only be used once.
		public static Block ChooseFrom ( Block[] blocks )
		{
			if ( blocks.Length <= 0 )
			{
				return null;
			}
			float total = 0;
			for ( int i = 0; i < blocks.Length; i++ )
			{
				total += blocks [ i ].Probability;
			}
			float randomPoint = Random.value * total;
			for ( int i = 0; i < blocks.Length; i++ )
			{
				if ( randomPoint < blocks [ i ].Probability )
				{
					return blocks [ i ];
				}
				else
				{
					randomPoint -= blocks [ i ].Probability;
				}
			}
			return blocks [ blocks.Length - 1 ];
		}

	}

}