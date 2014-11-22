using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformMethods
{
	public static Transform[] FindAll ( this Transform self, string name )
	{
		List<Transform> list = new List<Transform>();
		int count = self.childCount;
		for ( int i = 0; i < count; i++ )
		{
			Transform t = self.GetChild ( i );
			if ( t.name == name )
				list.Add ( t );
		}

		return list.ToArray ();
	}

	public static void SetScale ( this Transform self, float scale )
	{
		self.localScale = new Vector3 ( scale, scale, scale );
	}

	public static void ScaleBy ( this Transform self, float scaleBy )
	{
		self.localScale *= scaleBy;
	}
}
