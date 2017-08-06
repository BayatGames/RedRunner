using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Representation of 3D vectors and points.
	/// </summary>
	[Serializable]
	public struct Vector3Save
	{

		public float x;
		public float y;
		public float z;

		public Vector3Save ( float x )
		{
			this.x = x;
			this.y = 0f;
			this.z = 0f;
		}

		public Vector3Save ( float x, float y )
		{
			this.x = x;
			this.y = y;
			this.z = 0f;
		}

		public Vector3Save ( float x, float y, float z )
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3Save ( Vector2 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = 0f;
		}

		public Vector3Save ( Vector3 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		public Vector3Save ( Vector4 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		public Vector3Save ( Vector2Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = 0f;
		}

		public Vector3Save ( Vector3Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		public Vector3Save ( Vector4Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		public static implicit operator Vector3Save ( Vector2 vector )
		{
			return new Vector3Save ( vector );
		}

		public static implicit operator Vector2 ( Vector3Save vector )
		{
			return new Vector2 ( vector.x, vector.y );
		}

		public static implicit operator Vector3Save ( Vector3 vector )
		{
			return new Vector3Save ( vector );
		}

		public static implicit operator Vector3 ( Vector3Save vector )
		{
			return new Vector3 ( vector.x, vector.y, vector.z );
		}

		public static implicit operator Vector3Save ( Vector4 vector )
		{
			return new Vector3Save ( vector );
		}

		public static implicit operator Vector4 ( Vector3Save vector )
		{
			return new Vector4 ( vector.x, vector.y, vector.z );
		}

		public static implicit operator Vector3Save ( Vector2Save vector )
		{
			return new Vector3Save ( vector );
		}

		public static implicit operator Vector2Save ( Vector3Save vector )
		{
			return new Vector2Save ( vector );
		}

		public static implicit operator Vector3Save ( Vector4Save vector )
		{
			return new Vector3Save ( vector );
		}

		public static implicit operator Vector4Save ( Vector3Save vector )
		{
			return new Vector4Save ( vector );
		}

	}

}