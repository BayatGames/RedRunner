using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Utilities
{

	public class PathPoint : MonoBehaviour
	{

		public enum MoveType
		{
			MoveTowards,
			Lerp,
			SmoothDamp,
			Acceleration
		}

		public MoveType moveType = MoveType.MoveTowards;
		public float delay = 0f;
		public float speed = 1f;
		public float smoothTime = 0.3f;
		public float acceleration = 1f;
		public float maxSpeed = 60f;

	}

}