using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityStandardAssets.CrossPlatformInput;

using RedRunner.Utilities;

namespace RedRunner.Characters
{

	public class RedCharacter : Character
	{
		#region Fields

		[Header ( "Character Details" )]
		[Space]
		[SerializeField]
		protected float m_MaxRunSpeed = 8f;
		[SerializeField]
		protected float m_RunSmoothTime = 5f;
		[SerializeField]
		protected float m_RunSpeed = 5f;
		[SerializeField]
		protected float m_WalkSpeed = 1.75f;
		[SerializeField]
		protected float m_JumpStrength = 10f;
		[SerializeField]
		protected string[] m_Actions = new string[0];
		[SerializeField]
		protected int m_CurrentActionIndex = 0;

		[Header ( "Character Reference" )]
		[Space]
		[SerializeField]
		protected Rigidbody2D m_Rigidbody2D;
		[SerializeField]
		protected Collider2D m_Collider2D;
		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected GroundCheck m_GroundCheck;
		[SerializeField]
		protected ParticleSystem m_RunParticleSystem;
		[SerializeField]
		protected ParticleSystem m_JumpParticleSystem;
		[SerializeField]
		protected ParticleSystem m_WaterParticleSystem;
		[SerializeField]
		protected ParticleSystem m_BloodParticleSystem;
		[SerializeField]
		protected Skeleton m_Skeleton;
		[SerializeField]
		protected float m_RollForce = 10f;

		[Header ( "Character Audio" )]
		[Space]
		[SerializeField]
		protected AudioSource m_MainAudioSource;
		[SerializeField]
		protected AudioSource m_FootstepAudioSource;
		[SerializeField]
		protected AudioSource m_JumpAndGroundedAudioSource;

		#endregion

		#region Private Variables

		protected bool m_ClosingEye = false;
		protected bool m_Guard = false;
		protected bool m_Block = false;
		protected Vector2 m_Speed = Vector2.zero;
		protected float m_CurrentRunSpeed = 0f;
		protected float m_CurrentSmoothVelocity = 0f;
		protected int m_CurrentFootstepSoundIndex = 0;
		protected Vector3 m_InitialScale;
		protected Vector3 m_InitialPosition;

		#endregion

		#region Properties

		public override float MaxRunSpeed
		{
			get
			{
				return m_MaxRunSpeed;
			}
		}

		public override float RunSmoothTime
		{
			get
			{
				return m_RunSmoothTime;
			}
		}

		public override float RunSpeed
		{
			get
			{
				return m_RunSpeed;
			}
		}

		public override float WalkSpeed
		{
			get
			{
				return m_WalkSpeed;
			}
		}

		public override float JumpStrength
		{
			get
			{
				return m_JumpStrength;
			}
		}

		public override Vector2 Speed
		{
			get
			{
				return m_Speed;
			}
		}

		public override string[] Actions
		{
			get
			{
				return m_Actions;
			}
		}

		public override string CurrentAction
		{
			get
			{
				return m_Actions [ m_CurrentActionIndex ];
			}
		}

		public override int CurrentActionIndex
		{
			get
			{
				return m_CurrentActionIndex;
			}
		}

		public override GroundCheck GroundCheck
		{
			get
			{
				return m_GroundCheck;
			}
		}

		public override Rigidbody2D Rigidbody2D
		{
			get
			{
				return m_Rigidbody2D;
			}
		}

		public override Collider2D Collider2D
		{
			get
			{
				return m_Collider2D;
			}
		}

		public override Animator Animator
		{
			get
			{
				return m_Animator;
			}
		}

		public override ParticleSystem RunParticleSystem
		{
			get
			{
				return m_RunParticleSystem;
			}
		}

		public override ParticleSystem JumpParticleSystem
		{
			get
			{
				return m_JumpParticleSystem;
			}
		}

		public override ParticleSystem WaterParticleSystem
		{
			get
			{
				return m_WaterParticleSystem;
			}
		}

		public override ParticleSystem BloodParticleSystem
		{
			get
			{
				return m_BloodParticleSystem;
			}
		}

		public override Skeleton Skeleton
		{
			get
			{
				return m_Skeleton;
			}
		}

        public override bool ClosingEye
		{
			get
			{
				return m_ClosingEye;
			}
		}

		public override bool Guard
		{
			get
			{
				return m_Guard;
			}
		}

