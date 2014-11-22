using UnityEngine;
using System.Collections;

public class ClockQuad : MonoBehaviour
{
	public Texture texture;
	public VertexBatch vertexBatch = new VertexBatch ();
	public float startAngle = 0;
	public float stopAngle = -25;

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.A ) )
		{
			Mesh mesh = CreateMesh ( 1, 1, startAngle, stopAngle, null );
			renderer.material.mainTexture = texture;
			GetComponent<MeshFilter> ().mesh = mesh;
		}
	}

	public Mesh CreateMesh ( float width, float height, float startAngle, float stopAngle, Mesh reuse = null )
	{
		const int MaxCount = 4;

		float hw = width / 2;
		float hh = height / 2;

		int[] indices = new int[MaxCount * 3];
		vertexBatch.setSize ( MaxCount * 3 );

		int idx = 0;

		Vector2 a = new Vector2 ( -hw, hh );
		Vector2 b = new Vector2 ( hw, hh );
		Vector2 z = new Vector2 ( 0, 0 );
		Vector2 auv = new Vector2 ( 0, 1 );
		Vector2 buv = new Vector2 ( 1, 1 );
		Vector2 zuv = new Vector2 ( 0.5f, 0.5f );
		float lower = -45;
		float upper = 45;

		float radius = Mathf.Max ( width, height );

		CreateQuarterGeometry ( radius, a, b, z, auv, buv, zuv, lower, upper, ref idx, ref indices );

		Mesh mesh = reuse != null ? reuse : new Mesh ();
		mesh.triangles = null;
		mesh.vertices = vertexBatch.vertices;
		mesh.uv = vertexBatch.uvs;
		mesh.colors = vertexBatch.colors;
		mesh.triangles = indices;

		return mesh;
	}

	private void CreateQuarterGeometry
	(
		float radius,
		Vector2 a,
		Vector2 b,
		Vector2 z,
		Vector2 auv,
		Vector2 buv,
		Vector2 zuv,
		float lower,
		float upper,
		ref int idx,
		ref int[] indices
	)
	{
		Vector2 ip = Vector2.zero;		// Intersection Point
		Vector2 ipuv = Vector2.zero;

		if ( ( stopAngle >= lower ) || ( startAngle < upper ) )
		{
			if ( stopAngle >= lower )
			{
				vertexBatch.setVertex ( idx, z.x, z.y, zuv.x, zuv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;

				vertexBatch.setVertex ( idx, a.x, a.y, auv.x, auv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;

				Vector2 line = new Vector2 ( 0, radius ).Rotate ( stopAngle ) + z;		// top is ( 0, radius ), top is 0 degree angle
				Vector2ExtensionMethods.LineIntersectionPoint ( new Vector2 ( a.x, a.y ), new Vector2 ( b.x, b.y ), Vector2.zero, line, ref ip );

				Vector2 lineuv = new Vector2 ( 0, 2 ).Rotate ( stopAngle ) + zuv;
				Vector2ExtensionMethods.LineIntersectionPoint ( new Vector2 ( auv.x, auv.y ), new Vector2 ( buv.x, buv.y ), Vector2.zero, lineuv, ref ipuv );

				vertexBatch.setVertex ( idx, ip.x, ip.y, ipuv.x, ipuv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;
			}

			if ( startAngle < upper )
			{
				vertexBatch.setVertex ( idx, z.x, z.y, zuv.x, zuv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;

				Vector2 clockEnd = new Vector2 ( 0, radius ).Rotate ( startAngle ) + z;	// top is ( 0, radius ), top is 0 degree angle
				Vector2ExtensionMethods.LineIntersectionPoint ( new Vector2 ( a.x, a.y ), new Vector2 ( b.x, b.y ), Vector2.zero, clockEnd, ref ip );

				Vector2 lineuv = new Vector2 ( 0, 2 ).Rotate ( stopAngle ) + zuv;
				Vector2ExtensionMethods.LineIntersectionPoint ( new Vector2 ( auv.x, auv.y ), new Vector2 ( buv.x, buv.y ), Vector2.zero, lineuv, ref ipuv );

				vertexBatch.setVertex ( idx, ip.x, ip.y, ipuv.x, ipuv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;

				vertexBatch.setVertex ( idx, b.x, b.y, buv.x, buv.y, Color.white );
				for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
				idx++;
			}
		}
		else if ( ( stopAngle < lower ) && ( startAngle >= upper ) )
		{
		}
		else
		{
			vertexBatch.setVertex ( idx, z.x, z.y, zuv.x, zuv.y, Color.white );
			for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
			idx++;

			vertexBatch.setVertex ( idx, a.x, b.y, auv.x, auv.y, Color.white );
			for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
			idx++;

			vertexBatch.setVertex ( idx, b.x, b.y, buv.x, buv.y, Color.white );
			for ( int i = 0; i < 3; i++ ) indices[idx + i] = idx + i;
			idx++;
		}
	}
}
