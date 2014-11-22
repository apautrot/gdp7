using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Quad : MonoBehaviour
{
	public Color TopLeftColor = Color.gray;
	public Color TopRightColor = Color.gray;
	public Color BottomLeftColor = Color.gray;
	public Color BottomRightColor = Color.gray;
	public int Width = 1;
	public int Height = 1;
	public SpriteAlignment Alignment;

	void Awake ()
	{
		Recreate ();
	}

#if UNITY_EDITOR

//	private int lastChangesHash;
	int Hash
	{
		get
		{
			return Width.GetHashCode ()
				& Height.GetHashCode ()
				& TopLeftColor.GetHashCode ()
				& TopRightColor.GetHashCode ()
				& BottomLeftColor.GetHashCode ()
				& BottomRightColor.GetHashCode ()
				& Alignment.GetHashCode ();
		}
	}

	void Update ()
	{
// 		//Debug.Log ( "Checking hashes : " + lastChangesHash + " == " + Hash );
// 		if ( lastChangesHash != Hash )
// 		{
// 			//Debug.Log ( "Changes detected" );
// 			Recreate ();
// 		}
	}

#endif

	public void Recreate ()
	{
// #if UNITY_EDITOR
// 		lastChangesHash = Hash;
// 		//Debug.Log ( "Hash = " + lastChangesHash );
// #endif

		MeshFilter filter = gameObject.GetOrCreateComponent<MeshFilter> ();
		MeshRenderer renderer = gameObject.GetOrCreateComponent<MeshRenderer> ();

		if ( renderer.sharedMaterial == null )
		{
			Shader shader = Shader.Find ( "Sprites/Default" );
			if ( shader != null )
				renderer.sharedMaterial = new Material ( shader );
		}

//		if ( ( Width > 0 ) && ( Height > 0 ) )
		if ( Application.isEditor )
			filter.mesh = MeshUtil.CreateQuad ( null, Width, Height, Alignment, TopLeftColor, TopRightColor, BottomLeftColor, BottomRightColor );
	}
}
