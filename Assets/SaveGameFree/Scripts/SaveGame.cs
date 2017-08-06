using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree.Encoders;

namespace BayatGames.SaveGameFree
{

	/// <summary>
	/// Save game path. base paths for your save games.
	/// </summary>
	public enum SaveGamePath
	{
		
		/// <summary>
		/// The persistent data path. Application.persistentDataPath
		/// </summary>
		PersistentDataPath,

		/// <summary>
		/// The data path. Application.dataPath
		/// </summary>
		DataPath

	}

	/// <summary>
	/// Save Game.
	/// Use these APIs to Save & Load game data.
	/// If you are looking for Web saving and loading use SaveGameWeb.
	/// </summary>
	public static class SaveGame
	{

		/// <summary>
		/// Save handler.
		/// </summary>
		public delegate void SaveHandler ( object obj,string identifier,bool encode,string password,ISaveGameSerializer serializer,ISaveGameEncoder encoder,Encoding encoding,SaveGamePath path );

		/// <summary>
		/// Load handler.
		/// </summary>
		public delegate void LoadHandler ( object loadedObj,string identifier,bool encode,string password,ISaveGameSerializer serializer,ISaveGameEncoder encoder,Encoding encoding,SaveGamePath path );

		/// <summary>
		/// Occurs when on saved.
		/// </summary>
		public static event SaveHandler OnSaved;

		/// <summary>
		/// Occurs when on loaded.
		/// </summary>
		public static event LoadHandler OnLoaded;

		/// <summary>
		/// The save callback.
		/// </summary>
		public static SaveHandler SaveCallback;

		/// <summary>
		/// The load callback.
		/// </summary>
		public static LoadHandler LoadCallback;

		private static ISaveGameSerializer m_Serializer = new SaveGameJsonSerializer ();
		private static ISaveGameEncoder m_Encoder = new SaveGameSimpleEncoder ();
		private static Encoding m_Encoding = Encoding.UTF8;
		private static bool m_Encode = false;
		private static SaveGamePath m_SavePath = SaveGamePath.PersistentDataPath;
		private static string m_EncodePassword = "h@e#ll$o%^";
		private static bool m_LogError = false;

		/// <summary>
		/// Gets or sets the serializer.
		/// </summary>
		/// <value>The serializer.</value>
		public static ISaveGameSerializer Serializer
		{
			get
			{
				if ( m_Serializer == null )
				{
					m_Serializer = new SaveGameJsonSerializer ();
				}
				return m_Serializer;
			}
			set
			{
				m_Serializer = value;
			}
		}

		/// <summary>
		/// Gets or sets the encoder.
		/// </summary>
		/// <value>The encoder.</value>
		public static ISaveGameEncoder Encoder
		{
			get
			{
				if ( m_Encoder == null )
				{
					m_Encoder = new SaveGameSimpleEncoder ();
				}
				return m_Encoder;
			}
			set
			{
				m_Encoder = value;
			}
		}

