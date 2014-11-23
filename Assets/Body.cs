using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour
{
	void Restart ()
	{
		Start ();
	}

	void Start ()
	{
		// transform.animateBounce ( 1.0f, 0, 0.05f ).delays ( 0.25f ).loopsInfinitely ();
		transform.animateBounce ( 0.5f, 0, 0.15f ).delays ( 0.25f ).loopsInfinitely ();
	}
}
