using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;
using RedRunner.Utilities;

namespace RedRunner.Enemies
{

	public class Mace : Enemy
	{

		[SerializeField]
		protected Collider2D m_Collider2D;
		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected PathFollower m_PathFollower;
		[SerializeField]
		protected float m_MaulSpeed = 0.5f;
		[SerializeField]
		protected float m_MaulScale = 0.8f;
		[SerializeField]
		protected ParticleSystem m_ParticleSystem;

		public override Collider2D Collider2D {
			get {
				return m_Collider2D;
			}
		}

		protected virtual void Awake ()
		{
			GameManager.OnReset += Reset;
		}

		protected virtual void OnDestroy ()
		{
			GameManager.OnReset -= Reset;
		}

		void Reset ()
		{
			m_Animator.SetTrigger ("Reset");
			m_PathFollower.Stopped = false;
		}

		void OnCollisionEnter2D (Collision2D collision2D)
		{
			Vector2 position = collision2D.contacts [0].point;
			Character character = collision2D.collider.GetComponent<Character> ();
			bool pressable = false;
			for (int i = 0; i < collision2D.contacts.Length; i++) {
				if (!pressable) {
					pressable = (collision2D.contacts [i].normal.y >= 0.8f && collision2D.contacts [i].normal.y <= 1f && m_PathFollower.Velocity.y > m_MaulSpeed) ||
					(collision2D.contacts [i].normal.y <= -0.8f && collision2D.contacts [i].normal.y >= -1f && m_PathFollower.Velocity.y < m_MaulSpeed) ||
					(collision2D.contacts [i].normal.x >= 0.8f && collision2D.contacts [i].normal.x <= 1f && m_PathFollower.Velocity.x < m_MaulSpeed) ||
					(collision2D.contacts [i].normal.x <= -0.8f && collision2D.contacts [i].normal.x >= -1f && m_PathFollower.Velocity.x > m_MaulSpeed);
				} else {
					break;
				}
			}
			if (pressable && character == null && !collision2D.collider.CompareTag ("Player")) {
				Slam (position);
			}
			if (character != null && !character.IsDead.Value) {
				if (pressable) {
					Slam (position);
					Vector3 scale = character.transform.localScale;
					scale.y = m_MaulScale;
					character.transform.localScale = scale;
				}
				Kill (character);
			}
//			Camera.main.GetComponent<CameraControl> ().Shake (3f, 30, 300f);
		}

		public virtual void Slam (Vector3 position)
		{
			AudioManager.Singleton.PlayMaceSlamSound (transform.position);
			ParticleSystem particle = Instantiate<ParticleSystem> (m_ParticleSystem, position, m_ParticleSystem.transform.rotation);
			Destroy (particle.gameObject, particle.main.duration);
		}

		public override void Kill (Character target)
		{
			m_PathFollower.Stopped = true;
			target.Die (true);
			m_Animator.SetTrigger ("Smile");
			AudioManager.Singleton.PlaySpikeSound (transform.position);
		}

	}

}