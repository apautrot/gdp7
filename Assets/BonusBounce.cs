using UnityEngine;
using System.Collections;

public class BonusBounce : MonoBehaviour
{
	public float bounceDuration = 1;
	public Vector2 bounceOffset = new Vector2 ( 0, 0 );

	void Restart ()
	{
		Start ();
	}

	void Start ()
	{
		gameObject.transform.animateBounce ( bounceDuration, bounceOffset.x, bounceOffset.y ).loopsInfinitely ();
	}	
}
