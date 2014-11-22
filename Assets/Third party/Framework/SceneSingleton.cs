using UnityEngine;
using System.Collections;

public class SceneSingleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton
{
	private static T instance;
	internal static T Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = GameObject.FindObjectOfType<T> () as T;
				if ( instance == null )
					throw new System.Exception ( "Scene singleton " + typeof ( T ).ToString () + " not found in scene." );

				instance.OnFirstAccess ();
			}
			return instance;
		}
	}

	internal static bool InstanceCreated
	{
		get { return instance != null; }
	}

	public virtual void OnFirstAccess ()
	{
	}
}