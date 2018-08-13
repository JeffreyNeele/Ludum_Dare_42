using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Handle))]
public class HandleEditor : Editor
{
  private void OnScene(SceneView sceneview)
  {
    Handle myObj = (Handle)target;
    Handles.color = Color.blue;

    if(myObj != null) Handles.matrix = myObj.transform.localToWorldMatrix;
    if(myObj != null)Handles.DrawWireCube(Vector3.zero,Vector3.one);
  }

  void OnEnable()
  {
    SceneView.onSceneGUIDelegate -= OnScene;
    SceneView.onSceneGUIDelegate += OnScene;
  }
}
