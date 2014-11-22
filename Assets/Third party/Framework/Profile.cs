using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class Profile : Singleton<Profile>
{
	struct ProfileResult
	{
		internal string name;
		internal float duration;

		internal ProfileResult ( string name, float duration )
		{
			this.name = name;
			this.duration = duration;
		}
	}

	internal class ProfileCounter : System.IDisposable
	{
		internal string name;
		internal System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

		internal ProfileCounter ( string name )
		{
			this.name = name;
		}

		public void Dispose ()
		{
			watch.Stop ();
			float duration =  ( (float)watch.ElapsedMilliseconds / 1000 );
			counters.Add ( new ProfileResult ( name, duration ) );
		}
	}

	internal static ProfileCounter Method ( string name )
	{
		ProfileCounter pc = new ProfileCounter ( name );
		pc.watch.Start ();
		return pc;
	}

	static List<ProfileResult> counters = new List<ProfileResult> ();

	void FixedUpdate ()
	{
		if ( DebugWindow.InstanceCreated )
		{
			for ( int i = 0 ; i < counters.Count; i++ )
			{
				ProfileResult pr = counters[i];
				DebugWindow.Instance.AddEntry ( "Profiling", pr.name, pr.duration.ToString() );
			}
		}

		counters.Clear ();
	}
}

/*
public static class Profiler
{
	static int hash1 = Hash ( "Method" );

	static int Hash ( string name )
	{
		return name.GetHashCode();
	}

	internal static void Method ( int token )
	{

	}
}


class Test
{
	ProfiledMethod __MyMethod;

	void MyMethod ()
	{
		Profiler.Method ( "MyMethod" );
	}
}
*/