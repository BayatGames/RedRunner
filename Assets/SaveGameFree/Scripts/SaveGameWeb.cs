using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

namespace BayatGames.SaveGameFree
{

	/// <summary>
	/// Save Game Web.
	/// Use these APIs to Save & Load from web.
	/// </summary>
	public class SaveGameWeb
	{

		private static string m_DefaultUsername = "savegamefree";
		private static string m_DefaultPassword = "$@ve#game%free";
		private static string m_DefaultURL = "http://www.example.com";
		private static bool m_DefaultEncode = false;
		private static string m_DefaultEncodePassword = "h@e#ll$o%^";
		private static ISaveGameSerializer m_DefaultSerializer = new SaveGameJsonSerializer ();
		private static ISaveGameEncoder m_DefaultEncoder = new SaveGameSimpleEncoder ();
		private static Encoding m_DefaultEncoding = Encoding.UTF8;

		/// <summary>
		/// Gets or sets the default username.
		/// </summary>
		/// <value>The default username.</value>
		public static string DefaultUsername
		{
			get
			{
				return m_DefaultUsername;
			}
			set
			{
				m_DefaultUsername = value;
			}
		}

		/// <summary>
		/// Gets or sets the default password.
		/// </summary>
		/// <value>The default password.</value>
		public static string DefaultPassword
		{
			get
			{
				return m_DefaultPassword;
			}
			set
			{
				m_DefaultPassword = value;
			}
		}

