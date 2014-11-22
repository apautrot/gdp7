using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Group : MonoBehaviour
{
#if UNITY_EDITOR

	public static bool AreGroupsSelectable = true;

	[HideInInspector]
	public bool IsOpened;
	private Bounds bounds;
	private Vector3 boundsDecals;
	private bool initialized;
	public bool IsSelectable = true;

	void OnDrawGizmos ()
	{
		if ( !initialized )
		{
			initialized = true;
			if ( !IsOpened )
				Close ();
		}

		if ( ( bounds.size == Vector3.zero ) && ( transform.childCount != 0 ) )
			UpdateBounds ();

		if ( !AreGroupsSelectable || !IsSelectable || ( IsOpened && ( Selection.activeGameObject == gameObject ) && IsSelectable ) ) // when opened, wireframe gizmo allowing to select children
		{
			//Debug.Log ( "Draw opened gizmo" );
			Gizmos.color = new Color ( 1, 1, 1, 0.25f );
			Gizmos.DrawWireCube ( transform.position + boundsDecals, bounds.size );
		}
		else // when closed, invisible gizmo allow to select group, but avoid to select children
		{
			//Debug.Log ( "Draw closed gizmo" );
			Gizmos.color = new Color ( 1, 1, 1, 0 );
			Gizmos.DrawCube ( transform.position + boundsDecals, bounds.size );
		}
	}

	public void UpdateBounds()
	{
		bounds = gameObject.GetChildBounds ();
		boundsDecals = bounds.center - transform.position;
	}
	
	public void Close ()
	{
		IsOpened = false;
		foreach ( Transform t in transform )
		{
			t.gameObject.hideFlags |= HideFlags.HideInHierarchy;
		}
		
		EditorUtility.SetDirty ( this );
		EditorApplication.RepaintHierarchyWindow ();
		UpdateBounds ();
	}

	public void Open ()
	{
		IsOpened = true;
		foreach ( Transform t in transform )
		{
			t.gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
		}

		EditorUtility.SetDirty ( this );
		EditorApplication.RepaintHierarchyWindow ();
		UpdateBounds ();
	}

#endif
}
