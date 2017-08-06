using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.SaveGameFree.Types;

namespace BayatGames.SaveGameFree.Examples
{

	public class ExampleSaveScale : MonoBehaviour
	{

		public Transform target;
		public bool loadOnStart = true;
		public string identifier = "exampleSaveScale.dat";

		void Start ()
		{
			if ( loadOnStart )
			{
				Load ();
			}
		}

		void Update ()
		{
			Vector3 scale = target.localScale;
			scale.x += Input.GetAxis ( "Horizontal" );
			scale.y += Input.GetAxis ( "Vertical" );
			target.localScale = scale;
		}

		void OnApplicationQuit ()
		{
			Save ();
		}

		public void Save ()
		{
			SaveGame.Save<Vector3Save> ( identifier, target.localScale, SerializerDropdown.Singleton.ActiveSerializer );
		}

		public void Load ()
		{
			target.localScale = SaveGame.Load<Vector3Save> (
				identifier,
				new Vector3Save ( 1f, 1f, 1f ),
				SerializerDropdown.Singleton.ActiveSerializer );
		}

	}

}