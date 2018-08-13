using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Changes : EditorWindow
{
	Vector2 scrollPos;
	public static void ShowWindow ()
	{
        GetWindow(typeof(Changes));
		Changes myWindow = (Changes)GetWindow(typeof(Changes));
		myWindow.titleContent = new GUIContent ("Version Changes");
	}

	public static void Init()
	{
		Changes myWindow = (Changes)GetWindow(typeof(Changes));
		myWindow.Show();
		myWindow.autoRepaintOnSceneChange = true;
		EditorApplication.modifierKeysChanged += myWindow.Repaint;
	}

    public static bool twoDotFourFold
    {
        get { return EditorPrefs.GetBool("TwoDotFourFold", false); }
        set { EditorPrefs.SetBool("TwoDotFourFold", value); }
    }

    public static bool twoDotThreeFold
    {
        get { return EditorPrefs.GetBool("TwoDotThreeFold", false); }
        set { EditorPrefs.SetBool("TwoDotThreeFold", value); }
    }

    public static bool twoDotTwoFold
	{
		get { return EditorPrefs.GetBool("TwoDotTwoFold", false); }
		set { EditorPrefs.SetBool("TwoDotTwoFold", value); }
	}

	public static bool twoDotOneFold
	{
		get { return EditorPrefs.GetBool("TwoDotOneFold", false); }
		set { EditorPrefs.SetBool("TwoDotOneFold", value); }
	}

	public static bool twoDotZeroFold
	{
		get { return EditorPrefs.GetBool("TwoDotZeroFold", false); }
		set { EditorPrefs.SetBool("TwoDotZeroFold", value); }
	}

	public static bool oneDotOneFold
	{
		get { return EditorPrefs.GetBool("OneDotOneFold", false); }
		set { EditorPrefs.SetBool("OneDotOneFold", value); }
	}

	public static bool oneDotZeroFold
	{
		get { return EditorPrefs.GetBool("OneDotZeroFold", false); }
		set { EditorPrefs.SetBool("OneDotZeroFold", value); }
	}

	static GUIStyle _foldoutStyle;
	static GUIStyle foldoutStyle
	{
		get
		{
			if (_foldoutStyle == null)
			{
				_foldoutStyle = new GUIStyle(EditorStyles.foldout);
				_foldoutStyle.font = EditorStyles.boldFont;
			}
			return _foldoutStyle;
		}
	}

	static GUIStyle _boxStyle;
	public static GUIStyle boxStyle
	{
		get
		{
			if (_boxStyle == null)
			{
				_boxStyle = new GUIStyle(EditorStyles.helpBox);
			}
			return _boxStyle;
		}
	}

	void OnGUI()
	{
		Changes myWindow = (Changes)GetWindow(typeof(Changes));
		myWindow.minSize = new Vector2(350,215);
		myWindow.maxSize = myWindow.minSize;
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos,false,false);

        /*twoDotFourFold = BeginFold("New in V2.4 (2017)", twoDotFourFold);
        if (twoDotFourFold)
        {
            EditorGUILayout.LabelField("• Procedural UV mapping");
        }
        EndFold();*/

        twoDotThreeFold = BeginFold("New in V2.3 (September 2017)", twoDotThreeFold);
        if (twoDotThreeFold)
        {
            EditorGUILayout.LabelField("• Procedural UV mapping");
            EditorGUILayout.LabelField("• Lots of bug fixes and general improvements");
            EditorGUILayout.LabelField("• Added undo/redo functionality");
            EditorGUILayout.LabelField("• Improved UI");
            EditorGUILayout.LabelField("• New scene overlay GUI");
            EditorGUILayout.LabelField("• Reworked combine, export and settings sections");
        }
        EndFold();

        twoDotTwoFold = BeginFold("New in V2.2 (June 2017)",twoDotTwoFold);
		if(twoDotTwoFold)
		{
			EditorGUILayout.LabelField("• New and improved UI");
			EditorGUILayout.LabelField("• Improved saving and exporting tools");
			EditorGUILayout.LabelField("• Export as .asset and .prefab");
			EditorGUILayout.LabelField("• Prefab brush");
			EditorGUILayout.LabelField("• Bug fixes");
			EditorGUILayout.LabelField("• Improved code base");
		}
		EndFold();

		twoDotOneFold = BeginFold("New in V2.1 (June 2017)",twoDotOneFold);
		if(twoDotOneFold)
		{
			EditorGUILayout.LabelField("• Bug fixes");
			EditorGUILayout.LabelField("• Saving improvements");
		}
		EndFold();
		twoDotZeroFold = BeginFold("New in V2.0 (May 2017)",twoDotZeroFold);
		if(twoDotZeroFold)
		{
			EditorGUILayout.LabelField("• New rock generation systems (sculpt and compose)");
			EditorGUILayout.LabelField("• New and improved UI");
			EditorGUILayout.LabelField("• Export as .FBX");
			EditorGUILayout.LabelField("• Mesh combining tool");
		}
		EndFold();

		oneDotOneFold = BeginFold("New in V1.1 (April 2017)",oneDotOneFold);
		if(oneDotOneFold)
		{
			EditorGUILayout.LabelField("• New and improved UI (editor window)");
			EditorGUILayout.LabelField("• Save rock as .asset for improved workflow");
			EditorGUILayout.LabelField("• New rock placement settings");
		}
		EndFold();

		oneDotZeroFold = BeginFold("New in V1.0 (April 2017)",oneDotZeroFold);
		if(oneDotZeroFold)
		{
			EditorGUILayout.LabelField("• Basic rock generation");
			EditorGUILayout.LabelField("• Shape presets");
			EditorGUILayout.LabelField("• Seed system");
			EditorGUILayout.LabelField("• Quality/vertex count settings");
			EditorGUILayout.LabelField("• Material manager");
			EditorGUILayout.LabelField("• Save rocks as game-objects");
			EditorGUILayout.LabelField("• Intuitive UI");
			EditorGUILayout.LabelField("• Documentation file");
		}
		EndFold();
		EditorGUILayout.EndScrollView();
	}

	public static bool BeginFold(string foldName, bool foldState)
	{
		EditorGUILayout.BeginVertical(boxStyle);
		GUILayout.Space(3);
		foldState = EditorGUI.Foldout(EditorGUILayout.GetControlRect(),
		foldState, foldName, true, foldoutStyle);
		if (foldState) GUILayout.Space(3);
		return foldState;
	}

	public static void EndFold()
	{
		GUILayout.Space(3);
		EditorGUILayout.EndVertical();
		GUILayout.Space(0);
	}
}
