using UnityEngine;

static class Vector3ExtensionMethods
{
	public static Vector3 NormalizeValues ( this Vector3 self )
	{
		if ( ( self.x > self.y ) && ( self.x > self.z ) )
		{
			self.y /= self.x;
			self.z /= self.x;
			self.x = 1;
		}
		else if ( ( self.y > self.x ) && ( self.y > self.z ) )
		{
			self.x /= self.y;
			self.z /= self.y;
			self.y = 1;
		}
		else if ( ( self.z > self.x ) && ( self.z > self.y ) )
		{
			self.x /= self.z;
			self.y /= self.z;
			self.z = 1;
		}
		return self;
	}

	public static Vector3 EulerRotate ( this Vector3 self, float xAngle, float yAngle, float zAngle )
	{
		return Quaternion.Euler ( xAngle, yAngle, zAngle ) * self;
	}

	public static Vector3 AxisRotate ( this Vector3 self, float angle, Vector3 axis )
	{
		return Quaternion.AngleAxis ( angle, axis ) * self;
	}

	public static float Angle ( this Vector3 self, Vector3 other )
	{
		float angle = Mathf.Atan2 ( other.x - self.x, other.y - self.y ) * Mathf.Rad2Deg;
		return angle;
	}

	public static Vector3 AngleNormalize ( this Vector3 self )
	{
		self.x = ( self.x + 360 ) % 360;
		self.y = ( self.y + 360 ) % 360;
		self.z = ( self.z + 360 ) % 360;
		return self;
	}

	public static Vector3 AngleDelta ( this Vector3 self, Vector3 other )
	{
		return new Vector3 ( Mathf.DeltaAngle ( self.x, other.x ), Mathf.DeltaAngle ( self.y, other.y ), Mathf.DeltaAngle ( self.z, other.z ) );
	}

	public static Vector3 RotateX ( this Vector3 self, float angle )
	{
		float sin = Mathf.Sin ( angle );
		float cos = Mathf.Cos ( angle );

		float ty = self.y;
		float tz = self.z;
		self.y = ( cos * ty ) - ( sin * tz );
		self.z = ( cos * tz ) + ( sin * ty );

		return self;
	}

	public static Vector3 RotateY ( this Vector3 self, float angle )
	{
		float sin = Mathf.Sin ( angle );
		float cos = Mathf.Cos ( angle );

		float tx = self.x;
		float tz = self.z;
		self.x = ( cos * tx ) + ( sin * tz );
		self.z = ( cos * tz ) - ( sin * tx );

		return self;
	}

	public static Vector3 RotateZ ( this Vector3 self, float angle )
	{
		float sin = Mathf.Sin ( angle );
		float cos = Mathf.Cos ( angle );

		float tx = self.x;
		float ty = self.y;
		self.x = ( cos * tx ) - ( sin * ty );
		self.y = ( cos * ty ) + ( sin * tx );

		return self;
	}

	public static float GetPitch ( this Vector3 self )
	{
		float len = Mathf.Sqrt ( ( self.x * self.x ) + ( self.z * self.z ) );    // Length on xz plane.
		return ( -Mathf.Atan2 ( self.y, len ) );
	}

	public static float GetYaw ( this Vector3 self )
	{
		return ( Mathf.Atan2 ( self.x, self.z ) );
	}
}



static class Vector2ExtensionMethods
{
	public static Vector2 Rotate ( this Vector2 self, float angle )
	{
		float sin = Mathf.Sin ( angle );
		float cos = Mathf.Cos ( angle );

		float tx = self.x;
		float ty = self.y;
		self.x = ( cos * tx ) - ( sin * ty );
		self.y = ( cos * ty ) + ( sin * tx );

		return self;
	}

	public static Vector2 Add ( this Vector2 self, float x, float y )
	{
		self.x += x;
		self.y += y;
		return self;
	}

	public static bool LineIntersectionPoint ( Vector2 axy, Vector2 axy2, Vector2 bxy, Vector2 bxy2, ref Vector2 result )
	{
		// Get A,B,C of first line - points : ps1 to pe1
		float A1 = axy2.y - axy.y;
		float B1 = axy.x - axy2.x;
		float C1 = A1 * axy.x + B1 * axy.y;

		// Get A,B,C of second line - points : ps2 to pe2
		float A2 = bxy2.y - bxy.y;
		float B2 = bxy.x - bxy2.x;
		float C2 = A2 * bxy.x + B2 * bxy.y;

		// Get delta and check if the lines are parallel
		float delta = A1 * B2 - A2 * B1;
		if ( delta == 0 )
		{
			return false;
		}
		else
		{
			// now return the Vector2 intersection point
			result = new Vector2 (
				( B2 * C1 - B1 * C2 ) / delta,
				( A1 * C2 - A2 * C1 ) / delta
			);
			return true;
		}
	}
}