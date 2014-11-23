using UnityEngine;
using System.Collections;

public class EnemyLeg : MonoBehaviour
{
	public float delay;

	void Start ()
	{
		// transform.animateBounce ( 0.5f, 0, 0.15f ).delays ( 0.25f ).loopsInfinitely ();

		transform.localEulerAngles += new Vector3 ( 0, 15, 0 );
		transform.localEularAnglesTo ( 0.16f + delay, new Vector3 ( 0, 15, 0 ), true ).loops ( -1, GoLoopType.PingPong );
	}
}