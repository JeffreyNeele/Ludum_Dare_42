using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRockStudio
{
	public class MeshCombiner : MonoBehaviour
	{
		public static GameObject Combine(string combinedMeshName, GameObject parentObject)
		{
			Mesh combinedMesh = new Mesh();
			combinedMesh.name = "Combined Mesh (" + parentObject.name + ")";
			GameObject combinedGameObject = new GameObject(combinedMeshName);

			combinedGameObject.AddComponent<MeshFilter>();
			combinedGameObject.GetComponent<MeshFilter>().mesh = combinedMesh;

			Vector3 position = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);

			parentObject.transform.position = new Vector3(0, 0, 0);

			MeshFilter[] filters = parentObject.GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[filters.Length];

			for(int i = 0; i < filters.Length; i++)
			{
				if(filters[i].sharedMesh == null) continue;
				combine[i].mesh = filters[i].sharedMesh;
				combine[i].transform = filters[i].transform.localToWorldMatrix;
			}

			combinedMesh.CombineMeshes(combine, true, true);
			parentObject.transform.position = position;
			return combinedGameObject;
		}
	}
}
