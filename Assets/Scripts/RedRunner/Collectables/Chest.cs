using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

namespace RedRunner.Collectables
{

	public class Chest : Collectable
	{

		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected Collider2D m_Collider2D;
		[SerializeField]
		protected SpriteRenderer m_SpriteRenderer;
		[SerializeField]
		protected bool m_UseOnTriggerEnter2D = true;
		[SerializeField]
		protected int m_MinimumCoins = 5;
		[SerializeField]
		protected int m_MaximumCoins = 10;
		[SerializeField]
		protected CoinRigidbody2D m_CoinRigidbody2D;
		[SerializeField]
		protected Transform m_SpawnPoint;
		[SerializeField]
		protected ParticleSystem m_ParticleSystem;
		[SerializeField]
		[Range (-100f, 100f)]
		protected float m_RandomForceYMinimum = -10f;
		[SerializeField]
		[Range (-100f, 100f)]
		protected float m_RandomForceYMaximum = 10f;
		[SerializeField]
		[Range (-100f, 100f)]
		protected float m_RandomForceXMinimum = -10f;
		[SerializeField]
		[Range (-100f, 100f)]
		protected float m_RandomForceXMaximum = 10f;

		protected Character m_CurrentCharacter;

		public override Animator Animator {
			get {
				return m_Animator;
			}
		}

		public override Collider2D Collider2D {
			get {
				return m_Collider2D;
			}
		}

		public override SpriteRenderer SpriteRenderer {
			get {
				return m_SpriteRenderer;
			}
		}

		public override bool UseOnTriggerEnter2D {
			get {
				return m_UseOnTriggerEnter2D;
			}
			set {
				m_UseOnTriggerEnter2D = value;
			}
		}

		public virtual int MinimumCoins {
			get {
				return m_MinimumCoins;
			}
		}

		public virtual int MaximumCoins {
			get {
				return m_MaximumCoins;
			}
		}

		public virtual CoinRigidbody2D CoinRigidbody2D {
			get {
				return m_CoinRigidbody2D;
			}
		}

		public virtual Transform SpawnPoint {
			get {
				return m_SpawnPoint;
			}
		}

		public virtual ParticleSystem ParticleSystem {
			get {
				return m_ParticleSystem;
			}
		}

		public virtual float RandomForceYMinimum {
			get {
				return m_RandomForceYMinimum;
			}
			set {
				m_RandomForceYMinimum = value;
			}
		}

		public virtual float RandomForceYMaximum {
			get {
				return m_RandomForceYMaximum;
			}
			set {
				m_RandomForceYMaximum = value;
			}
		}

		public virtual float RandomForceXMinimum {
			get {
				return m_RandomForceXMinimum;
			}
			set {
				m_RandomForceXMinimum = value;
			}
		}

		public virtual float RandomForceXMaximum {
			get {
				return m_RandomForceXMaximum;
			}
			set {
				m_RandomForceXMaximum = value;
			}
		}

		public override void OnCollisionEnter2D (Collision2D collision2D)
		{
			Character character = collision2D.collider.GetComponent<Character> ();
			if (!m_UseOnTriggerEnter2D && character != null) {
				m_CurrentCharacter = character;
				Collect ();
			}
		}

		public override void OnTriggerEnter2D (Collider2D other)
		{
			Character character = other.GetComponent<Character> ();
			if (m_UseOnTriggerEnter2D && character != null) {
				m_CurrentCharacter = character;
				Collect ();
			}
		}

		public override void Collect ()
		{
			m_Animator.SetBool ("Open", true);
		}

		public virtual void OnChestOpened ()
		{
			AudioManager.Singleton.PlayChestSound (transform.position);
			m_ParticleSystem.Play ();
			int coinsCount = Random.Range (m_MinimumCoins, m_MaximumCoins);
			for (int i = 0; i < coinsCount; i++) {
				CoinRigidbody2D coin = Instantiate<CoinRigidbody2D> (m_CoinRigidbody2D, m_SpawnPoint.position, Quaternion.identity, transform);
				float x = Random.Range (m_RandomForceXMinimum, m_RandomForceXMaximum);
				float y = Random.Range (m_RandomForceYMinimum, m_RandomForceYMaximum);
				Vector2 force = new Vector2 (x, y);
				coin.Rigidbody2D.AddForce (force, ForceMode2D.Impulse);
				StartCoroutine (IgnoreAndEnableCollision (m_CurrentCharacter.Collider2D, coin.Collider2D));
			}
		}

		protected virtual IEnumerator IgnoreAndEnableCollision (Collider2D collider2D1, Collider2D collider2D2)
		{
			Physics2D.IgnoreCollision (collider2D1, collider2D2, true);
			yield return new WaitForSeconds (0.3f);
			Physics2D.IgnoreCollision (collider2D1, collider2D2, false);
		}
	
	}

}