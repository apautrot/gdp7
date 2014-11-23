using UnityEngine;
using System.Collections;

public class EnemyExplosion : MonoBehaviour
{
	public GameObject empCircle;
	private bool started = false;

	void Start ()
	{
		if ( empCircle != null )
		{
			empCircle.transform.scaleFrom ( 1.0f, 0 );
			empCircle.renderer.material.alphaTo ( 0.75f, 0 ).delays ( 0.25f );
		}
		
		// gameObject.particleSystem.Play ();
	}
	
	void FixedUpdate ()
	{
		if ( ! started )
			started = gameObject.particleSystem.particleCount > 0;
		else
			if ( gameObject.particleSystem.particleCount == 0 )
				gameObject.DestroySelf ();
	}
}
