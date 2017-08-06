using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Utilities;

namespace RedRunner.Characters
{

	[RequireComponent ( typeof ( Rigidbody2D ) )]
	[RequireComponent ( typeof ( Collider2D ) )]
	[RequireComponent ( typeof ( Animator ) )]
	[RequireComponent ( typeof ( Skeleton ) )]
	public abstract class Character : MonoBehaviour
	{

		public delegate void DeadHandler ();

		public virtual event DeadHandler OnDead;

		public abstract float MaxRunSpeed { get; }

		public abstract float RunSmoothTime { get; }

		public abstract float RunSpeed { get; }

		public abstract float WalkSpeed { get; }

		public abstract float JumpStrength { get; }

		public abstract Vector2 Speed { get; }

		public abstract string[] Actions { get; }

		public abstract string CurrentAction { get; }

		public abstract int CurrentActionIndex { get; }

		public abstract GroundCheck GroundCheck { get; }

		public abstract Rigidbody2D Rigidbody2D { get; }

		public abstract Collider2D Collider2D { get; }

		public abstract Animator Animator { get; }

		public abstract ParticleSystem RunParticleSystem { get; }

		public abstract ParticleSystem JumpParticleSystem { get; }

		public abstract ParticleSystem WaterParticleSystem { get; }

		public abstract ParticleSystem BloodParticleSystem { get; }

		public abstract Skeleton Skeleton { get; }

		public abstract bool IsDead { get; }

		public abstract bool ClosingEye { get; }

		public abstract bool Guard { get; }

		public abstract bool Block { get; }

		public abstract AudioSource Audio { get; }

		public abstract void Move ( float horizontalAxis );

		public abstract void Jump ();

		public abstract void Die ();

		public abstract void Die ( bool blood );

		public abstract void EmitRunParticle ();

		public abstract void Reset ();

	}

}