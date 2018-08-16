using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

namespace RedRunner.Enemies
{

	public class Eye : MonoBehaviour
	{

		[SerializeField]
		protected float m_Radius = 1f;
		[SerializeField]
		protected Transform m_Pupil;
		[SerializeField]
		protected Transform m_Eyelid;
		[SerializeField]
		protected float m_MaximumDistance = 5f;
		[SerializeField]
		protected Character m_LatestCharacter;
		[SerializeField]
		protected Vector3 m_InitialPosition;
		[SerializeField]
		protected float m_Speed = 0.01f;
		[SerializeField]
		protected float m_DeadSpeed = 0.005f;
		[SerializeField]
		protected Vector3 m_DeadPosition;
		protected Vector3 m_PupilDestination;

		public virtual float Radius {
			get {
				return m_Radius;
			}
			set {
				m_Radius = value;
			}
		}

		public virtual Transform Pupil {
			get {
				return m_Pupil;
			}
		}

		public virtual Vector3 InitialPosition {
			get {
				return m_InitialPosition;
			}
		}

		public virtual Vector3 PupilDestination {
			get {
				return m_PupilDestination;
			}
		}

		public virtual float Speed {
			get {
				return m_Speed;
			}
		}

		protected virtual void Awake ()
		{
//			m_InitialPosition = m_Pupil.transform.position;
		}

		protected virtual void Update ()
		{
			Collider2D [] colliders = Physics2D.OverlapCircleAll ( transform.parent.position, m_MaximumDistance, LayerMask.GetMask ( "Characters" ) );
			for ( int i = 0; i < colliders.Length; i++ )
			{
				Character character = colliders [ i ].GetComponent<Character> ();
				if ( character != null )
				{
					m_LatestCharacter = character;
				}
			}
			SetupPupil ();
		}

		protected virtual void OnDrawGizmos ()
		{
			Gizmos.DrawWireSphere ( transform.position, m_Radius );
			Gizmos.DrawWireSphere ( transform.parent.position, m_MaximumDistance );
		}

		protected virtual void SetupPupil ()
		{
			if ( m_LatestCharacter != null )
			{
				float speed = m_Speed;
				Vector3 distanceToTarget = m_LatestCharacter.transform.position - m_Pupil.position;
				if ( m_LatestCharacter.IsDead.Value )
				{
					speed = m_DeadSpeed;
					distanceToTarget = Vector3.ClampMagnitude ( m_DeadPosition, m_Radius );
					Vector3 finalPupilPosition = transform.position + distanceToTarget;
					m_PupilDestination = finalPupilPosition;
				}
				else
				{
					float distance = Vector3.Distance ( m_LatestCharacter.transform.position, transform.parent.position );
					if ( distance <= m_MaximumDistance )
					{
						distanceToTarget = Vector3.ClampMagnitude ( distanceToTarget, m_Radius );
					}
					else
					{
						distanceToTarget = Vector3.ClampMagnitude ( m_InitialPosition, m_Radius );
					}
					Vector3 finalPupilPosition = transform.position + distanceToTarget;
					m_PupilDestination = finalPupilPosition;
				}
				m_Pupil.position = Vector3.MoveTowards ( m_Pupil.position, m_PupilDestination, speed );
			}
		}

	}

}