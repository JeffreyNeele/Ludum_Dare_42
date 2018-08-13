using UnityEngine;
using UnityEditor;

namespace UnityRockStudio
{//THIS IS A WIP BECAUSE IDK IF I WANT THAT PEOPLE NEED TO ALSO SAVE A .ASSET AND
    //THAT IF IT'S DELETED, THINGS GO WRONG
	public class PrefabCreator : MonoBehaviour
	{
		public static void CreatePrefab(GameObject gameobject,string path)
		{
			path = EditorUtility.SaveFilePanel("Save mesh as prefab", path, gameobject.name, "prefab");
			if(string.IsNullOrEmpty(path)) return;
			path = FileUtil.GetProjectRelativePath(path);

			//if(RockStudio.createFolder)
			//{
				Mesh mesh = gameobject.GetComponent<MeshFilter>().sharedMesh;
            

            if (mesh)
				{
					//AssetDatabase.CreateFolder(path.Replace("/"+gameobject.name + ".prefab",""), gameobject.name);
					//AssetDatabase.CreateAsset(mesh, path.Replace(".prefab","")+"/"+gameobject.name+".asset");
					Object prefab = PrefabUtility.CreateEmptyPrefab(path.Replace(".prefab","")+"/"+gameobject.name+".prefab");
					PrefabUtility.ReplacePrefab(gameobject, prefab, ReplacePrefabOptions.ConnectToPrefab);
					//AssetDatabase.Refresh();
					Debug.Log("Asset saved at " + path.Replace("/"+gameobject.name + ".prefab",""));
				}
				else Debug.LogWarning("No mesh was found on the given object.");
			//}

			/*else
			{
				Mesh mesh = gameobject.GetComponent<MeshFilter>().sharedMesh;
                UVMapper.CreateUVMap(mesh, gameobject.transform);
                if (mesh)
				{
					AssetDatabase.CreateAsset(mesh, path.Replace(".prefab",".asset"));
					var prefab = PrefabUtility.CreateEmptyPrefab(path);
					PrefabUtility.ReplacePrefab(gameobject, prefab);
					AssetDatabase.Refresh();
					Debug.Log("Asset saved at " + path);
				}

				else Debug.LogWarning("No mesh was found on the given object.");
			}*/
		}
	}
}
