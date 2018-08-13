using UnityEngine;
using UnityEditor;

namespace UnityRockStudio
{
	public class SaveToAssets : MonoBehaviour
	{
		public static string ChangePath(string path)
		{
			string emptyString = "";
			path = EditorUtility.SaveFolderPanel("Choose Folder", "", "");
			if(string.IsNullOrEmpty(path)) return emptyString;
			path = FileUtil.GetProjectRelativePath(path);
			return path;
		}
	}
}
