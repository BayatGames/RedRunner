using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BayatGames.SaveGameFree.Serializers;

namespace BayatGames.SaveGameFree.Examples
{

	public class SerializerDropdown : Dropdown
	{

		private static SerializerDropdown m_Singleton;

		public static SerializerDropdown Singleton
		{
			get
			{
				return m_Singleton;
			}
		}

		private static ISaveGameSerializer[] m_Serializers = new ISaveGameSerializer[] {
			new SaveGameXmlSerializer (),
			new SaveGameJsonSerializer (),
			new SaveGameBinarySerializer ()
		};

		protected ISaveGameSerializer m_ActiveSerializer;

		public ISaveGameSerializer ActiveSerializer
		{
			get
			{
				if ( m_ActiveSerializer == null )
				{
					m_ActiveSerializer = new SaveGameJsonSerializer ();
				}
				return m_ActiveSerializer;
			}
		}

		protected override void Awake ()
		{
			if ( m_Singleton != null )
			{
				Destroy ( gameObject );
				return;
			}
			m_Singleton = this;
			base.Awake ();
			options = new List<OptionData> () {
				new OptionData ( "XML" ),
				new OptionData ( "JSON" ),
				new OptionData ( "Binary" )
			};
			onValueChanged.AddListener ( OnValueChanged );
			value = SaveGame.Load<int> ( "serializer", 0, new SaveGameJsonSerializer () );
		}

		protected virtual void OnValueChanged ( int index )
		{
			m_ActiveSerializer = m_Serializers [ index ];
		}

		protected virtual void OnApplicationQuit ()
		{
			SaveGame.Save<int> ( "serializer", value, new SaveGameJsonSerializer () );
		}

	}

}