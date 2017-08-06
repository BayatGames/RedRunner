using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Utilities;

namespace RedRunner.TerrainGeneration
{

	public class BackgroundBlock : Block
	{

		[SerializeField]
		protected float m_MinWidth = 1f;
		[SerializeField]
		protected float m_MaxWidth = 10f;

		public virtual float MinWidth {
			get {
				return m_MinWidth;
			}
		}

		public virtual float MaxWidth {
			get {
				return m_MaxWidth;
			}
		}

		public override float Width {
			get {
				return base.Width;
			}
			set {
				m_Width = value;
			}
		}

		protected virtual void Start ()
		{
			
		}

	}

}