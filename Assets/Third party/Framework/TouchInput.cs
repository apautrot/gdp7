using UnityEngine;
using System.Collections;

class TouchInput : Singleton<TouchInput>
{
	internal static bool isJustDown;
	internal static bool isJustDragged;
	internal static bool isJustUp;

	internal static bool isDown;
	internal static bool isUp;
	internal static bool isMoved;
	internal static bool isDragging;

	internal static Vector3 previousPosition;
	internal static Vector3 position;
	internal static Vector3 dragVector;

	public Camera Camera = null;

	void Update ()
	{
		Refresh ();
	}

	internal Vector3 GetWorldPositionOnXYPlane ( Camera camera, Vector3 screenPosition, float z = 0 )
	{
		Camera inputCamera = Camera != null ? Camera : Camera.main;
		Ray ray = inputCamera.ScreenPointToRay ( screenPosition );
		Plane xy = new Plane ( Vector3.forward, new Vector3 ( 0, 0, z ) );
		float distance;
		xy.Raycast ( ray, out distance );
		return ray.GetPoint ( distance );
	}

	internal void Refresh()
	{
		previousPosition = position;
		Camera inputCamera = Camera != null ? Camera : Camera.main;
		if ( inputCamera.orthographic )
			position = inputCamera.ScreenToWorldPoint ( Input.mousePosition );
		else
			position = GetWorldPositionOnXYPlane ( camera, Input.mousePosition );

		isDown = Input.GetMouseButton ( 0 );
		isUp = !isDown;
		isMoved = ( previousPosition != position );
		isJustDown = Input.GetMouseButtonDown ( 0 );
		isJustUp = Input.GetMouseButtonUp ( 0 );

		isJustDragged = false;
		if ( isDown )
		{
			isJustDragged = isMoved;
			isDragging |= isMoved;
			if ( !isJustDown )
			{
				dragVector.x += position.x - previousPosition.x;
				dragVector.y += position.y - previousPosition.y;
			}
		}
		else
		{
			isDragging = false;
			dragVector = Vector2.zero;
		}

		// Debug.Log ( "refresh " + position );
	}

	/*
	private void OnApplicationFocus ( bool loseFocus )
	{
		Camera inputCamera = Camera != null ? Camera : Camera.main;
		position = inputCamera.ScreenToWorldPoint ( Input.mousePosition );
		previousPosition = position;
		dragVector = Vector2.zero;
	}
	*/
}