using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextMeshEx : MonoBehaviour
{
	// http://wiki.unity3d.com/index.php?title=Expose_properties_in_inspector
	public string Text = "Hello world";
	// public float OffsetZ;
	public float CharacterSize = 1;
	public float LineMaxWidth = 0;
	//public float LineSpacing = 1;
	public TextAnchor Anchor;
	public TextAlignment Alignement;
	//public bool RichText;
	public Color Color = Color.white;
	public Font Font;

	private TextRenderer textRenderer = new TextRenderer ();
	public TextRenderer TextRenderer
	{
		get { return textRenderer; }
	}

	public float Height
	{
		get
		{
			return textRenderer.Dimension.y;
		}
	}

	void Start ()
	{
		Recreate ();
	}

	public void Recreate ()
	{
		textRenderer.Text = Text;
		textRenderer.Scale = CharacterSize;
		textRenderer.Alignment = Alignement;
		textRenderer.Anchor = Anchor;
		textRenderer.Color = Color;
		textRenderer.Font = Font;
		textRenderer.LineMaxWidth = LineMaxWidth > 0 ? LineMaxWidth : float.MaxValue;

		UpdateGeometry ();
	}

	public void UpdateGeometry ()
	{
		MeshFilter filter = gameObject.GetOrCreateComponent<MeshFilter> ();
		MeshRenderer renderer = gameObject.GetOrCreateComponent<MeshRenderer> ();

		if ( Font != null )
		{
			if ( renderer.sharedMaterial == null )
				renderer.sharedMaterial = Font.material;
			filter.mesh = textRenderer.Mesh;
		}
		else
			filter.mesh = null;
	}

}