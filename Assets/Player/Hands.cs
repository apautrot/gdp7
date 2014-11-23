using UnityEngine;
using System.Collections;

public class Hands : MonoBehaviour
{
	void Restart ()
	{
		Start ();
	}

	void Start ()
	{
		// transform.animateBounce ( 0.5f, 0, 0.125f ).loopsInfinitely ();
		transform.animateBounce ( 1.0f, 0, 0.125f ).loopsInfinitely ();
	}
}
