using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Utilities
{

	public static class Extensions
	{

		public struct Unit
		{
			public string symbol;
			public float magnitude;
			public string format;

			public Unit ( string symbol, float magnitude, string format )
			{
				this.symbol = symbol;
				this.magnitude = magnitude;
				this.format = format;
			}
		}

		public static Unit[] lengthUnits = new Unit[] {
			new Unit ( "m", 1f, "0" ),
			new Unit ( "km", 1000f, "00.00" )
		};

		public static float modifier = 0.1f;

		public static string ToLength ( this float value )
		{
			value = value * modifier;
			if ( Mathf.Approximately ( value, 0f ) )
			{
				return "0 " + lengthUnits [ 0 ].symbol;
			}
			Unit unit = lengthUnits [ 0 ];
			for ( int i = 0; i < lengthUnits.Length; i++ )
			{
				if ( value > lengthUnits [ i ].magnitude )
				{
					unit = lengthUnits [ i ];
				}
			}
			return ( value / unit.magnitude ).ToString ( unit.format ) + " " + unit.symbol;
		}

	}

}