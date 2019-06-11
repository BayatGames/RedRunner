using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Utilities;

// Details specific to the Background Block objects
// Assigned to prefabs in: Assets -> Prefabs -> Background Blocks
namespace RedRunner.TerrainGeneration
{
    // Extents Block.cs
	public class BackgroundBlock : Block
	{
        // Establish width min and max
		[SerializeField]
		protected float m_MinWidth = 1f;
		[SerializeField]
		protected float m_MaxWidth = 10f;

        // gets defined minimum width
		public virtual float MinWidth {
			get {
				return m_MinWidth;
			}
		}

        // gets defined maximum width
		public virtual float MaxWidth {
			get {
				return m_MaxWidth;
			}
		}

        // setter and getter to override blocks width settings
		public override float Width {
			get {
				return base.Width;
			}
			set {
				m_Width = value;
			}
		}

        // Behavior set up for backgroun block in start state
		protected virtual void Start ()
		{
			
		}

	}

}