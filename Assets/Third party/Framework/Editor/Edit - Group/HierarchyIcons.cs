using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[InitializeOnLoad]
class HierarchyIcons
{
	private static Texture2D objectLocked;
	private static Texture2D groupOpened;
	private static Texture2D groupClosed;
	private static List<int> openedGroups;
	private static List<int> closedGroups;
	private static List<int> lockedObjects;

	static HierarchyIcons ()
	{
		openedGroups = new List<int> ();
		closedGroups = new List<int> ();
		lockedObjects = new List<int> ();

		if ( EditorGUIUtility.isProSkin )
		{
			objectLocked = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/black-theme/object-locked.png", typeof ( Texture2D ) ) as Texture2D;
			groupOpened = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/black-theme/group-opened.png", typeof ( Texture2D ) ) as Texture2D;
			groupClosed = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/black-theme/group-closed.png", typeof ( Texture2D ) ) as Texture2D;
		}
		else
		{
			objectLocked = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/white-theme/object-locked.png", typeof ( Texture2D ) ) as Texture2D;
			groupOpened = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/white-theme/group-opened.png", typeof ( Texture2D ) ) as Texture2D;
			groupClosed = AssetDatabase.LoadAssetAtPath ( "Assets/Framework/Editor/Edit - Group/white-theme/group-closed.png", typeof ( Texture2D ) ) as Texture2D;
		}
		EditorApplication.update += UpdateCB;
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
	}

	static void UpdateCB ()
	{
		Group[] groups = GameObject.FindObjectsOfType ( typeof ( Group ) ) as Group[];
		openedGroups.Clear ();
		closedGroups.Clear ();
		lockedObjects.Clear ();
		int count = groups.Length;
		for ( int i = 0; i < count; i++ )
		{
			Group group = groups[i];
			if ( group != null )
				if ( group.IsOpened )
					openedGroups.Add ( group.gameObject.GetInstanceID () );
				else
					closedGroups.Add ( group.gameObject.GetInstanceID () );
		}
		for ( int i = 0; i < LockSelection.lockedObjects.Count; i++ )
		{
			GameObject go = LockSelection.lockedObjects[i];
			lockedObjects.Add ( go.GetInstanceID () );
		}
	}

	static void HierarchyItemCB ( int instanceID, Rect selectionRect )
	{
		Rect r = new Rect ( selectionRect );

		if ( openedGroups.Contains ( instanceID ) )
		{
			r.x = r.xMax - 25;
			r.width = 20;
			GUI.Label ( r, groupOpened );
		}

		if ( closedGroups.Contains ( instanceID ) )
		{
			r.x = r.xMax - 25;
			r.width = 20;
			GUI.Label ( r, groupClosed );
		}

		if ( lockedObjects.Contains ( instanceID ) )
		{
			r.x = r.xMax - 40;
			r.width = 20;
			GUI.Label ( r, objectLocked );
		}
	}

}