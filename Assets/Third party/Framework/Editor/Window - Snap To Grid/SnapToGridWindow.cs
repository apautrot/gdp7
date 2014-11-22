using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class SnapToGridWindow : EditorWindow
{
	enum SnapMode
	{
		Horizontal,
		Vertical,
		HorizontalAndVertical
	}

	float width = 1;
	float height = 1;
	SnapMode snapMode = SnapMode.HorizontalAndVertical;
	bool autoApply;
	bool displayGrid;

	[MenuItem ( "Window/Snap to Grid" )]
	static void Init ()
	{
		EditorWindow window = EditorWindow.GetWindow ( typeof ( SnapToGridWindow ) );
		window.title = "Snap to Grid";
		window.minSize = new Vector2 ( 250, 150 );
		window.Show ();
	}

	void OnGUI ()
	{
		EditorGUILayout.Space ();
 		GUILayout.Label ( "  Snap To Grid v1.0", "" );
		EditorGUILayout.Space ();

		snapMode = (SnapMode)EditorGUILayout.EnumPopup ( snapMode );

		GUI.enabled = snapMode != SnapMode.Vertical;
		float newWidth = EditorGUILayout.FloatField ( "Grid width", width );
		if ( ( newWidth != 0 ) && ( newWidth != width ) )
		{
			width = newWidth;
			autoApply = false;
		}
		GUI.enabled = true;

		GUI.enabled = snapMode != SnapMode.Horizontal;
		float newHeight = EditorGUILayout.FloatField ( "Grid height", height );
		if ( ( newHeight != 0 ) && ( newHeight != height ) )
		{
			height = newHeight;
			autoApply = false;
		}
		GUI.enabled = true;

		EditorGUILayout.Space();
		bool displayGridBefore = displayGrid;
		displayGrid = EditorGUILayout.Toggle ( "Display grid", displayGrid );
		if ( displayGridBefore != displayGrid )
			SceneView.currentDrawingSceneView.Repaint ();

		EditorGUILayout.Space();
		autoApply = EditorGUILayout.Toggle ( "Auto apply", autoApply );
		if ( autoApply )
		{
			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.gray;
			style.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label ( "Auto applied to selection", style );
		}
		else
		{
			if ( GUILayout.Button ( "Apply to selection" ) )
				ApplyToSelection ();
		}
	}
	
// 	void OnSelectionChange ()
// 	{
// 	}

// 	void OnHierarchyChange ()
// 	{
// 	}

	void Update ()
	{
		if ( autoApply )
			ApplyToSelection ();
	}

	void ApplyToSelection ()
	{
		foreach ( Object o in Selection.objects )
		{
			GameObject go = o as GameObject;
			if ( go != null )
				SnapPosition ( go );
		}
	}

	void SnapPosition ( GameObject go )
	{
		Vector3 p = go.transform.position;

		float x = (int)( p.x / width ) * width;
		float y = (int)( p.y / height ) * height;
		float xRem = p.x % width;
		float yRem = p.y % height;
		if ( xRem > width / 2 )
			x += width;
		if ( yRem > height / 2 )
			y += height;

		if ( snapMode == SnapMode.Vertical )
			x = p.x;
		if ( snapMode == SnapMode.Horizontal )
			y = p.y;

		if ( ( p.x != x ) || ( p.y != y ) )
			go.transform.position = new Vector3 ( x, y, p.z );
	}

	void OnFocus ()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnEnable ()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDestroy ()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}

	void OnSceneGUI ( SceneView sceneView )
	{
//		Handles.BeginGUI ();
		if ( displayGrid )
			if ( Event.current.type == EventType.Repaint )
			{
				Camera camera = SceneView.currentDrawingSceneView.camera;

				Vector3 bottomLeft = new Vector3 ( 0, Screen.height, 0 );
				Vector3 topRight = new Vector3 ( Screen.width, 0, 0 );

				Vector2 bottomLeftWP = camera.ScreenToWorldPoint ( bottomLeft );
				Vector2 topRightWP = camera.ScreenToWorldPoint ( topRight );

				float widthInPixel = ( width * camera.pixelRect.width ) / ( topRightWP.x - bottomLeftWP.x );
				if ( widthInPixel > 5 )
				{
					float alpha = System.Math.Min ( 0.10f, ( widthInPixel - 5 ) / 100 );
					Handles.color = new Color ( 1, 1, 1, alpha );

					Vector3 v1 = new Vector3 ( 0, bottomLeftWP.y, 0 );
					Vector3 v2 = new Vector3 ( 0, topRightWP.y, 0 );
					float x1 = (int)( bottomLeftWP.x / width ) * width;
					float x2 = topRightWP.x;

					for ( float x = x1; x < x2; x += width )
					{
						v1.x = x;
						v2.x = x;

						Handles.DrawLine ( v1, v2 );
					}
				}

				float heightInPixel = ( height * camera.pixelRect.height ) / ( bottomLeftWP.y - topRightWP.y );
				if ( heightInPixel > 5 )
				{
					float alpha = System.Math.Min ( 0.10f, ( heightInPixel - 5 ) / 100 );
					Handles.color = new Color ( 1, 1, 1, alpha );

					Vector3 v1 = new Vector3 ( topRightWP.x, 0, 0 );
					Vector3 v2 = new Vector3 ( bottomLeftWP.x, 0, 0 );
					float y1 = (int)( topRightWP.y / height ) * height;
					float y2 = bottomLeftWP.y;

					for ( float y = y1; y < y2; y += height )
					{
						v1.y = y;
						v2.y = y;

						Handles.DrawLine ( v1, v2 );
					}
				}
			}
	}
}