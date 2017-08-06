using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// A class that allows creating and modifying meshes from scripts.
	/// </summary>
	[Serializable]
	public class MeshSave
	{

		public Vector3Save[] vertices;
		public int[] triangles;
		public Vector2Save[] uv;
		public Vector3Save[] normals;
		public Color[] colors;
		public Color32[] colors32;

		public MeshSave ( Mesh mesh )
		{
			this.vertices = mesh.vertices.Cast<Vector3Save> ().ToArray ();
			this.triangles = mesh.triangles;
			this.uv = mesh.uv.Cast<Vector2Save> ().ToArray ();
			this.normals = mesh.normals.Cast<Vector3Save> ().ToArray ();
			this.colors = mesh.colors.Cast<Color> ().ToArray ();
			this.colors32 = mesh.colors32.Cast<Color32> ().ToArray ();
		}

		public static implicit operator MeshSave ( Mesh mesh )
		{
			return new MeshSave ( mesh );
		}

		public static implicit operator Mesh ( MeshSave mesh )
		{
			Mesh newMesh = new Mesh ();
			newMesh.vertices = mesh.vertices.Cast<Vector3> ().ToArray ();
			newMesh.triangles = mesh.triangles;
			newMesh.uv = mesh.uv.Cast<Vector2> ().ToArray ();
			newMesh.normals = mesh.normals.Cast<Vector3> ().ToArray ();
			newMesh.colors = mesh.colors.Cast<Color> ().ToArray ();
			newMesh.colors32 = mesh.colors32.Cast<Color32> ().ToArray ();
			return newMesh;
		}

	}

}