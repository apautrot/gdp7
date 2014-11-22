using UnityEngine;
using UnityEditor;
using System.Collections;

/*
[CustomEditor ( typeof ( Transform ) ), CanEditMultipleObjects]
public class RotationTool : Editor
{
	void OnSceneGUI ()
	{
		Transform transform = (Transform)target;
		Vector3 pos = transform.position;
		Handles.Label ( pos, "   " + 5 );
	}

	public override void OnInspectorGUI ()
	{
		Transform t = (Transform)target;

		Undo.RecordObject ( t, "Transform Change" );

		EditorGUI.indentLevel = 0;
		Vector3 position = EditorGUILayout.Vector3Field ( "Position", t.localPosition );
		if ( GUI.changed )
		{
			foreach ( Object o in targets )
			{
				Transform transform = (Transform)o;
				transform.localPosition = FixIfNaN ( position );
			}
		}

		Vector3 eulerAngles = EditorGUILayout.Vector3Field ( "Rotation", t.localEulerAngles );
		if ( GUI.changed )
		{
			foreach ( Object o in targets )
			{
				Transform transform = (Transform)o;
				transform.localEulerAngles = FixIfNaN ( eulerAngles );
			}
		}

		Vector3 scale = EditorGUILayout.Vector3Field ( "Scale", t.localScale );
		if ( GUI.changed )
		{
			foreach ( Object o in targets )
			{
				Transform transform = (Transform)o;
				transform.localScale = FixIfNaN ( scale );
			}
		}

// 		if ( GUI.changed )
// 		{
// 			t.localPosition = FixIfNaN ( position );
// 			t.localEulerAngles = FixIfNaN ( eulerAngles );
// 			t.localScale = FixIfNaN ( scale );
// 		}
	}

	private Vector3 FixIfNaN ( Vector3 v )
	{
		if ( float.IsNaN ( v.x ) )
		{
			v.x = 0;
		}
		if ( float.IsNaN ( v.y ) )
		{
			v.y = 0;
		}
		if ( float.IsNaN ( v.z ) )
		{
			v.z = 0;
		}
		return v;
	}
}
*/