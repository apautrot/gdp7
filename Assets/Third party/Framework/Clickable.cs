using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Clickable : MonoBehaviour
{
	public float scaleFactor = 1.1f;

	private Vector3 originalScale;
	private List<GoTween> tweeners;

	void OnMouseDown ()
	{
		originalScale = transform.localScale;
		transform.localScale = new Vector3 ( originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor );
		tweeners = Go.tweensWithTarget ( transform, true );
		foreach ( GoTween t in tweeners )
			t.pause ();
	}

	void OnMouseUp ()
	{
		transform.localScale = originalScale;
		foreach ( GoTween t in tweeners )
			t.play ();

		if ( gameObject.IsUnderMouse () )
		{
			OnClicked ();
		}
	}

	public abstract void OnClicked ();
}