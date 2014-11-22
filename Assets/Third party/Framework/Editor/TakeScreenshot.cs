using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class TakeScreenshot : EditorWindow
{
	[MenuItem ( "Window/Take Screen Shot", false, 1100 )]
	static void Execute ()
	{
		Camera.main.Render ();
		ScreenshotEncode ();
	}

	static void ScreenshotEncode ()
	{
//		Application.CaptureScreenshot ( "screenshot.png" );

		string path = EditorUtility.SaveFilePanel ( "Save to...", Application.dataPath, "screenshot", "png" );
		if ( path.Length > 0 )
		{
			Texture2D texture = new Texture2D ( Screen.width, Screen.height, TextureFormat.ARGB32, false );
			texture.ReadPixels ( new Rect ( 0, 0, Screen.width, Screen.height ), 0, 0 );
			texture.Apply ();

			byte[] bytes = texture.EncodeToPNG ();

			File.WriteAllBytes ( path, bytes );

			Debug.Log ( "Screen shot taken (" + texture.width + "x" + texture.height + ")." );
			DestroyImmediate ( texture );
		}
	}
}