		public override bool Block
		{
			get
			{
				return m_Block;
			}
		}

		public override AudioSource Audio
		{
			get
			{
				return m_MainAudioSource;
			}
		}

		#endregion

		#region MonoBehaviour Messages

		void Awake ()
		{
			m_InitialPosition = transform.position;
			m_InitialScale = transform.localScale;
			m_GroundCheck.OnGrounded += GroundCheck_OnGrounded;
			m_Skeleton.OnActiveChanged += Skeleton_OnActiveChanged;
            IsDead = new Property<bool>(false);
			m_ClosingEye = false;
			m_Guard = false;
			m_Block = false;
			m_CurrentFootstepSoundIndex = 0;
			GameManager.OnReset += GameManager_OnReset;
		}

		void Update ()
		{
			if ( !GameManager.Singleton.gameStarted || !GameManager.Singleton.gameRunning )
			{
				return;
			}

			if ( transform.position.y < 0f )
			{
				Die ();
			}

			// Speed
			m_Speed = new Vector2 ( Mathf.Abs ( m_Rigidbody2D.velocity.x ), Mathf.Abs ( m_Rigidbody2D.velocity.y ) );

			// Speed Calculations
			m_CurrentRunSpeed = m_RunSpeed;
			if ( m_Speed.x >= m_RunSpeed )
			{
				m_CurrentRunSpeed = Mathf.SmoothDamp ( m_Speed.x, m_MaxRunSpeed, ref m_CurrentSmoothVelocity, m_RunSmoothTime );
			}

			// Input Processing
			Move ( CrossPlatformInputManager.GetAxis ( "Horizontal" ) );
			if ( CrossPlatformInputManager.GetButtonDown ( "Jump" ) )
			{
				Jump ();
			}
			if ( IsDead.Value && !m_ClosingEye )
			{
				StartCoroutine ( CloseEye () );
			}
			if ( CrossPlatformInputManager.GetButtonDown ( "Guard" ) )
			{
				m_Guard = !m_Guard;
			}
			if ( m_Guard )
			{
				if ( CrossPlatformInputManager.GetButtonDown ( "Fire" ) )
				{
					m_Animator.SetTrigger ( m_Actions [ m_CurrentActionIndex ] );
					if ( m_CurrentActionIndex < m_Actions.Length - 1 )
					{
						m_CurrentActionIndex++;
					}
					else
					{
						m_CurrentActionIndex = 0;
					}
				}
			}

			if ( Input.GetButtonDown ( "Roll" ) )
			{
				Vector2 force = new Vector2 ( 0f, 0f );
				if ( transform.localScale.z > 0f )
				{
					force.x = m_RollForce;
				}
				else if ( transform.localScale.z < 0f )
				{
					force.x = -m_RollForce;
				}
				m_Rigidbody2D.AddForce ( force );
			}
		}

		void LateUpdate ()
		{
			m_Animator.SetFloat ( "Speed", m_Speed.x );
			m_Animator.SetFloat ( "VelocityX", Mathf.Abs ( m_Rigidbody2D.velocity.x ) );
			m_Animator.SetFloat ( "VelocityY", m_Rigidbody2D.velocity.y );
			m_Animator.SetBool ( "IsGrounded", m_GroundCheck.IsGrounded );
			m_Animator.SetBool ( "IsDead", IsDead.Value );
			m_Animator.SetBool ( "Block", m_Block );
			m_Animator.SetBool ( "Guard", m_Guard );
			if ( Input.GetButtonDown ( "Roll" ) )
			{
				m_Animator.SetTrigger ( "Roll" );
			}
		}

		//		void OnCollisionEnter2D ( Collision2D collision2D )
		//		{
		//			bool isGround = collision2D.collider.CompareTag ( GroundCheck.GROUND_TAG );
		//			if ( isGround && !m_IsDead )
		//			{
		//				bool isBottom = false;
		//				for ( int i = 0; i < collision2D.contacts.Length; i++ )
		//				{
		//					if ( !isBottom )
		//					{
		//						isBottom = collision2D.contacts [ i ].normal.y == 1;
		//					}
		//					else
		//					{
		//						break;
		//					}
		//				}
		//				if ( isBottom )
		//				{
		//					m_JumpParticleSystem.Play ();
		//				}
		//			}
		//		}

		#endregion