		/// <summary>
		/// Gets or sets the encoding.
		/// </summary>
		/// <value>The encoding.</value>
		public static Encoding DefaultEncoding
		{
			get
			{
				if ( m_Encoding == null )
				{
					m_Encoding = Encoding.UTF8;
				}
				return m_Encoding;
			}
			set
			{
				m_Encoding = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SaveGameFree.SaveGame"/> encrypt data by default.
		/// </summary>
		/// <value><c>true</c> if encode; otherwise, <c>false</c>.</value>
		public static bool Encode
		{
			get
			{
				return m_Encode;
			}
			set
			{
				m_Encode = value;
			}
		}

		/// <summary>
		/// Gets or sets the save path.
		/// </summary>
		/// <value>The save path.</value>
		public static SaveGamePath SavePath
		{
			get
			{
				return m_SavePath;
			}
			set
			{
				m_SavePath = value;
			}
		}

		/// <summary>
		/// Gets or sets the encryption password.
		/// </summary>
		/// <value>The encryption password.</value>
		public static string EncodePassword
		{
			get
			{
				return m_EncodePassword;
			}
			set
			{
				m_EncodePassword = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SaveGameFree.SaveGame"/> log error.
		/// </summary>
		/// <value><c>true</c> if log error; otherwise, <c>false</c>.</value>
		public static bool LogError
		{
			get
			{
				return m_LogError;
			}
			set
			{
				m_LogError = value;
			}
		}

		/// <summary>
		/// Saves data using the identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object to save.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj )
		{
			Save<T> ( identifier, obj, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj, encode and encodePassword.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, bool encode )
		{
			Save<T> ( identifier, obj, encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj and encodePassword.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, string encodePassword )
		{
			Save <T> ( identifier, obj, Encode, encodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj and serializer.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="serializer">Serializer.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, ISaveGameSerializer serializer )
		{
			Save<T> ( identifier, obj, Encode, EncodePassword, serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj and encoder.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="encoder">Encoder.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, ISaveGameEncoder encoder )
		{
			Save<T> ( identifier, obj, Encode, EncodePassword, Serializer, encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj and encoding.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, Encoding encoding )
		{
			Save<T> ( identifier, obj, Encode, EncodePassword, Serializer, Encoder, encoding, SavePath );
		}

		/// <summary>
		/// Save the specified identifier, obj and savePath.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <param name="savePath">Save path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier, T obj, SaveGamePath savePath )
		{
			Save<T> ( identifier, obj, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, savePath );
		}

		/// <summary>
		/// Saves data using the identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object to save.</param>
		/// <param name="encode">Encrypt the data?</param>
		/// <param name="password">Encryption Password.</param>
		/// <param name="serializer">Serializer.</param>
		/// <param name="encoder">Encoder.</param>
		/// <param name="encoding">Encoding.</param>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Save<T> ( string identifier,
		                             T obj,
		                             bool encode,
		                             string password,
		                             ISaveGameSerializer serializer,
		                             ISaveGameEncoder encoder,
		                             Encoding encoding,
		                             SaveGamePath path )
		{
			if ( serializer == null )
			{
				serializer = SaveGame.Serializer;
			}
			if ( encoding == null )
			{
				encoding = SaveGame.DefaultEncoding;
			}
			string filePath = "";
			if ( !IsFilePath ( identifier ) )
			{
				switch ( path )
				{
					default:
					case SaveGamePath.PersistentDataPath:
						filePath = string.Format ( "{0}/{1}", Application.persistentDataPath, identifier );
						break;
					case SaveGamePath.DataPath:
						filePath = string.Format ( "{0}/{1}", Application.dataPath, identifier );
						break;
				}
			}
			else
			{
				filePath = identifier;
			}
			if ( obj == null )
			{
				obj = default(T);
			}
			Stream stream = null;
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			#if UNITY_WSA || UNITY_WINRT
			UnityEngine.Windows.Directory.CreateDirectory ( filePath );
			#else
			Directory.CreateDirectory ( Path.GetDirectoryName ( filePath ) );
			#endif
			#endif
			if ( encode )
			{
				stream = new MemoryStream ();
			}
			else
			{
				#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
				if ( IOSupported () )
				{
					#if UNITY_WSA || UNITY_WINRT
					stream = new MemoryStream ();
					#else
					stream = File.Create ( filePath );
					#endif
				}
				else
				{
					stream = new MemoryStream ();
				}
				#else
				stream = new MemoryStream ();
				#endif
			}
			serializer.Serialize ( obj, stream, encoding );
			if ( encode )
			{
				string data = encoding.GetString ( ( ( MemoryStream )stream ).ToArray () );
				string encoded = encoder.Encode ( data, password );
				#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
				if ( IOSupported () )
				{
					#if UNITY_WSA || UNITY_WINRT
					UnityEngine.Windows.File.WriteAllBytes ( filePath, encoding.GetBytes ( encoded ) );
					#else
					File.WriteAllText ( filePath, encoded, encoding );
					#endif
				}
				else
				{
					PlayerPrefs.SetString ( filePath, encoded );
					PlayerPrefs.Save ();
				}
				#else
				PlayerPrefs.SetString ( filePath, encoded );
				PlayerPrefs.Save ();
				#endif
			}
			else if ( !IOSupported () )
			{
				string data = encoding.GetString ( ( ( MemoryStream )stream ).ToArray () );
				PlayerPrefs.SetString ( filePath, data );
				PlayerPrefs.Save ();
			}
			stream.Dispose ();
			if ( SaveCallback != null )
			{
				SaveCallback.Invoke ( 
					obj,
					identifier,
					encode,
					password,
					serializer,
					encoder,
					encoding,
					path );
			}
			if ( OnSaved != null )
			{
				OnSaved ( 
					obj,
					identifier,
					encode,
					password,
					serializer,
					encoder,
					encoding,
					path );
			}
		}

		/// <summary>
		/// Loads data using identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier )
		{
			return Load<T> ( identifier, default(T), Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and defaultValue.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue )
		{
			return Load<T> ( identifier, defaultValue, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and encode.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, bool encode )
		{
			return Load<T> ( identifier, default(T), encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and encodePassword.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, string encodePassword )
		{
			return Load<T> ( identifier, default(T), Encode, encodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and serializer.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="serializer">Serializer.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, ISaveGameSerializer serializer )
		{
			return Load<T> ( identifier, default(T), Encode, EncodePassword, serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and encoder.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="encoder">Encoder.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, ISaveGameEncoder encoder )
		{
			return Load<T> ( identifier, default(T), Encode, EncodePassword, Serializer, encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and encoding.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, Encoding encoding )
		{
			return Load<T> ( identifier, default(T), Encode, EncodePassword, Serializer, Encoder, encoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier and savePath.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="savePath">Save path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, SaveGamePath savePath )
		{
			return Load<T> ( identifier, default(T), Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, savePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and encode.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, bool encode )
		{
			return Load<T> ( identifier, defaultValue, encode, EncodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and encodePassword.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, string encodePassword )
		{
			return Load<T> ( identifier, defaultValue, Encode, encodePassword, Serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and serializer.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="serializer">Serializer.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, ISaveGameSerializer serializer )
		{
			return Load<T> ( identifier, defaultValue, Encode, EncodePassword, serializer, Encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and encoder.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="encoder">Encoder.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, ISaveGameEncoder encoder )
		{
			return Load<T> ( identifier, defaultValue, Encode, EncodePassword, Serializer, encoder, DefaultEncoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and encoding.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="encoding">Encoding.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, Encoding encoding )
		{
			return Load<T> ( identifier, defaultValue, Encode, EncodePassword, Serializer, Encoder, encoding, SavePath );
		}

		/// <summary>
		/// Load the specified identifier, defaultValue and savePath.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="savePath">Save path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier, T defaultValue, SaveGamePath savePath )
		{
			return Load<T> ( identifier, defaultValue, Encode, EncodePassword, Serializer, Encoder, DefaultEncoding, savePath );
		}

		/// <summary>
		/// Loads data using identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default Value.</param>
		/// <param name="encode">Load encrypted data? (set it to true if you have used encryption in save)</param>
		/// <param name="password">Encryption Password.</param>
		/// <param name="serializer">Serializer.</param>
		/// <param name="encoder">Encoder.</param>
		/// <param name="encoding">Encoding.</param>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Load<T> ( string identifier,
		                          T defaultValue,
		                          bool encode,
		                          string password,
		                          ISaveGameSerializer serializer,
		                          ISaveGameEncoder encoder,
		                          Encoding encoding,
		                          SaveGamePath path )
		{
			if ( serializer == null )
			{
				serializer = SaveGame.Serializer;
			}
			if ( encoding == null )
			{
				encoding = SaveGame.DefaultEncoding;
			}
			T result = defaultValue;
			string filePath = "";
			if ( !IsFilePath ( identifier ) )
			{
				switch ( path )
				{
					default:
					case SaveGamePath.PersistentDataPath:
						filePath = string.Format ( "{0}/{1}", Application.persistentDataPath, identifier );
						break;
					case SaveGamePath.DataPath:
						filePath = string.Format ( "{0}/{1}", Application.dataPath, identifier );
						break;
				}
			}
			else
			{
				filePath = identifier;
			}
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			if ( !Exists ( filePath, path ) )
			#else
			if ( !Exists ( identifier, path ) )
			#endif
			{
				Debug.LogWarningFormat (
					"The specified identifier ({1}) does not exists. please use Exists () to check for existent before calling Load.\n" +
					"returning the default(T) instance.",
					filePath,
					identifier );
				return result;
			}
			Stream stream = null;
			if ( encode )
			{
				string data = "";
				#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
				if ( IOSupported () )
				{
					#if UNITY_WSA || UNITY_WINRT
					data = encoding.GetString ( UnityEngine.Windows.File.ReadAllBytes ( filePath ) );
					#else
					data = File.ReadAllText ( filePath, encoding );
					#endif
				}
				else
				{
					data = PlayerPrefs.GetString ( filePath );
				}
				#else
				data = PlayerPrefs.GetString ( filePath );
				#endif
				string decoded = encoder.Decode ( data, password );
				stream = new MemoryStream ( encoding.GetBytes ( decoded ), true );
			}
			else
			{
				#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
				if ( IOSupported () )
				{
					#if UNITY_WSA || UNITY_WINRT
					stream = new MemoryStream ( UnityEngine.Windows.File.ReadAllBytes ( filePath ) );
					#else
					stream = File.OpenRead ( filePath );
					#endif
				}
				else
				{
					string data = PlayerPrefs.GetString ( filePath );
					stream = new MemoryStream ( encoding.GetBytes ( data ) );
				}
				#else
				string data = PlayerPrefs.GetString ( filePath );
				stream = new MemoryStream ( encoding.GetBytes ( data ) );
				#endif
			}
			result = serializer.Deserialize <T> ( stream, encoding );
			stream.Dispose ();
			if ( result == null )
			{
				result = defaultValue;
			}
			if ( LoadCallback != null )
			{
				LoadCallback.Invoke ( 
					result,
					identifier,
					encode,
					password,
					serializer,
					encoder,
					encoding,
					path );
			}
			if ( OnLoaded != null )
			{
				OnLoaded ( 
					result,
					identifier,
					encode,
					password,
					serializer,
					encoder,
					encoding,
					path );
			}
			return result;
		}

		/// <summary>
		/// Checks whether the specified identifier exists or not.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public static bool Exists ( string identifier )
		{
			return Exists ( identifier, SavePath );
		}

		/// <summary>
		/// Checks whether the specified identifier exists or not.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="path">Path.</param>
		/// <param name="web">Check in Web?</param>
		/// <param name="webUsername">Web username.</param>
		/// <param name="webPassword">Web password.</param>
		/// <param name="webURL">Web URL.</param>
		public static bool Exists ( string identifier, SaveGamePath path )
		{
			string filePath = "";
			if ( !IsFilePath ( identifier ) )
			{
				switch ( path )
				{
					default:
					case SaveGamePath.PersistentDataPath:
						filePath = string.Format ( "{0}/{1}", Application.persistentDataPath, identifier );
						break;
					case SaveGamePath.DataPath:
						filePath = string.Format ( "{0}/{1}", Application.dataPath, identifier );
						break;
				}
			}
			else
			{
				filePath = identifier;
			}
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			if ( IOSupported () )
			{
				#if UNITY_WSA || UNITY_WINRT
				return UnityEngine.Windows.File.Exists ( filePath );
				#else
				return File.Exists ( filePath );
				#endif
			}
			else
			{
				return PlayerPrefs.HasKey ( filePath );
			}
			#else
			return PlayerPrefs.HasKey ( filePath );
			#endif
		}

		public static void Delete ( string identifier )
		{
			Delete ( identifier, SavePath );
		}

		public static void Delete ( string identifier, SaveGamePath path )
		{
			string filePath = "";
			if ( !IsFilePath ( identifier ) )
			{
				switch ( path )
				{
					default:
					case SaveGamePath.PersistentDataPath:
						filePath = string.Format ( "{0}/{1}", Application.persistentDataPath, identifier );
						break;
					case SaveGamePath.DataPath:
						filePath = string.Format ( "{0}/{1}", Application.dataPath, identifier );
						break;
				}
			}
			else
			{
				filePath = identifier;
			}
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			if ( IOSupported () )
			{
				#if UNITY_WSA || UNITY_WINRT
				UnityEngine.Windows.File.Delete ( filePath );
				#else
				File.Delete ( filePath );
				#endif
			}
			else
			{
				PlayerPrefs.DeleteKey ( filePath );
			}
			#else
			PlayerPrefs.DeleteKey ( filePath );
			#endif
		}

		public static void DeleteAll ()
		{
			DeleteAll ( SavePath );
		}

		public static void DeleteAll ( SaveGamePath path )
		{
			string dirPath = "";
			switch ( path )
			{
				case SaveGamePath.PersistentDataPath:
					dirPath = Application.persistentDataPath;
					break;
				case SaveGamePath.DataPath:
					dirPath = Application.dataPath;
					break;
			}
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			if ( IOSupported () )
			{
				#if UNITY_WSA || UNITY_WINRT
				UnityEngine.Windows.Directory.Delete ( dirPath );
				#else
				DirectoryInfo info = new DirectoryInfo ( dirPath );
				FileInfo [] files = info.GetFiles ();
				for ( int i = 0; i < files.Length; i++ )
				{
					files [ i ].Delete ();
				}
				DirectoryInfo [] dirs = info.GetDirectories ();
				for ( int i = 0; i < dirs.Length; i++ )
				{
					dirs [ i ].Delete ( true );
				}
				#endif
			}
			else
			{
				PlayerPrefs.DeleteAll ();
			}
			#else
			PlayerPrefs.DeleteAll ();
			#endif
		}

		/// <summary>
		/// Checks if the IO is supported on current platform or not.
		/// </summary>
		/// <returns><c>true</c>, if supported was IOed, <c>false</c> otherwise.</returns>
		public static bool IOSupported ()
		{
			return Application.platform != RuntimePlatform.WebGLPlayer &&
			Application.platform != RuntimePlatform.WSAPlayerARM &&
			Application.platform != RuntimePlatform.WSAPlayerX64 &&
			Application.platform != RuntimePlatform.WSAPlayerX86 &&
			Application.platform != RuntimePlatform.SamsungTVPlayer &&
			Application.platform != RuntimePlatform.tvOS &&
			Application.platform != RuntimePlatform.PS4;
		}

		/// <summary>
		/// Determines if the string is file path.
		/// </summary>
		/// <returns><c>true</c> if is file path the specified str; otherwise, <c>false</c>.</returns>
		/// <param name="str">String.</param>
		public static bool IsFilePath ( string str )
		{
			bool result = false;
			#if !UNITY_SAMSUNGTV && !UNITY_TVOS && !UNITY_WEBGL
			if ( Path.IsPathRooted ( str ) )
			{
				try
				{
					Path.GetFullPath ( str );
					result = true;
				}
				catch ( System.Exception )
				{
					result = false;
				}
			}
			#endif
			return result;
		}

	}

}