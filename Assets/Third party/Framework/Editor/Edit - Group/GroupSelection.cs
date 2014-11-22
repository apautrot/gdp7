using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

static class GroupSelection
{
	[MenuItem ( "Edit/Reset HideFlag", false, 1100 )]
	static void ResetHideFlag ()
	{
		GameObject[] all = GameObject.FindObjectsOfType<GameObject>() as GameObject[];
		foreach ( GameObject go in all )
		{
			go.hideFlags = HideFlags.None;
			EditorUtility.SetDirty ( go );
		}

		EditorApplication.RepaintProjectWindow ();
	}

	[MenuItem ( "Edit/Groups/Convert to group", false, 1100 )]
	static void ConvertSelectedObjectsToGroups ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return;

		foreach ( GameObject g in Selection.gameObjects )
		{
			if ( g.GetComponent<Group> () == null )
			{
				Group group = g.AddComponent<Group> ();
				group.UpdateBounds ();
				group.Close ();

//				Bounds bounds = g.GetBounds ( GetBoundsOption.FullHierarchy ); 
// 				CircleCollider2D collider = g.GetOrCreateComponent<CircleCollider2D> ();
// 				collider.radius = bounds.size.magnitude / 2;
			}
		}
	}

	[MenuItem ( "Edit/Groups/Group Selection %g", false, 1100 )]
	static void GroupSelectedObjects ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return;

		Bounds bounds = new Bounds ( Selection.gameObjects[0].transform.position, Vector3.zero );
		foreach ( GameObject s in Selection.gameObjects )
			if ( s.renderer != null )
				bounds.Encapsulate ( s.renderer.bounds );
			else
				bounds.Encapsulate ( s.GetChildBounds () );

		Vector3 center = bounds.center;
		GameObject groupObject = new GameObject ();
		groupObject.transform.position = center;
		groupObject.transform.parent = Selection.gameObjects[0].transform.parent;
		groupObject.name = "Group";

		foreach ( GameObject s in Selection.gameObjects )
			s.transform.parent = groupObject.transform;

		Group group = groupObject.AddComponent<Group> ();
		group.UpdateBounds ();
		group.Close ();

// 		CircleCollider2D collider = groupObject.AddComponent<CircleCollider2D> ();
// 		collider.radius = bounds.size.sqrMagnitude / 2;

		Selection.activeGameObject = groupObject;
	}

	[MenuItem ( "Edit/Groups/Ungroup Selection %#g", false, 1101 )]
	static void UngroupSelectedObjects ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return;

		GameObject[] selection = Selection.gameObjects;

		List<GameObject> ungrouped = new List<GameObject> ();
		foreach ( GameObject s in selection )
		{
			UngroupObject ( s, ungrouped );
			DestroyIfGroupGameObject ( s );
		}

		Selection.objects = ungrouped.ToArray();
	}

	static void UngroupObject ( GameObject groupObject, List<GameObject> ungrouped )
	{
		Group group = groupObject.GetComponent<Group> ();
		if ( group != null )
			group.Open ();

		List<GameObject> childs = groupObject.GetChilds ();
		foreach ( GameObject c in childs )
		{
			ungrouped.Add ( c );
			c.transform.parent = groupObject.transform.parent;
		}
	}

	static void DestroyIfGroupGameObject ( GameObject g )
	{
		if ( g.GetComponent<Group>() == null )
			return;

		GameObject.DestroyImmediate ( g );
	}

	[MenuItem ( "Edit/Groups/Open or Close Selected Group &%g", false, 2000 )]
	static void OpenCloseSelectedGroups ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return;

		foreach ( GameObject s in Selection.gameObjects )
		{
			Group group = s.GetComponent<Group> ();
			if ( group != null )
				if ( group.IsOpened )
					group.Close ();
				else
					group.Open ();
		}
	}

	[MenuItem ( "Edit/Groups/Open or Close Selected Group &%g", true, 2000 )]
	static bool ValidateOpenCloseSelectedGroups ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return false;

		foreach ( GameObject s in Selection.gameObjects )
			if ( s.GetComponent<Group> () != null )
				return true;

		return false;
	}
}


static class LockSelection
{
	internal static List<GameObject> lockedObjects = new List<GameObject> ();

	[MenuItem ( "Edit/Lock or Unlock Selection %&l", false, 2103 )]
	static void LockUnlockSelection ()
	{
		if ( Selection.gameObjects.Length == 0 )
			return;

		// HideFlags hideFlag = HideFlags.HideInHierarchy;
		HideFlags hideFlag = HideFlags.NotEditable;

		foreach ( GameObject s in Selection.gameObjects )
		{
			if ( ( s.hideFlags & hideFlag ) == hideFlag )
			{
				s.hideFlags -= hideFlag;
				lockedObjects.Remove ( s );
			}
			else
			{
				if ( ! lockedObjects.Contains ( s ) )
					lockedObjects.Add ( s );
				s.hideFlags |= hideFlag;
			}

			EditorUtility.SetDirty ( s );
		}

		EditorApplication.RepaintProjectWindow ();
	}
}