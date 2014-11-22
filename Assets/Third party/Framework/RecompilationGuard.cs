using UnityEngine;
using System.Collections;

public class RecompilationGuard : MonoBehaviour
{
	private static bool startMethodHasBeenCalled;

	RecompilationGuard ()
	{
	}

	void Start ()
	{
		startMethodHasBeenCalled = true;
	}

	void Update ()
	{
		if ( ! startMethodHasBeenCalled )
		{
			startMethodHasBeenCalled = true;

			// Debug.Log ( "Compilation detected, restarting..." );

			GameObjects.BroadcastMessageToScene ( "Restart" );
			// GameObjects.BroadcastMessageToScene ( "Start" );
		}
	}
}
