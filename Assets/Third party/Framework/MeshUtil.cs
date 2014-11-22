using UnityEngine;
using System.Collections;

static class MeshUtil
{
	internal static Mesh CreateQuad ( Mesh original, int width, int height, Color color )
	{
		return CreateQuad ( original, width, height, SpriteAlignment.Center, color, color, color, color );
	}

	internal static Mesh CreateQuad ( Mesh original, int width, int height, SpriteAlignment alignment, Color topLeftColor, Color topRightColor, Color bottomLeftColor, Color bottomRightColor )
	{
		Mesh mesh = original != null ? original : new Mesh ();
		mesh.name = "Quad";

		float x1 = 0;
		float y1 = 0;
		float x2 = 0;
		float y2 = 0;

		switch ( alignment )
		{
			case SpriteAlignment.Custom:
			case SpriteAlignment.Center:
			case SpriteAlignment.BottomCenter:
			case SpriteAlignment.TopCenter:
				x1 = -width / 2;
				x2 = width / 2;
				break;

			case SpriteAlignment.BottomLeft:
			case SpriteAlignment.LeftCenter:
			case SpriteAlignment.TopLeft:
				x1 = -width;
				x2 = 0;
				break;

			case SpriteAlignment.BottomRight:
			case SpriteAlignment.RightCenter:
			case SpriteAlignment.TopRight:
				x1 = 0;
				x2 = width;
				break;
		}

		switch ( alignment )
		{
			case SpriteAlignment.Custom:
			case SpriteAlignment.Center:
			case SpriteAlignment.LeftCenter:
			case SpriteAlignment.RightCenter:
				y1 = -height / 2;
				y2 = height / 2;
				break;

			case SpriteAlignment.BottomCenter:
			case SpriteAlignment.BottomLeft:
			case SpriteAlignment.BottomRight:
				y1 = -height;
				y2 = 0;
				break;

			case SpriteAlignment.TopCenter:
			case SpriteAlignment.TopLeft:
			case SpriteAlignment.TopRight:
				y1 = 0;
				y2 = height;
				break;
		}

		Vector3[] vertices = new Vector3[]
        {
            new Vector3( x2, y2, 0),
            new Vector3( x2, y1, 0),
            new Vector3( x1, y2, 0),
            new Vector3( x1, y1, 0)
        };

		Vector2[] uv = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };

		int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };

		Color[] colors = new Color[]
		{
			topRightColor,
			bottomRightColor,
			topLeftColor,
			bottomLeftColor
		};

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.colors = colors;

		return mesh;
	}

	internal static Mesh CreatePlane ( float width, float height, int widthSegments, int heightSegments )
	{
		Mesh m = new Mesh ();
		m.name = "Quads";

		int hCount2 = widthSegments + 1;
		int vCount2 = heightSegments + 1;
		int numTriangles = widthSegments * heightSegments * 6;
		int numVertices = hCount2 * vCount2;

		Vector3[] vertices = new Vector3[numVertices];
		Vector2[] uvs = new Vector2[numVertices];
		int[] triangles = new int[numTriangles];

		int index = 0;
		float uvFactorX = 1.0f / widthSegments;
		float uvFactorY = 1.0f / heightSegments;
		float scaleX = width / widthSegments;
		float scaleY = height / heightSegments;
		for ( float y = 0.0f; y < vCount2; y++ )
		{
			for ( float x = 0.0f; x < hCount2; x++ )
			{
				vertices[index] = new Vector3 ( x * scaleX - width / 2f, 0.0f, y * scaleY - height / 2f );
				uvs[index++] = new Vector2 ( x * uvFactorX, y * uvFactorY );
			}
		}

		index = 0;
		for ( int y = 0; y < heightSegments; y++ )
		{
			for ( int x = 0; x < widthSegments; x++ )
			{
				triangles[index] = ( y * hCount2 ) + x;
				triangles[index + 1] = ( ( y + 1 ) * hCount2 ) + x;
				triangles[index + 2] = ( y * hCount2 ) + x + 1;

				triangles[index + 3] = ( ( y + 1 ) * hCount2 ) + x;
				triangles[index + 4] = ( ( y + 1 ) * hCount2 ) + x + 1;
				triangles[index + 5] = ( y * hCount2 ) + x + 1;
				index += 6;
			}
		}

		m.vertices = vertices;
		m.uv = uvs;
		m.triangles = triangles;
		m.RecalculateNormals ();

		return m;
	}

	public static void SetMesh ( GameObject gameObject, Mesh mesh, Texture2D texture )
	{
		MeshFilter filter = gameObject.GetOrCreateComponent<MeshFilter> ();
		MeshRenderer renderer = gameObject.GetOrCreateComponent<MeshRenderer> ();
		if ( renderer.sharedMaterial == null )
		{
			Shader shader = Shader.Find ( "Sprites/Default" );
			if ( shader != null )
				renderer.sharedMaterial = new Material ( shader );
		}
		if ( renderer.sharedMaterial != null )
			renderer.sharedMaterial.mainTexture = texture;

		filter.mesh = mesh;
	}

}