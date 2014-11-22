using UnityEngine;
using UnityEditor;
using System.Collections;

public class AlignObjectsEditor : EditorWindow
{
	bool resourcesLoaded;
	Texture2D turnLeft;
	Texture2D turnRight;
	Texture2D alignBottom;
	Texture2D alignTop;
	Texture2D alignLeft;
	Texture2D alignRight;
	Texture2D alignCenterH;
	Texture2D alignCenterV;
// 	Texture2D destributeAlignLeft;
// 	Texture2D destributeAlignRight;
// 	Texture2D destributeAlignTop;
// 	Texture2D destributeAlignBottom;
// 	Texture2D destributeAlignEvenH;
// 	Texture2D destributeAlignEvenV;

	Texture[] axisOnImages;
	Texture[] axisImages;

	Texture[] buttonImages;

	enum Alignment
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	private Alignment rotationAxis = Alignment.Center;

	[MenuItem ( "Window/Align Objects" )]
	static void Init ()
	{
		EditorWindow window = EditorWindow.GetWindow ( typeof ( AlignObjectsEditor ) );
		window.title = "Align Objects";
		// window.minSize = new Vector2 ( 250, 150 );
		window.Show ();
		( window as AlignObjectsEditor ).LoadResources();
	}

	void LoadResources()
	{
		turnLeft = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Turn left.png" );
		turnRight = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Turn right.png" );

		alignBottom = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-bottom.png" );
		alignTop = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-top.png" );
		alignLeft = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-left.png" );
		alignRight = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-right.png" );
		alignCenterH = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-center-h.png" );
		alignCenterV = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Align-center-v.png" );
// 		destributeAlignLeft = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-align-left.png" );
// 		destributeAlignRight = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-align-right.png" );
// 		destributeAlignTop = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-align-top.png" );
// 		destributeAlignBottom = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-align-bottom.png" );
// 		destributeAlignEvenH = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-even-h.png" );
// 		destributeAlignEvenV = Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Distribute-even-v.png" );

		axisOnImages = new Texture[]
		{
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-top-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-top.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-top-right.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-center.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-right.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-bottom-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-bottom.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-on-bottom-right.png" )
		};

		axisImages = new Texture[]
		{
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-top-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-top.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-top-right.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-center.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-right.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-bottom-left.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-bottom.png" ),
			Resources.LoadAssetAtPath<Texture2D> ( "Assets/Framework/Editor/Window - Align objects/Images/Axis-bottom-right.png" )
		};

		UpdateAxisButtonsImages ();

		resourcesLoaded = true;
	}

	void UpdateAxisButtonsImages ()
	{
		buttonImages = new Texture[]
		{
			rotationAxis == Alignment.TopLeft ? axisOnImages[0] : axisImages[0],
			rotationAxis == Alignment.Top ? axisOnImages[1] : axisImages[1],
			rotationAxis == Alignment.TopRight ? axisOnImages[2] : axisImages[2],
			rotationAxis == Alignment.Left ? axisOnImages[3] : axisImages[3],
			rotationAxis == Alignment.Center ? axisOnImages[4] : axisImages[4],
			rotationAxis == Alignment.Right ? axisOnImages[5] : axisImages[5],
			rotationAxis == Alignment.BottomLeft ? axisOnImages[6] : axisImages[6],
			rotationAxis == Alignment.Bottom ? axisOnImages[7] : axisImages[7],
			rotationAxis == Alignment.BottomRight ? axisOnImages[8] : axisImages[8]
		};
	}

	void OnSelectionChange ()
	{
		Repaint ();
	}

