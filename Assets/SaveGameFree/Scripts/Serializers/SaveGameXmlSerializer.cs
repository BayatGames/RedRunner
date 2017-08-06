using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BayatGames.SaveGameFree.Serializers
{

	/// <summary>
	/// Save Game Xml Serializer.
	/// </summary>
	public class SaveGameXmlSerializer : ISaveGameSerializer
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
			try
			{
				XmlSerializer serializer = new XmlSerializer ( typeof ( T ) );
				serializer.Serialize ( stream, obj );
			}
			catch ( Exception ex )
			{
				Debug.LogException ( ex );
			}
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
			try
			{
				XmlSerializer serializer = new XmlSerializer ( typeof ( T ) );
				result = ( T )serializer.Deserialize ( stream );
			}
			catch ( Exception ex )
			{
				Debug.LogException ( ex );
			}
			return result;
		}

	}

}