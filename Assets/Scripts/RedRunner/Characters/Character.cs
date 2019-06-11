//Global Libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Local Libraries
using RedRunner.Utilities;

namespace RedRunner.Characters
{
	//Required Components 
	[RequireComponent ( typeof ( Rigidbody2D ) )]
	[RequireComponent ( typeof ( Collider2D ) )]
	[RequireComponent ( typeof ( Animator ) )]
	[RequireComponent ( typeof ( Skeleton ) )]

	//This Class consist of public getter functions that allow other scripts in RedRunner to access information about otherwise private RedCharacter variables.
	public abstract class Character : MonoBehaviour
	{
		//Function for handling a death event
		public delegate void DeadHandler ();

		//What to do upon death 
		public virtual event DeadHandler OnDead;

		//Returns Character Max Run Speed
		public abstract float MaxRunSpeed { get; }

		//Returns Character settling time 
		public abstract float RunSmoothTime { get; }

		//Returns current speed
		public abstract float RunSpeed { get; }

		//Returns walking speed 
		public abstract float WalkSpeed { get; }

		//Returns the strength constant associated with jumping
		public abstract float JumpStrength { get; }

		//Returns vector speed of Character 
		public abstract Vector2 Speed { get; }

		//Returns current actions in string array 
		public abstract string[] Actions { get; }

		//Retuns single string with current action
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

		public virtual Property<bool> IsDead { get; set; }

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