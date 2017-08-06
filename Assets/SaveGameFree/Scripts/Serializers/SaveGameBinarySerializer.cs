using System;
using System.IO;

#if !UNITY_WSA || !UNITY_WINRT
using System.Runtime.Serialization.Formatters.Binary;
#endif

using System.Text;
using UnityEngine;

namespace BayatGames.SaveGameFree.Serializers
{

	/// <summary>
	/// Save Game Binary Serializer.
	/// </summary>
	public class SaveGameBinarySerializer : ISaveGameSerializer
	{

		/// <summary>
		/// Serialize the specified object to stream with encoding.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Serialize<T> ( T obj, Stream stream, Encoding encoding )
		{
			#if !UNITY_WSA || !UNITY_WINRT
			try
			{
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Serialize ( stream, obj );
			}
			catch ( Exception ex )
			{
				Debug.LogException ( ex );
			}
			#else
			Debug.LogError ( "SaveGameFree: The Binary Serialization isn't supported in Windows Store and UWP." );
			#endif
		}

		/// <summary>
		/// Deserialize the specified object from stream using the encoding.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( Stream stream, Encoding encoding )
		{
			T result = default(T);
			#if !UNITY_WSA || !UNITY_WINRT
			try
			{
				BinaryFormatter formatter = new BinaryFormatter ();
				result = ( T )formatter.Deserialize ( stream );
			}
			catch ( Exception ex )
			{
				Debug.LogException ( ex );
			}
			#else
			Debug.LogError ( "SaveGameFree: The Binary Serialization isn't supported in Windows Store and UWP." );
			#endif
			return result;
		}

	}

}