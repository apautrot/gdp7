using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

class SkyboxRenderer : MonoBehaviour
{
	static int faceSize = 512;
	static string directory = "Assets/Map/Skybox";

	static string[] skyBoxImage = new string[] { "front", "right", "back", "left", "top", "bottom" };
	static string[] skyBoxProps = new string[] { "_FrontTex", "_RightTex", "_BackTex", "_LeftTex", "_UpTex", "_DownTex" };

	static Vector3[] skyDirection = new Vector3[] { new Vector3 ( 0, 0, 0 ), new Vector3 ( 0, -90, 0 ), new Vector3 ( 0, 180, 0 ), new Vector3 ( 0, 90, 0 ), new Vector3 ( -90, 0, 0 ), new Vector3 ( 90, 0, 0 ) };

	void Start ()
	{
		if ( !Directory.Exists ( directory ) )
			Directory.CreateDirectory ( directory );

		string name = Path.GetFileNameWithoutExtension ( EditorApplication.currentScene );
		StartCoroutine ( RenderSkyboxTo6PNG ( name, Vector3.zero ) );
	}

	IEnumerator RenderSkyboxTo6PNG ( string name, Vector3 position )
	{
		Camera camera = Camera.main;

		camera.fieldOfView = 90;
		camera.aspect = 1.0f;

		camera.gameObject.transform.position = position;
		camera.gameObject.transform.rotation = Quaternion.identity;

		//Render skybox        
		for ( int orientation = 0; orientation < skyDirection.Length; orientation++ )
		{
			yield return new WaitForEndOfFrame ();

			string assetPath = Path.Combine ( directory, name + "_" + skyBoxImage[orientation] + ".png" );
			RenderSkyBoxFaceToPNG ( orientation, camera, assetPath );
		}

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

		yield break;
	}

	static void RenderSkyBoxFaceToPNG ( int orientation, Camera cam, string assetPath )
	{
// 		cam.transform.eulerAngles = skyDirection[orientation];
// 		RenderTexture rt = new RenderTexture ( faceSize, faceSize, 24 );
// 		cam.camera.targetTexture = rt;
// 		cam.camera.Render ();
// 		RenderTexture.active = rt;
// 
// 		Texture2D screenShot = new Texture2D ( faceSize, faceSize, TextureFormat.RGB24, false );
// 		screenShot.ReadPixels ( new Rect ( 0, 0, faceSize, faceSize ), 0, 0 );
// 
// 		RenderTexture.active = null;
// 		GameObject.DestroyImmediate ( rt );

		cam.transform.eulerAngles = skyDirection[orientation];
		cam.camera.Render ();

		Texture2D screenShot = new Texture2D ( Screen.width, Screen.height, TextureFormat.RGB24, false );
		screenShot.ReadPixels ( new Rect ( 0, 0, Screen.width, Screen.height ), 0, 0 );
		screenShot.Apply ();

		// screenShot.Resize ( faceSize, faceSize, TextureFormat.ARGB32, false );
		TextureScale.Bilinear ( screenShot, faceSize, faceSize );

		byte[] bytes = screenShot.EncodeToPNG ();
		File.WriteAllBytes ( assetPath, bytes );

		AssetDatabase.ImportAsset ( assetPath, ImportAssetOptions.ForceUpdate );
	}

}