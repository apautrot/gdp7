using UnityEngine;
using System.Collections;

static class RectUtil
{
	internal static Rect FromCollider ( BoxCollider2D collider )
	{
		float scaleX = 1;
		float scaleY = 1;

		if ( collider.transform.parent != null )
		{
			scaleX = collider.transform.parent.lossyScale.x;
			scaleY = collider.transform.parent.lossyScale.y;
		}

		float sizeX = collider.size.x * scaleX;
		float sizeY = collider.size.y * scaleY;
		float centerX = collider.center.x * scaleX;
		float centerY = collider.center.y * scaleY;

		float left = collider.transform.position.x - ( sizeX / 2 ) + centerX;
		float bottom = collider.transform.position.y - ( sizeY / 2 ) + centerY;
		return new Rect ( left, bottom, sizeX, sizeY );
	}
}


static class RandomInt
{
	private static System.Random rnd = new System.Random ();

	//! Range between min (included) anx max (included).
	internal static int Range ( int min, int max )
	{
		return rnd.Next ( min, max+1 );
	}
}


static class RandomBool
{
	private static System.Random rnd = new System.Random ();

	internal static bool Next ()
	{
		return ( ( rnd.Next () & 1 ) == 0 );
	}
}


static class Vector3Utils
{
	public static Vector3 getPointOnPath ( Vector3 from, Vector3 to, float atLength, float minNoise, float maxNoise )
	{
		Vector3 path = ( to - from );
		Vector3 point = ( path * atLength ) + from;
		
		Vector3 direction = path.normalized;
		bool decalLeft = RandomBool.Next ();
		Vector3 decal = new Vector3 ( direction.y, direction.x, 0 ) * Random.Range ( minNoise, maxNoise );
		if ( decalLeft ) decal *= -1;
		// Debug.Log ( "Decal " + decal + " " + decalLeft );
		// Vector3 decal = Quaternion.Euler ( 0, 0, Random.Range ( 0, 360 ) ) * ( Vector3.right * distanceRadius );
		point += decal;
		
		return point;
	}
}