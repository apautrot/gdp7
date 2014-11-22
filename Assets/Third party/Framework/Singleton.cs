using UnityEngine;
using System.Collections;

public interface ISingleton
{
	void OnFirstAccess ();
}

public class Singleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = GameObject.FindObjectOfType<T> () as T;
				if ( instance == null )
				{
					GameObject singletonManagerObject = GameObject.Find ( "Singletons" );
					if ( singletonManagerObject == null )
					{
						singletonManagerObject = new GameObject ();
						singletonManagerObject.name = "Singletons";
					}

					instance = singletonManagerObject.AddComponent<T> ();
				}

				GameObject.DontDestroyOnLoad ( instance.gameObject );
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