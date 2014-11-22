using UnityEngine;
using System.Collections;

[RequireComponent ( typeof ( Camera ) )]
public class CameraFrustumGizmo : MonoBehaviour
{
	public int ScreenWidth = 960;
	public int ScreenHeight = 640;

	public virtual void OnDrawGizmos ()
	{
		Matrix4x4 temp = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS ( transform.position, transform.rotation, Vector3.one );
		if ( camera.orthographic )
		{
			float spread = camera.farClipPlane - camera.nearClipPlane;
			float center = ( camera.farClipPlane + camera.nearClipPlane ) * 0.5f;
			Gizmos.DrawWireCube ( new Vector3 ( 0, 0, center ), new Vector3 ( camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread ) );
		}
		else
		{
			// Gizmos.DrawFrustum ( Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect );
			float aspectRatio = (float)ScreenWidth / (float)ScreenHeight;
			Gizmos.matrix = Matrix4x4.TRS ( camera.transform.position, camera.transform.rotation, new Vector3 ( 1.0f, aspectRatio, 1.0f ) );
			Gizmos.DrawFrustum ( Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, aspectRatio );
		}
		Gizmos.matrix = temp;
	}
}
