using UnityEditor;//qqq
using UnityEngine;

[CustomEditor ( typeof ( Quad ) ), CanEditMultipleObjects]
class QuadInspector : Editor
{
	SerializedProperty TopLeftColor;
	SerializedProperty TopRightColor;
	SerializedProperty BottomLeftColor;
	SerializedProperty BottomRightColor;
	SerializedProperty Width;
	SerializedProperty Height;
	SerializedProperty Alignment;
	
	void OnEnable ()
	{
		TopLeftColor = serializedObject.FindProperty ( "TopLeftColor" );
		TopRightColor = serializedObject.FindProperty ( "TopRightColor" );
		BottomLeftColor = serializedObject.FindProperty ( "BottomLeftColor" );
		BottomRightColor = serializedObject.FindProperty ( "BottomRightColor" );
		Width = serializedObject.FindProperty ( "Width" );
		Height = serializedObject.FindProperty ( "Height" );
		Alignment = serializedObject.FindProperty ( "Alignment" );
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.BeginVertical ();

				EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ( "Size", GUILayout.Width ( 60 ) );
					// GUILayout.FlexibleSpace ();
					EditorGUILayout.PropertyField ( Width, GUIContent.none, GUILayout.Width ( 40 ) );
					EditorGUILayout.LabelField ( "x", GUILayout.Width ( 12 ) );
					EditorGUILayout.PropertyField ( Height, GUIContent.none, GUILayout.Width ( 40 ) );
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ( "Alignment", GUILayout.Width ( 60 ) );
					EditorGUILayout.PropertyField ( Alignment, GUIContent.none, GUILayout.Width ( 60 ) );
				EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();

			GUILayout.FlexibleSpace ();

			EditorGUILayout.BeginVertical ();

				EditorGUILayout.BeginHorizontal ();
					// GUILayout.FlexibleSpace ();
					EditorGUILayout.LabelField ( "Color", GUILayout.Width ( 60 ) );
					Color previousColor = TopLeftColor.colorValue;
					EditorGUILayout.PropertyField ( TopLeftColor, GUIContent.none, GUILayout.Width ( 60 ) );
					if ( previousColor != TopLeftColor.colorValue )
					{
						TopRightColor.colorValue = TopLeftColor.colorValue;
						BottomLeftColor.colorValue = TopLeftColor.colorValue;
						BottomRightColor.colorValue = TopLeftColor.colorValue;
					}
					GUILayout.FlexibleSpace ();
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
					// GUILayout.FlexibleSpace ();
					EditorGUILayout.PropertyField ( TopLeftColor, GUIContent.none, GUILayout.Width ( 60 ) );
					EditorGUILayout.PropertyField ( TopRightColor, GUIContent.none, GUILayout.Width ( 60 ) );
					// GUILayout.FlexibleSpace ();
				EditorGUILayout.EndHorizontal ();


				EditorGUILayout.BeginHorizontal ();
					// GUILayout.FlexibleSpace ();
					EditorGUILayout.PropertyField ( BottomLeftColor, GUIContent.none, GUILayout.Width ( 60 ) );
					EditorGUILayout.PropertyField ( BottomRightColor, GUIContent.none, GUILayout.Width ( 60 ) );
					// GUILayout.FlexibleSpace ();
				EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();

		if ( GUI.changed )
		{
			foreach ( Quad q in targets )
				q.Recreate ();
		}
	}
}