using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Details specific to the Block objects
// Assigned to prefabs in: Assets -> Prefabs -> Blocks
namespace RedRunner.TerrainGeneration
{

	public abstract class Block : MonoBehaviour
	{

		[SerializeField]
		protected float m_Width;
		[SerializeField]
		protected float m_Probability = 1f;

        // Setter and getter for the blocks width
		public virtual float Width {
			get {
				return m_Width;
			}
			set {
				m_Width = value;
			}
		}

        // Getter for blocks probability
		public virtual float Probability {
			get {
				return m_Probability;
			}
		}

        // Behavior setup for the moment a block is removed.
		public virtual void OnRemove (TerrainGenerator generator)
		{
			
		}

        // Behavior setup for time prior to the blocks removal.
		public virtual void PreGenerate (TerrainGenerator generator)
		{
			
		}

        // Behavior setup to time after the blocks removal.
		public virtual void PostGenerate (TerrainGenerator generator)
		{
			
		}

	}

}