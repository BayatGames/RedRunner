using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Representation of RGBA color in 32 bit format.
	/// </summary>
	[Serializable]
	public struct Color32Save
	{

		public byte r;
		public byte g;
		public byte b;
		public byte a;

		public Color32Save ( Color32 color )
		{
			this.r = color.r;
			this.g = color.g;
			this.b = color.b;
			this.a = color.a;
		}

		public static implicit operator Color32Save ( Color32 color )
		{
			return new Color32Save ( color );
		}

		public static implicit operator Color32 ( Color32Save color )
		{
			return new Color32 ( color.r, color.g, color.b, color.a );
		}

	}

}