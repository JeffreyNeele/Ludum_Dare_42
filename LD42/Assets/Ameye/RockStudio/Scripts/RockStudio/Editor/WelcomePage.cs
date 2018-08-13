using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WelcomePage : EditorWindow
{
	[MenuItem("Tools/Rock Studio")]
	public static void ShowWindow ()
	{
        GetWindow(typeof(WelcomePage));
		WelcomePage myWindow = (WelcomePage)GetWindow(typeof(WelcomePage));
		myWindow.titleContent = new GUIContent ("Rock Studio");
        GetWindow(typeof(RockStudio)).Close();
        GetWindow(typeof(Changes)).Close();
	}

	public static void Init()
	{
		WelcomePage myWindow = (WelcomePage)GetWindow(typeof(WelcomePage));
		myWindow.Show();
	}

	void OnGUI()
	{
		WelcomePage myWindow = (WelcomePage)GetWindow(typeof(WelcomePage));
		myWindow.minSize = new Vector2(300,297);
		myWindow.maxSize = myWindow.minSize;

		GUIStyle style = new GUIStyle();
		style.richText = true;

		if(GUILayout.Button(Styles.RockStudio,Styles.helpbox))
		{
            GetWindow(typeof(RockStudio));
			this.Close();
		}

		if(GUILayout.Button(Styles.Forum, Styles.helpbox))
		{
			Application.OpenURL("https://forum.unity3d.com/threads/rockstudio-2-1-a-low-poly-procedural-rock-generator.468676/");
		}

		if(GUILayout.Button(Styles.Documentation, Styles.helpbox))
		{
			Application.OpenURL("https://www.docdroid.net/9cHyYRk/documentation.docx.html");
		}

		if(GUILayout.Button(Styles.Contact, Styles.helpbox))
		{
			Application.OpenURL("mailto:alexanderameye@gmail.com?");
		}

		if(GUILayout.Button(Styles.Twitter, Styles.helpbox))
		{
			Application.OpenURL("https://twitter.com/blacksadunity");
		}

		if(GUILayout.Button(Styles.Review, Styles.helpbox))
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/account/downloads/search=RockStudio");
		}

		if(GUILayout.Button(Styles.Changes, Styles.helpbox))
		{
            GetWindow(typeof(Changes));
			this.Close();
		}
	}

	static class Styles
	{
		internal static GUIContent RockStudio;
		internal static GUIContent Forum;
		internal static GUIContent Documentation;
		internal static GUIContent Contact;
		internal static GUIContent Twitter;
		internal static GUIContent Review;
		internal static GUIContent Changes;
		internal static GUIStyle helpbox;

		static Styles()
		{
			RockStudio = IconContent("rock2", "<size=11><b> Rock Studio</b></size>");
			Forum = IconContent("forum_colored", "<size=11><b> Support Forum</b></size>");
			Documentation = IconContent("documentation_colored", "<size=11><b> Online Documentation</b></size>");
			Contact = IconContent("contact_colored", "<size=11><b> Contact</b></size>");
			Review = IconContent("review_colored", "<size=11><b> Rate and Review</b></size>");
			Changes = IconContent("Changes_colored", "<size=11><b> Version Changes</b></size>");
			Twitter = IconContent("twitter_colored", "<size=11><b> Twitter</b></size>");

			helpbox = new GUIStyle(EditorStyles.helpBox);
			helpbox.alignment = TextAnchor.MiddleLeft;
			helpbox.richText = true;
		}

		static GUIContent IconContent(string icon, string text)
		{
            Texture2D cached = EditorGUIUtility.Load("Assets/Ameye/RockStudio/Icons/" + icon + ".png") as Texture2D;
            return new GUIContent(text, cached);
		}
	}
}