		#region Private Methods

		IEnumerator CloseEye ()
		{
			m_ClosingEye = true;
			yield return new WaitForSeconds ( 0.6f );
			while ( m_Skeleton.RightEye.localScale.y > 0f )
			{
				if ( m_Skeleton.RightEye.localScale.y > 0f )
				{
					Vector3 scale = m_Skeleton.RightEye.localScale;
					scale.y -= 0.1f;
					m_Skeleton.RightEye.localScale = scale;
				}
				if ( m_Skeleton.LeftEye.localScale.y > 0f )
				{
					Vector3 scale = m_Skeleton.LeftEye.localScale;
					scale.y -= 0.1f;
					m_Skeleton.LeftEye.localScale = scale;
				}
				yield return new WaitForSeconds ( 0.05f );
			}
		}

		#endregion

		#region Public Methods

		public virtual void PlayFootstepSound ()
		{
			if ( m_GroundCheck.IsGrounded )
			{
				AudioManager.Singleton.PlayFootstepSound ( m_FootstepAudioSource, ref m_CurrentFootstepSoundIndex );
			}
		}

		public override void Move ( float horizontalAxis )
		{
			if ( !IsDead.Value )
			{
				float speed = m_CurrentRunSpeed;
//				if ( CrossPlatformInputManager.GetButton ( "Walk" ) )
//				{
//					speed = m_WalkSpeed;
				//				}
				Vector2 velocity = m_Rigidbody2D.velocity;
				velocity.x = speed * horizontalAxis;
				m_Rigidbody2D.velocity = velocity;
				if ( horizontalAxis > 0f )
				{
					Vector3 scale = transform.localScale;
					scale.x = Mathf.Sign ( horizontalAxis );
					transform.localScale = scale;
				}
				else if ( horizontalAxis < 0f )
				{
					Vector3 scale = transform.localScale;
					scale.x = Mathf.Sign ( horizontalAxis );
					transform.localScale = scale;
				}
			}
		}

		public override void Jump ()
		{
			if ( !IsDead.Value )
			{
				if ( m_GroundCheck.IsGrounded )
				{
					Vector2 velocity = m_Rigidbody2D.velocity;
					velocity.y = m_JumpStrength;
					m_Rigidbody2D.velocity = velocity;
					m_Animator.ResetTrigger ( "Jump" );
					m_Animator.SetTrigger ( "Jump" );
					m_JumpParticleSystem.Play ();
					AudioManager.Singleton.PlayJumpSound ( m_JumpAndGroundedAudioSource );
				}
			}
		}

		public override void Die ()
		{
			Die ( false );
		}

		public override void Die ( bool blood )
		{
			if ( !IsDead.Value )
			{
                IsDead.Value = true;
				m_Skeleton.SetActive ( true, m_Rigidbody2D.velocity );
				if ( blood )
				{
					ParticleSystem particle = Instantiate<ParticleSystem> (
						                          m_BloodParticleSystem,
						                          transform.position,
						                          Quaternion.identity );
					Destroy ( particle.gameObject, particle.main.duration );
				}
				CameraController.Singleton.fastMove = true;
			}
		}

		public override void EmitRunParticle ()
		{
			if ( !IsDead.Value )
			{
				m_RunParticleSystem.Emit ( 1 );
			}
		}

		public override void Reset ()
		{
            IsDead.Value = false;
			m_ClosingEye = false;
			m_Guard = false;
			m_Block = false;
			m_CurrentFootstepSoundIndex = 0;
			transform.localScale = m_InitialScale;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Skeleton.SetActive ( false, m_Rigidbody2D.velocity );
		}

		#endregion

		#region Events

		void GameManager_OnReset ()
		{
			transform.position = m_InitialPosition;
			Reset ();
		}

		void Skeleton_OnActiveChanged ( bool active )
		{
			m_Animator.enabled = !active;
			m_Collider2D.enabled = !active;
			m_Rigidbody2D.simulated = !active;
		}

		void GroundCheck_OnGrounded ()
		{
			if ( !IsDead.Value )
			{
				m_JumpParticleSystem.Play ();
				AudioManager.Singleton.PlayGroundedSound ( m_JumpAndGroundedAudioSource );
			}
		}

		#endregion

		[System.Serializable]
		public class CharacterDeadEvent : UnityEvent
		{

		}

	}

}
