using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

static class SelectParent
{
	[MenuItem ( "Edit/Select parent &s", false, 1100 )]
	static void Execute ()
	{
		List<GameObject> selection = new List<GameObject> ();
		foreach ( GameObject s in Selection.gameObjects )
		{
			if ( s.transform.parent != null )
				if ( !selection.Contains ( s.transform.parent.gameObject ) )
					selection.Add ( s.transform.parent.gameObject );
		}

		Selection.objects = selection.ToArray ();
	}
}