using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRockStudio
{
	public class MakeLowPoly : MonoBehaviour
	{
		public static void LowPoly(Transform transform, int toolBar, string name, bool GenerateUV)
		{
			if(transform == null)
			{
				Debug.Log ("No appropriate object selected.");
				return;
			}

			MeshFilter meshfilter = transform.GetComponent<MeshFilter>();
			if(meshfilter == null || meshfilter.sharedMesh == null)
			{
				Debug.Log ("No mesh found on the selected object");
				return;
			}

			GameObject gameobject = transform.gameObject;
			gameobject.name = name;

			meshfilter = gameobject.GetComponent<MeshFilter>();

			Mesh mesh = meshfilter.sharedMesh;
			Vector3[] oldVerts = mesh.vertices;
			int[] triangles = mesh.triangles;
			Vector3[] vertices = new Vector3[triangles.Length];

			for(int i = 0; i < triangles.Length; i++)
			{
				vertices[i] = oldVerts[triangles[i]];
				triangles[i] = i;
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

            if(GenerateUV)
                UVMapper.CreateUVMap(mesh, gameobject.transform);
        }
	}
}
