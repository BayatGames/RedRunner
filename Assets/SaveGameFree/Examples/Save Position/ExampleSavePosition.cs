using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.SaveGameFree.Types;

namespace BayatGames.SaveGameFree.Examples
{

	public class ExampleSavePosition : MonoBehaviour
	{

		public Transform target;
		public bool loadOnStart = true;
		public string identifier = "exampleSavePosition.dat";

		void Start ()
		{
			if ( loadOnStart )
			{
				Load ();
			}
		}

		void Update ()
		{
			Vector3 newPosition = target.position;
			newPosition.x += Input.GetAxis ( "Horizontal" );
			newPosition.y += Input.GetAxis ( "Vertical" );
			target.position = newPosition;
		}

		void OnApplicationQuit ()
		{
			Save ();
		}

		public void Save ()
		{
			SaveGame.Save<Vector3Save> ( identifier, target.position, SerializerDropdown.Singleton.ActiveSerializer );
		}

		public void Load ()
		{
			target.position = SaveGame.Load<Vector3Save> (
				identifier,
				Vector3.zero,
				SerializerDropdown.Singleton.ActiveSerializer );
		}

	}

}