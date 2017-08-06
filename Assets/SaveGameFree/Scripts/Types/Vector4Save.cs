using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Representation of four-dimensional vectors.
	/// </summary>
	[Serializable]
	public struct Vector4Save
	{

		public float x;
		public float y;
		public float z;
		public float w;

		public Vector4Save ( float x )
		{
			this.x = x;
			this.y = 0f;
			this.z = 0f;
			this.w = 0f;
		}

		public Vector4Save ( float x, float y )
		{
			this.x = x;
			this.y = y;
			this.z = 0f;
			this.w = 0f;
		}

		public Vector4Save ( float x, float y, float z )
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = 0f;
		}

		public Vector4Save ( float x, float y, float z, float w )
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Vector4Save ( Vector2 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = 0f;
			this.w = 0f;
		}

		public Vector4Save ( Vector3 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
			this.w = 0f;
		}

		public Vector4Save ( Vector4 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
			this.w = vector.w;
		}

		public Vector4Save ( Vector2Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = 0f;
			this.w = 0f;
		}

		public Vector4Save ( Vector3Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
			this.w = 0f;
		}

		public Vector4Save ( Vector4Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
			this.w = vector.w;
		}

		public static implicit operator Vector4Save ( Vector2 vector )
		{
			return new Vector4Save ( vector );
		}

		public static implicit operator Vector2 ( Vector4Save vector )
		{
			return new Vector2 ( vector.x, vector.y );
		}

		public static implicit operator Vector4Save ( Vector3 vector )
		{
			return new Vector4Save ( vector );
		}

		public static implicit operator Vector3 ( Vector4Save vector )
		{
			return new Vector3 ( vector.x, vector.y, vector.z );
		}

		public static implicit operator Vector4Save ( Vector4 vector )
		{
			return new Vector4Save ( vector );
		}

		public static implicit operator Vector4 ( Vector4Save vector )
		{
			return new Vector4 ( vector.x, vector.y, vector.z );
		}

		public static implicit operator Vector4Save ( Vector2Save vector )
		{
			return new Vector4Save ( vector );
		}

		public static implicit operator Vector2Save ( Vector4Save vector )
		{
			return new Vector2Save ( vector );
		}

		public static implicit operator Vector4Save ( Vector3Save vector )
		{
			return new Vector4Save ( vector );
		}

		public static implicit operator Vector3Save ( Vector4Save vector )
		{
			return new Vector3Save ( vector );
		}

	}

}