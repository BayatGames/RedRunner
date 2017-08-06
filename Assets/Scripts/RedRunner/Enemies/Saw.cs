using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

namespace RedRunner.Enemies
{

	public class Saw : Enemy
	{

		[SerializeField]
		private Collider2D m_Collider2D;
		[SerializeField]
		private Transform targetRotation;
		[SerializeField]
		private float m_Speed = 1f;
		[SerializeField]
		private bool m_RotateClockwise = false;
		[SerializeField]
		private AudioClip m_DefaultSound;
		[SerializeField]
		private AudioClip m_SawingSound;
		[SerializeField]
		private AudioSource m_AudioSource;

		public override Collider2D Collider2D {
			get {
				return m_Collider2D;
			}
		}

		void Start ()
		{
			if (targetRotation == null) {
				targetRotation = transform;
			}
		}

		void Update ()
		{
			Vector3 rotation = targetRotation.rotation.eulerAngles;
			if (!m_RotateClockwise) {
				rotation.z += m_Speed;
			} else {
				rotation.z -= m_Speed;
			}
			targetRotation.rotation = Quaternion.Euler (rotation);
		}

		void OnCollisionEnter2D (Collision2D collision2D)
		{
			Character character = collision2D.collider.GetComponent<Character> ();
			if (character != null) {
				Kill (character);
			}
		}

		void OnCollisionStay2D (Collision2D collision2D)
		{
			if (collision2D.collider.CompareTag ("Player")) {
				if (m_AudioSource.clip != m_SawingSound) {
					m_AudioSource.clip = m_SawingSound;
				} else if (!m_AudioSource.isPlaying) {
					m_AudioSource.Play ();
				}
			}
		}

		void OnCollisionExit2D (Collision2D collision2D)
		{
			if (collision2D.collider.CompareTag ("Player")) {
				if (m_AudioSource.clip != m_DefaultSound) {
					m_AudioSource.clip = m_DefaultSound;
				}
				m_AudioSource.Play ();
			}
		}

		public override void Kill (Character target)
		{
			target.Die (true);
		}

	}

}