		/// <summary>
		/// Gets or sets the default UR.
		/// </summary>
		/// <value>The default UR.</value>
		public static string DefaultURL
		{
			get
			{
				return m_DefaultURL;
			}
			set
			{
				m_DefaultURL = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> default encode.
		/// </summary>
		/// <value><c>true</c> if default encode; otherwise, <c>false</c>.</value>
		public static bool DefaultEncode
		{
			get
			{
				return m_DefaultEncode;
			}
			set
			{
				m_DefaultEncode = value;
			}
		}

		/// <summary>
		/// Gets or sets the default encode password.
		/// </summary>
		/// <value>The default encode password.</value>
		public static string DefaultEncodePassword
		{
			get
			{
				return m_DefaultEncodePassword;
			}
			set
			{
				m_DefaultEncodePassword = value;
			}
		}

		/// <summary>
		/// Gets or sets the default serializer.
		/// </summary>
		/// <value>The default serializer.</value>
		public static ISaveGameSerializer DefaultSerializer
		{
			get
			{
				if ( m_DefaultSerializer == null )
				{
					m_DefaultSerializer = new SaveGameJsonSerializer ();
				}
				return m_DefaultSerializer;
			}
			set
			{
				m_DefaultSerializer = value;
			}
		}

		/// <summary>
		/// Gets or sets the default encoder.
		/// </summary>
		/// <value>The default encoder.</value>
		public static ISaveGameEncoder DefaultEncoder
		{
			get
			{
				if ( m_DefaultEncoder == null )
				{
					m_DefaultEncoder = new SaveGameSimpleEncoder ();
				}
				return m_DefaultEncoder;
			}
			set
			{
				m_DefaultEncoder = value;
			}
		}

		/// <summary>
		/// Gets or sets the default encoding.
		/// </summary>
		/// <value>The default encoding.</value>
		public static Encoding DefaultEncoding
		{
			get
			{
				if ( m_DefaultEncoding == null )
				{
					m_DefaultEncoding = Encoding.UTF8;
				}
				return m_DefaultEncoding;
			}
			set
			{
				m_DefaultEncoding = value;
			}
		}

		protected string m_Username;
		protected string m_Password;
		protected string m_URL;
		protected bool m_Encode;
		protected string m_EncodePassword;
		protected ISaveGameSerializer m_Serializer;
		protected ISaveGameEncoder m_Encoder;
		protected Encoding m_Encoding;
		protected UnityWebRequest m_Request;
		protected bool m_IsError = false;
		protected string m_Error = "";

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public virtual string Username
		{
			get
			{
				return m_Username;
			}
			set
			{
				m_Username = value;
			}
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public virtual string Password
		{
			get
			{
				return m_Password;
			}
			set
			{
				m_Password = value;
			}
		}

		/// <summary>
		/// Gets or sets the UR.
		/// </summary>
		/// <value>The UR.</value>
		public virtual string URL
		{
			get
			{
				return m_URL;
			}
			set
			{
				m_URL = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> is encode.
		/// </summary>
		/// <value><c>true</c> if encode; otherwise, <c>false</c>.</value>
		public virtual bool Encode
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
		/// Gets or sets the encode password.
		/// </summary>
		/// <value>The encode password.</value>
		public virtual string EncodePassword
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
		/// Gets or sets the serializer.
		/// </summary>
		/// <value>The serializer.</value>
		public virtual ISaveGameSerializer Serializer
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
		public virtual ISaveGameEncoder Encoder
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
		public virtual Encoding Encoding
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
		/// Gets the request.
		/// </summary>
		/// <value>The request.</value>
		public virtual UnityWebRequest Request
		{
			get
			{
				return m_Request;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is error.
		/// </summary>
		/// <value><c>true</c> if this instance is error; otherwise, <c>false</c>.</value>
		public virtual bool IsError
		{
			get
			{
				return m_IsError;
			}
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public virtual string Error
		{
			get
			{
				return m_Error;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		public SaveGameWeb () : this ( DefaultUsername )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		public SaveGameWeb ( string username ) : this ( username, DefaultPassword )
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public SaveGameWeb ( string username, string password ) : this ( username, password, DefaultURL )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		public SaveGameWeb ( string username, string password, string url ) : this ( username, password, url, DefaultEncode )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		public SaveGameWeb ( string username, string password, string url, bool encode ) : this ( username,
			                                                                                         password,
			                                                                                         url,
			                                                                                         encode,
			                                                                                         DefaultEncodePassword )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <param name="encodePassword">Encode password.</param>
		public SaveGameWeb ( string username, string password, string url, bool encode, string encodePassword ) : this ( username,
			                                                                                                                password,
			                                                                                                                url,
			                                                                                                                encode,
			                                                                                                                encodePassword,
			                                                                                                                DefaultSerializer )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <param name="serializer">Serializer.</param>
		public SaveGameWeb ( string username,
		                     string password,
		                     string url,
		                     bool encode,
		                     string encodePassword,
		                     ISaveGameSerializer serializer ) : this ( username,
			                                                                                                                                                password,
			                                                                                                                                                url,
			                                                                                                                                                encode,
			                                                                                                                                                encodePassword,
			                                                                                                                                                serializer,
			                                                                                                                                                DefaultEncoder )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <param name="serializer">Serializer.</param>
		/// <param name="encoder">Encoder.</param>
		public SaveGameWeb ( string username,
		                     string password,
		                     string url,
		                     bool encode,
		                     string encodePassword,
		                     ISaveGameSerializer serializer,
		                     ISaveGameEncoder encoder ) : this ( username,
			                                                                                                                                                                          password,
			                                                                                                                                                                          url,
			                                                                                                                                                                          encode,
			                                                                                                                                                                          encodePassword,
			                                                                                                                                                                          serializer,
			                                                                                                                                                                          encoder,
			                                                                                                                                                                          DefaultEncoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.SaveGameFree.SaveGameWeb"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="url">URL.</param>
		/// <param name="encode">If set to <c>true</c> encode.</param>
		/// <param name="encodePassword">Encode password.</param>
		/// <param name="serializer">Serializer.</param>
		/// <param name="encoder">Encoder.</param>
		/// <param name="encoding">Encoding.</param>
		public SaveGameWeb ( string username,
		                     string password,
		                     string url,
		                     bool encode,
		                     string encodePassword,
		                     ISaveGameSerializer serializer,
		                     ISaveGameEncoder encoder,
		                     Encoding encoding )
		{
			m_Username = username;
			m_Password = password;
			m_URL = url;
			m_Encode = encode;
			m_EncodePassword = encodePassword;
			m_Serializer = serializer;
			m_Encoder = encoder;
			m_Encoding = encoding;
		}

		/// <summary>
		/// Save the specified identifier and obj.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual IEnumerator Save<T> ( string identifier, T obj )
		{
			MemoryStream memoryStream = new MemoryStream ();
			Serializer.Serialize<T> ( obj, memoryStream, Encoding );
			string data = Encoding.GetString ( memoryStream.ToArray () );
			if ( Encode )
			{
				data = Encoder.Encode ( data, EncodePassword );
			}
			yield return Send ( identifier, data, "save" );
			if ( m_IsError )
			{
				Debug.LogError ( m_Error );
			}
			else
			{
				Debug.Log ( "Data successfully saved." );
			}
		}

		/// <summary>
		/// Download the specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public virtual IEnumerator Download ( string identifier )
		{
			yield return Send ( identifier, null, "load" );
			if ( m_IsError )
			{
				Debug.LogError ( m_Error );
			}
			else
			{
				Debug.Log ( "Data successfully downloaded." );
			}
		}

		/// <summary>
		/// Load the specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T Load<T> ( string identifier )
		{
			return Load<T> ( identifier, default(T) );
		}

		/// <summary>
		/// Load the specified identifier and defaultValue.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T Load<T> ( string identifier, T defaultValue )
		{
			if ( defaultValue == null )
			{
				defaultValue = default(T);
			}
			T result = defaultValue;
			if ( !m_IsError && !string.IsNullOrEmpty ( m_Request.downloadHandler.text ) )
			{
				string data = m_Request.downloadHandler.text;
				if ( Encode )
				{
					data = Encoder.Decode ( data, EncodePassword );
				}
				MemoryStream memoryStream = new MemoryStream ( Encoding.GetBytes ( data ) );
				result = Serializer.Deserialize<T> ( memoryStream, Encoding );
				memoryStream.Dispose ();
				if ( result == null )
				{
					result = defaultValue;
				}
			}
			return result;
		}

		/// <summary>
		/// Send the specified identifier, data and action.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		/// <param name="data">Data.</param>
		/// <param name="action">Action.</param>
		public virtual IEnumerator Send ( string identifier, string data, string action )
		{
			Dictionary<string, string> formFields = new Dictionary<string, string> () { {
					"identifier",
					identifier
				}, {
					"action",
					action
				}, {
					"username",
					Username
				}
			};
			if ( !string.IsNullOrEmpty ( data ) )
			{
				formFields.Add ( "data", data );
			}
			if ( !string.IsNullOrEmpty ( Password ) )
			{
				formFields.Add ( "password", Password );
			}
			m_Request = UnityWebRequest.Post ( URL, formFields );
			yield return m_Request.Send ();
			if ( m_Request.isNetworkError || m_Request.isHttpError )
			{
				m_IsError = true;
				m_Error = m_Request.error;
			}
			else if ( m_Request.downloadHandler.text.StartsWith ( "Error" ) )
			{
				m_IsError = true;
				m_Error = m_Request.downloadHandler.text;
			}
			else
			{
				m_IsError = false;
			}
		}

	}

}