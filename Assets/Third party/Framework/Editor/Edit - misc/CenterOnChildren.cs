using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

static class CenterOnChildren
{
	[MenuItem ( "Edit/Center XForm on children &%c", false, 1100 )]
	static void Execute ()
	{
		foreach ( GameObject s in Selection.gameObjects )
		{
			Vector3 center = s.GetChildBounds ().center;
			Vector3 translation = center - s.transform.position;

			if ( translation != Vector3.zero )
			{
				Undo.RecordObject ( s, "Center XForm" );

				s.transform.position = center;

				foreach ( Transform t in s.transform )
					t.position -= translation;
			}
		}
	}
}