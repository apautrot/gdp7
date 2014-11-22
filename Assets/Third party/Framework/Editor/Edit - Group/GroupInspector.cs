using UnityEditor;
using UnityEngine;

[CustomEditor ( typeof ( Group ) ), CanEditMultipleObjects]
class GroupInspector : Editor
{
	SerializedProperty IsSelectable;

	void OnEnable ()
	{
		IsSelectable = serializedObject.FindProperty ( "IsSelectable" );
	}

	public override void OnInspectorGUI ()
	{
		// DrawDefaultInspector ();
		serializedObject.Update ();

		Group group = target as Group;
		
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if ( GUILayout.Button ( group.IsOpened ? "Close" : "Open", GUILayout.Height ( 22 ), GUILayout.Width ( 120 ) ) )
		{
			bool closing = group.IsOpened;
			foreach ( Group g in targets )
			{
				if ( closing )
					g.Close ();
				else
					g.Open ();
				
				g.UpdateBounds ();
			}
			EditorApplication.RepaintHierarchyWindow ();
		}
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		int count = group.gameObject.transform.childCount;
		if ( count > 1 ) GUILayout.Label ( count.ToString () + " children" );
		else if ( count == 1 ) GUILayout.Label ( "1 child" );
		else GUILayout.Label ( "No child" );
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.FlexibleSpace ();
		Group.AreGroupsSelectable = EditorGUILayout.Toggle ( "All groups selectable", Group.AreGroupsSelectable );
		GUILayout.FlexibleSpace ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();

		GUILayout.FlexibleSpace ();
		GUI.enabled = Group.AreGroupsSelectable;
		GUI.changed = false;
		EditorGUILayout.PropertyField ( IsSelectable, new GUIContent ( "This group selectable" ) );
		if ( GUI.changed )
			foreach ( Group g in targets )
				g.UpdateBounds ();
		// group.IsSelectable = EditorGUILayout.ToggleLeft ( "All groups are selectable", Group.IsSelectable );
		GUILayout.FlexibleSpace ();
		
		EditorGUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();
	}
}

