using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Quaternions are used represent rotations.
	/// </summary>
	[Serializable]
	public struct QuaternionSave
	{

		public float x;
		public float y;
		public float z;
		public float w;

		public QuaternionSave ( float x )
		{
			this.x = x;
			this.y = 0f;
			this.z = 0f;
			this.w = 0f;
		}

		public QuaternionSave ( float x, float y )
		{
			this.x = x;
			this.y = y;
			this.z = 0f;
			this.w = 0f;
		}

		public QuaternionSave ( float x, float y, float z )
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = 0f;
		}

		public QuaternionSave ( float x, float y, float z, float w )
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public QuaternionSave ( Quaternion quaternion )
		{
			this.x = quaternion.x;
			this.y = quaternion.y;
			this.z = quaternion.z;
			this.w = quaternion.w;
		}

		public static implicit operator QuaternionSave ( Quaternion quaternion )
		{
			return new QuaternionSave ( quaternion );
		}

		public static implicit operator Quaternion ( QuaternionSave quaternion )
		{
			return new Quaternion ( quaternion.x, quaternion.y, quaternion.z, quaternion.w );
		}

	}

}