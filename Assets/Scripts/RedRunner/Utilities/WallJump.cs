using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Utilities
{

	public class WallJump : MonoBehaviour
	{
		public const string WALL_LAYER_NAME = "Ground";
		private int m_ClippedWalls = 0;

		[SerializeField]
		private Collider2D m_Collider2D;

		public bool TouchingWall { get { return m_TouchingWall; } }
		private bool m_TouchingWall = false;

		void Awake()
		{
			m_TouchingWall = false;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (LayerMask.LayerToName(collider.gameObject.layer) == WALL_LAYER_NAME)
			{
				m_ClippedWalls++;
				UpdateWallStatus();
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			if (LayerMask.LayerToName(collider.gameObject.layer) == WALL_LAYER_NAME)
			{
				m_ClippedWalls--;
				UpdateWallStatus();
			}
		}

		private void UpdateWallStatus()
		{
			m_TouchingWall = (m_ClippedWalls > 0);
		}
	}
}