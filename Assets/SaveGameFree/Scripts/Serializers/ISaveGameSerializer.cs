using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BayatGames.SaveGameFree.Serializers
{

	/// <summary>
	/// Interface for Save Game Serializers.
	/// </summary>
	public interface ISaveGameSerializer
	{

		/// <summary>
		/// Serialize the specified object to stream with encoding.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Serialize<T> ( T obj, Stream stream, Encoding encoding );

		/// <summary>
		/// Deserialize the specified object from stream using the encoding.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		T Deserialize<T> ( Stream stream, Encoding encoding );

	}

}