using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;
using RedRunner.Utilities;

namespace RedRunner.Enemies
{
	[CreateAssetMenu]
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
			Character character = collision2D.collider.GetComponent<Character> ();
			if (character != null && !character.IsDead.Value) {
				Vector3 scale = character.transform.localScale;
				scale.y = m_MaulScale;
				character.transform.localScale = scale;
				Kill (character);
			}
			Camera.main.GetComponent<CameraControl> ().Shake (3f, 30, 300f);
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
			base.Kill(target);
			m_Animator.SetTrigger ("Smile");
			AudioManager.Singleton.PlaySpikeSound (transform.position);
		}

	}

}