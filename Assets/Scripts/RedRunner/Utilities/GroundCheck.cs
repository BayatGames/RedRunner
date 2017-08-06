using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Utilities
{

	public class GroundCheck : MonoBehaviour
	{

		public delegate void GroundedHandler ();

		public event GroundedHandler OnGrounded;

		public const string GROUND_TAG = "Ground";
		public const string GROUND_LAYER_NAME = "Ground";

		[SerializeField]
		private Collider2D m_Collider2D;

		[SerializeField]
		private float m_RayDistance = 0.5f;

		public bool IsGrounded { get { return m_IsGrounded; } }

		private bool m_IsGrounded = false;

		void Awake ()
		{
			m_IsGrounded = false;
		}

		void Update ()
		{
			Vector2 left = new Vector2 (m_Collider2D.bounds.max.x, m_Collider2D.bounds.center.y);
			Vector2 center = new Vector2 (m_Collider2D.bounds.center.x, m_Collider2D.bounds.center.y);
			Vector2 right = new Vector2 (m_Collider2D.bounds.min.x, m_Collider2D.bounds.center.y);
		
			RaycastHit2D hit1 = Physics2D.Raycast (left, new Vector2 (0f, -1f), m_RayDistance, LayerMask.GetMask (GROUND_LAYER_NAME));
			Debug.DrawRay (left, new Vector2 (0f, -m_RayDistance));
			bool grounded1 = hit1 != null && hit1.collider != null && hit1.collider.CompareTag (GROUND_TAG);
		
			RaycastHit2D hit2 = Physics2D.Raycast (center, new Vector2 (0f, -1f), m_RayDistance, LayerMask.GetMask (GROUND_LAYER_NAME));
			Debug.DrawRay (center, new Vector2 (0f, -m_RayDistance));
			bool grounded2 = hit2 != null && hit2.collider != null && hit2.collider.CompareTag (GROUND_TAG);
		
			RaycastHit2D hit3 = Physics2D.Raycast (right, new Vector2 (0f, -1f), m_RayDistance, LayerMask.GetMask (GROUND_LAYER_NAME));
			Debug.DrawRay (right, new Vector2 (0f, -m_RayDistance));
			bool grounded3 = hit3 != null && hit3.collider != null && hit3.collider.CompareTag (GROUND_TAG);

			bool grounded = grounded1 || grounded2 || grounded3;
		
			if (grounded && !m_IsGrounded) {
				if (OnGrounded != null) {
					OnGrounded ();
				}
			}

			m_IsGrounded = grounded;
		}

	}

}