using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Collectables
{

	[RequireComponent (typeof(SpriteRenderer))]
	[RequireComponent (typeof(Collider2D))]
	[RequireComponent (typeof(Animator))]
	public abstract class Collectable : MonoBehaviour
	{

		public const string COLLECT_TRIGGER = "Collect";

		public abstract SpriteRenderer SpriteRenderer { get; }

		public abstract Collider2D Collider2D { get; }

		public abstract Animator Animator { get; }

		public abstract bool UseOnTriggerEnter2D { get; set; }

		public abstract void OnTriggerEnter2D (Collider2D other);

		public abstract void OnCollisionEnter2D (Collision2D collision2D);

		public abstract void Collect ();

	}

}