	void OnGUI ()
	{
		if ( !resourcesLoaded )
			LoadResources ();

		GUIStyle labelStyle = new GUIStyle ( GUI.skin.label );
		labelStyle.normal.textColor = new Color ( 0.40f, 0.40f, 0.40f );

		GUIStyle flatButtonStyle = new GUIStyle ( GUI.skin.label );
		flatButtonStyle.padding = new RectOffset ( 0,0,0,0 );

		EditorGUILayout.BeginHorizontal ();

		GUI.enabled = Selection.gameObjects.Length > 0;
		if ( GUILayout.Button ( turnLeft, GUILayout.Width ( 30 ), GUILayout.Height ( 30 ) ) ) TurnLeft ();
		if ( GUILayout.Button ( turnRight, GUILayout.Width ( 30 ), GUILayout.Height ( 30 ) ) ) TurnRight ();
		GUI.enabled = true;

		GUILayout.FlexibleSpace ();

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ( "Around", labelStyle, GUILayout.Width ( 55 ) );
		int newAxis = GUILayout.SelectionGrid ( (int)rotationAxis, buttonImages, 3, flatButtonStyle, GUILayout.Width ( 60 ), GUILayout.Height ( 60 ) );
		if ( newAxis != (int)rotationAxis )
		{
			rotationAxis = (Alignment)newAxis;
			UpdateAxisButtonsImages ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ( "Apply to", labelStyle, GUILayout.Width ( 55 ) );

		EditorGUILayout.BeginVertical ();
		GUILayout.Toggle ( true, " group" );
		GUILayout.Toggle ( false, " object" );
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Separator ();

		EditorGUILayout.BeginHorizontal ();
		GUI.enabled = Selection.gameObjects.Length > 1;
		if ( GUILayout.Button ( alignLeft, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignLeft ();
		if ( GUILayout.Button ( alignCenterH, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignCenterH ();
		if ( GUILayout.Button ( alignRight, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignRight ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUI.enabled = Selection.gameObjects.Length > 1;
		if ( GUILayout.Button ( alignTop, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignTop ();
		if ( GUILayout.Button ( alignCenterV, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignCenterV ();
		if ( GUILayout.Button ( alignBottom, GUILayout.Width ( 36 ), GUILayout.Height ( 36 ) ) ) AlignBottom ();
		EditorGUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		GUI.enabled = true;
	}

	private Vector3 RotateAroundPoint ( Vector3 point, Vector3 pivot, Quaternion angle )
	{
		return angle * ( point - pivot ) + pivot;
	}

	private void TurnLeft ()
	{
		foreach ( GameObject go in Selection.gameObjects )
		{
 			go.transform.Rotate ( 0, 0, 90 );
			Vector3 pivot = GetRelativePosition ( go, rotationAxis );
			Debug.Log ( "" + pivot );
			go.transform.position = RotateAroundPoint ( go.transform.position, pivot, Quaternion.Euler ( 0, 0, 90 ) );
		}
	}

	private void TurnRight ()
	{
		foreach ( GameObject go in Selection.gameObjects )
			go.transform.Rotate ( new Vector3 ( 0, 0, 1 ), -90, Space.World );
	}

	private Vector3 GetRelativePosition ( GameObject go, Alignment alignment )
	{
		Bounds bounds = go.GetBounds ();
		if ( bounds.size != Vector3.zero )
			switch ( alignment )
			{
				case Alignment.TopLeft:		return new Vector3 ( bounds.min.x, bounds.max.y );
				case Alignment.Top:			return new Vector3 ( bounds.center.x, bounds.max.y ); 
				case Alignment.TopRight:	return new Vector3 ( bounds.max.x, bounds.max.y ); 

				case Alignment.Left:		return new Vector3 ( bounds.min.x, bounds.center.y ); 
				case Alignment.Center:		return new Vector3 ( bounds.center.x, bounds.center.y ); 
				case Alignment.Right:		return new Vector3 ( bounds.max.x, bounds.center.y ); 

				case Alignment.BottomLeft:	return new Vector3 ( bounds.min.x, bounds.min.y ); 
				case Alignment.Bottom:		return new Vector3 ( bounds.center.x, bounds.min.y );
				case Alignment.BottomRight:	return new Vector3 ( bounds.max.x, bounds.min.y ); 
			}

		return Vector3.zero;
	}

	private void AlignLeft ()
	{
		float min = float.MaxValue;
		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds();
			min = System.Math.Min ( min, bounds.min.x );
		}

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.min.x - min;
			if ( translation != 0 )
				go.transform.Translate ( -translation, 0, 0 );
		}
	}

	private void AlignCenterH ()
	{
		float avg = Selection.gameObjects[0].transform.position.x;
		for ( int i = 1 ; i < Selection.gameObjects.Length; i++ )
		{
			GameObject go = Selection.gameObjects[i];
			Bounds bounds = go.GetBounds ();
			avg += bounds.center.x;
		}
		avg /= Selection.gameObjects.Length;

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.center.x - avg;
			if ( translation != 0 )
				go.transform.Translate ( -translation, 0, 0 );
		}
	}

	private void AlignRight ()
	{
		float max = float.MinValue;
		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			max = System.Math.Max ( max, bounds.max.x );
		}

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.max.x - max;
			if ( translation != 0 )
				go.transform.Translate ( -translation, 0, 0 );
		}
	}

	private void AlignTop ()
	{
		float max = float.MinValue;
		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			max = System.Math.Max ( max, bounds.max.y );
		}

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.max.y - max;
			if ( translation != 0 )
				go.transform.Translate ( 0, -translation, 0 );
		}
	}

	private void AlignCenterV ()
	{
		float avg = Selection.gameObjects[0].transform.position.y;
		for ( int i = 1; i < Selection.gameObjects.Length; i++ )
		{
			GameObject go = Selection.gameObjects[i];
			Bounds bounds = go.GetBounds ();
			avg += bounds.center.y;
		}
		avg /= Selection.gameObjects.Length;

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.center.y - avg;
			if ( translation != 0 )
				go.transform.Translate ( 0, -translation, 0 );
		}
	}

	private void AlignBottom ()
	{
		float min = float.MaxValue;
		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			min = System.Math.Min ( min, bounds.min.y );
		}

		foreach ( GameObject go in Selection.gameObjects )
		{
			Bounds bounds = go.GetBounds ();
			float translation = bounds.min.y - min;
			if ( translation != 0 )
				go.transform.Translate ( 0, -translation, 0 );
		}
	}

}
