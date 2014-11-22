using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

static class ReplaceSelection
{
	static private GameObject source;

	[MenuItem ( "Edit/Replace selection/Copy selection as source", true, 1101 )]
	static bool SelectObject_Validate ()
	{
		return Selection.gameObjects.Length != 0;
	}

	[MenuItem ( "Edit/Replace selection/Copy selection as source", false, 1101 )]
	static void SelectObject_Command ()
	{
		source = null;

		if ( Selection.gameObjects.Length != 0 )
		{
			if ( Selection.gameObjects.Length == 1 )
				source = Selection.gameObjects[0];
			else
				Debug.LogWarning ( "Select only one object as a source for Replace selection" );
		}
	}

	[MenuItem ( "Edit/Replace selection/Replace selection", true, 1100 )]
	static bool ReplaceSelection_Validate ()
	{
		return ( source != null );
	}
	
	[MenuItem ( "Edit/Replace selection/Replace selection", false, 1100 )]
	static void ReplaceSelection_Command ()
	{
		List<GameObject> newSelection = new List<GameObject> ();

		foreach ( GameObject s in Selection.gameObjects )
		{
			if ( s != source )
			{
				GameObject go = GameObject.Instantiate ( source ) as GameObject;
				go.transform.parent = s.transform.parent;
				go.transform.position = s.transform.position;
				go.name = s.name;

				newSelection.Add ( go );

				GameObject.DestroyImmediate ( s );
			}
		}

		Selection.objects = newSelection.ToArray ();
	}
}