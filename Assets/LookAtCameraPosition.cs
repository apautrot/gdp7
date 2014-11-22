using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LookAtCameraPosition : MonoBehaviour
{
	public bool lookAtBaseOfCamera;

#if UNITY_EDITOR
	void Update ()
	{
		FixedUpdate ();
	}
#endif

	void FixedUpdate ()
	{
		Vector3 targetPosition = transform.position - Camera.main.transform.position;
		if ( lookAtBaseOfCamera )
		{
			targetPosition.y = 0;
		}

		transform.rotation = Quaternion.LookRotation ( targetPosition, Vector3.up );
	}
}
