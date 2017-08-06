using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

namespace RedRunner.Utilities
{

	public class PathFollower : MonoBehaviour
	{

		[SerializeField]
		protected PathDefinition m_PathDefinition;
		[SerializeField]
		protected float m_Step = 0.1f;
		[SerializeField]
		protected bool m_FollowSpeeds = true;
		[SerializeField]
		protected float m_Speed = 1f;
		[SerializeField]
		protected bool m_FollowDelays = true;
		[SerializeField]
		protected float m_Delay = 0f;
		[SerializeField]
		protected bool m_Smart = false;
		[SerializeField]
		protected Vector3 m_RangeSize = new Vector3 (4f, 4f, 4f);
		[SerializeField]
		protected Vector3 m_RangeOffset = new Vector3 (0f, 0f, 0f);

		protected bool m_Stopped = false;
		protected IEnumerator<PathPoint> m_CurrentPoint;
		protected bool m_IsMovingNext = false;
		protected Vector3 m_LastPosition;
		protected Vector3 m_Velocity;
		protected Vector3 m_SmoothVelocity;
		protected float m_OverTimeSpeed = 0f;

		public virtual bool Stopped {
			get {
				return m_Stopped;
			}
			set {
				m_Stopped = value;
			}
		}

		public virtual Vector3 Velocity {
			get {
				return m_Velocity;
			}
		}

		void Awake ()
		{
			m_Stopped = m_Smart;
		}

		void Start ()
		{
			if (m_PathDefinition == null) {
				return;
			}

			m_CurrentPoint = m_PathDefinition.GetPathEnumerator ();
			m_CurrentPoint.MoveNext ();

			if (m_CurrentPoint.Current == null)
				return;

			transform.position = m_CurrentPoint.Current.transform.position;
			StartCoroutine (CalcVelocity ());
		}

		void OnDrawGizmos ()
		{
			if (m_Smart) {
				Gizmos.DrawWireCube (transform.position + m_RangeOffset, m_RangeSize);
			}
		}

		void Update ()
		{
			if (m_Smart) {
				Collider2D[] colliders = Physics2D.OverlapBoxAll (transform.position + m_RangeOffset, m_RangeSize, 0f, LayerMask.GetMask ("Characters"));
				for (int i = 0; i < colliders.Length; i++) {
					Character character = colliders [i].GetComponent<Character> ();
					if (character != null) {
						m_Stopped = false;
					}
				}
			} else {
				m_Stopped = false;
			}

			if (m_CurrentPoint == null || m_CurrentPoint.Current == null || m_Stopped || !GameManager.Singleton.gameRunning) {
				return;
			}

			float speed = Time.deltaTime * m_CurrentPoint.Current.speed;
			if (m_CurrentPoint.Current.moveType == PathPoint.MoveType.MoveTowards) {
				transform.position = Vector3.MoveTowards (transform.position, m_CurrentPoint.Current.transform.position, speed);
			} else if (m_CurrentPoint.Current.moveType == PathPoint.MoveType.Lerp) {
				transform.position = Vector3.Lerp (transform.position, m_CurrentPoint.Current.transform.position, speed);
			} else if (m_CurrentPoint.Current.moveType == PathPoint.MoveType.SmoothDamp) {
				transform.position = Vector3.SmoothDamp (transform.position, m_CurrentPoint.Current.transform.position, ref m_SmoothVelocity, m_CurrentPoint.Current.smoothTime);
			} else if (m_CurrentPoint.Current.moveType == PathPoint.MoveType.Acceleration) {
				Vector3 direction = (m_CurrentPoint.Current.transform.position - transform.position).normalized;
				transform.position = Vector3.MoveTowards (transform.position, m_CurrentPoint.Current.transform.position, m_OverTimeSpeed);
				m_OverTimeSpeed += m_CurrentPoint.Current.acceleration;
				if (m_OverTimeSpeed > m_CurrentPoint.Current.maxSpeed) {
					m_OverTimeSpeed = m_CurrentPoint.Current.maxSpeed;
				}
			}

			var distanceSquared = (transform.position - m_CurrentPoint.Current.transform.position).sqrMagnitude;
			if (distanceSquared < m_Step * m_Step) {
				m_Stopped = true;
				if (!m_IsMovingNext) {
					StartCoroutine (MoveNext ());
				}
			}
		}

		IEnumerator CalcVelocity ()
		{
			while (Application.isPlaying) {
				m_LastPosition = transform.position;
				yield return new WaitForEndOfFrame ();
				m_Velocity = (m_LastPosition - transform.position) / Time.deltaTime;
			}
		}

		IEnumerator MoveNext ()
		{
			m_IsMovingNext = true;
			float delay = m_CurrentPoint.Current.delay;
			if (m_FollowDelays && m_PathDefinition.UseGlobalDelay) {
				delay = m_PathDefinition.GlobalDelay;
			} else if (!m_FollowDelays) {
				delay = m_Delay;
			}
			yield return new WaitForSeconds (delay);
			m_OverTimeSpeed = 0f;
			m_CurrentPoint.MoveNext ();
			m_IsMovingNext = false;
		}

	}

}