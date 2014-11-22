using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FontData
{
	public float CapHeight
	{
		get;
		private set;
	}

	public float XHeight
	{
		get;
		private set;
	}

	public float AscenderHeight
	{
		get;
		private set;
	}

	public float DescenderHeight
	{
		get;
		private set;
	}

	public float FontHeight
	{
		get { return AscenderHeight + XHeight + DescenderHeight; }
	}

	public float LineHeight
	{
		get;
		private set;
	}

	public Font Font
	{
		get;
		private set;
	}

	private string xHeightLetters = "xeaonsrcumvwz";
	public string XHeightLetters
	{
		get { return xHeightLetters; }
		set
		{
			xHeightLetters = value;
			ComputeXHeightAndAscenderHeight ();
			ComputeDescenderHeight (); // dependent of xHeight
		}
	}

	private void ComputeXHeightAndAscenderHeight ()
	{
		XHeight = 0;
		for ( int i = 0 ; i < xHeightLetters.Length; i++ )
		{
			char c = xHeightLetters[i];
			CharacterInfo ci;
			if ( Font.GetCharacterInfo ( c, out ci ) )
			{
				XHeight = System.Math.Abs ( ci.vert.height );
				AscenderHeight = System.Math.Abs ( ci.vert.height ) - System.Math.Abs ( ci.vert.y );
				return;
			}
		}
	}

	private string capLetters = "MNBDCEFKAGHIJLOPQRSTUVWXYZ";
	public string CapLetters
	{
		get { return capLetters; }
		set
		{
			capLetters = value;
			ComputeCapHeight ();
		}
	}

	private void ComputeCapHeight ()
	{
		CapHeight = 0;
		for ( int i = 0; i < capLetters.Length; i++ )
		{
			char c = capLetters[i];
			CharacterInfo ci;
			if ( Font.GetCharacterInfo ( c, out ci ) )
			{
				CapHeight = System.Math.Abs ( ci.vert.height );
				return;
			}
		}
	}

	private string descenderLetters = "gpqyj";
	public string DescenderLetters
	{
		get { return descenderLetters; }
		set
		{
			descenderLetters = value;
			ComputeDescenderHeight ();
		}
	}

	private void ComputeDescenderHeight ()
	{
		DescenderHeight = 0;
		for ( int i = 0; i < descenderLetters.Length; i++ )
		{
			char c = descenderLetters[i];
			CharacterInfo ci;
			if ( Font.GetCharacterInfo ( c, out ci ) )
			{
				DescenderHeight = System.Math.Abs ( ci.vert.height ) - XHeight;
				return;
			}
		}
	}

	private void ComputeLineHeight ()
	{
		LineHeight = FontHeight; // 1.62f;
	}

	public FontData ( Font font )
	{
		Font = font;
		Font.textureRebuildCallback += OnFontTextureReduild;
		EnsureFontAlphabet ();
	}

	internal void EnsureFontAlphabet()
	{
		const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 .,:;!?-+/*&'\"()[]îéêèàâçùû";
		Font.RequestCharactersInTexture ( alphabet );
		ComputeCapHeight ();
		ComputeXHeightAndAscenderHeight ();
		ComputeDescenderHeight (); // dependent of xHeight
		ComputeLineHeight ();
	}

	private void OnFontTextureReduild ()
	{
		EnsureFontAlphabet ();
	}
}

public class FontDepot
{
	private static FontDepot instance;
	public static FontDepot Instance
	{
		get
		{
			if ( instance == null )
				instance = new FontDepot();
			return instance;
		}
	}

	private Dictionary<Font,FontData> fontData = new Dictionary<Font,FontData>();

	public FontData GetData ( Font font )
	{
		FontData fontData = null;
		this.fontData.TryGetValue ( font, out fontData );
		if ( fontData == null )
		{
			fontData = new FontData ( font );
			this.fontData[font] = fontData;
		}

#if UNITY_EDITOR
		// fix problems with font texture rebuild
		if ( Application.isEditor && ! Application.isPlaying )
			fontData.EnsureFontAlphabet ();
#endif

		return fontData;
	}
}


public enum TextAlignment
{
	Left,
	Center,
	Right,
	Justify
}

public class TextRenderer
{
	public class LineDescriptor
	{
		internal int charStartIndexInText;
		internal int charCount;
		internal int quadStartIndexInBatch;
		internal int quadCount;
		internal bool issuedFromLineCut;

		internal LineDescriptor ( int startIndexInText, int length, bool issuedFromLineCut )
		{
			this.charStartIndexInText = startIndexInText;
			this.charCount = length;
			this.issuedFromLineCut = issuedFromLineCut;
		}

		internal LineDescriptor ( int startIndexInText, int length, int quadIndexInBatch, int quadCount, bool issuedFromLineCut )
		{
			this.charStartIndexInText = startIndexInText;
			this.charCount = length;
			this.quadStartIndexInBatch = quadIndexInBatch;
			this.quadCount = quadCount;
			this.issuedFromLineCut = issuedFromLineCut;
		}
	}

	protected Font font;
	protected FontData fontData;
	protected QuadBatch quadBatch = new QuadBatch ();
	protected Mesh mesh;
	protected Vector3 position = new Vector3 ();
	protected Vector2 dimension = new Vector2 ();
	protected float scale = 1;
	protected TextAlignment alignment = TextAlignment.Left;
	protected float lineMaxWidth = float.MaxValue;
	protected bool preserveWords = true;
	protected Color color = Color.white;
	protected List<LineDescriptor> lineList = new List<LineDescriptor> ();
	protected string text = "";
	protected Rect bounds = new Rect ();
	// protected bool trimTopLeftSpace = true;
	protected float rotation = 0.0f;

	private bool quadBatchUpdateNeeded = true;
	private bool meshUpdateNeeded = true;

	public TextRenderer ()
	{
	}

	public TextRenderer ( Font font )
	{
		Font = font;
	}

	public TextRenderer ( TextRenderer text )
	{
		Set ( text );
	}

	public QuadBatch QuadBatch
	{
		get { return quadBatch; }
		set
		{
			quadBatch = value;
			meshUpdateNeeded = true;
		}
	}

	public Font Font
	{
		get { return font; }
		set
		{
			this.font = value;
			if ( font != null )
				fontData = FontDepot.Instance.GetData ( font );
			quadBatchUpdateNeeded = true;
		}
	}

	void Set ( TextRenderer bitmapText )
	{
		this.font = bitmapText.font;
		this.fontData = bitmapText.fontData;
		this.scale = bitmapText.scale;
		this.color = bitmapText.color;
		this.lineMaxWidth = bitmapText.lineMaxWidth;
		this.alignment = bitmapText.alignment;
		this.text = bitmapText.text;
//		this.setPosition ( bitmapText.getX (), bitmapText.getY () );
	}

	public float LineMaxWidth
	{
		get { return lineMaxWidth; }
		set
		{
			lineMaxWidth = value;
			quadBatchUpdateNeeded = true;
		}
	}

	public float Scale
	{
		get { return scale; }
		set
		{
			scale = value;
			quadBatchUpdateNeeded = true;
		}
	}

// 	public bool TrimTopLeftSpace
// 	{
// 		get { return trimTopLeftSpace; }
// 		set
// 		{
// 			trimTopLeftSpace = value;
// 			updateNeeded = true;
// 		}
// 	}

	public void SetEmptyText ()
	{
		text = "";
		quadBatch.setSize ( 0 );
		dimension = Vector2.zero;
		bounds = new Rect();
		quadBatchUpdateNeeded = true;
	}

	public Color Color
	{
		get { return color; }
		set
		{
			color = value;
			quadBatch.colorize ( 0, quadBatch.Count, color );
		}
	}

	public float Alpha
	{
		get { return color.a; }
		set
		{
			color.a = value;
			quadBatch.colorize ( 0, quadBatch.Count, color );
		}
	}

	public void SetCharColor ( int index, Color color )
	{
		quadBatch.colorize ( index, 1, color );
	}

	public TextAlignment Alignment
	{
		get { return alignment; }
		set
		{
			alignment = value;
			quadBatchUpdateNeeded = true;
		}
	}

	private TextAnchor anchor;
	public TextAnchor Anchor
	{
		get { return anchor; }
		set
		{
			anchor = value;
			quadBatchUpdateNeeded = true;
		}
	}

	public float LineHeight
	{
		get { return fontData.LineHeight * scale; }
		set
		{
			scale = value / fontData.LineHeight;
			quadBatchUpdateNeeded = true;
		}
	}

	public void NotifyQuadBatchHaveBeenChanged()
	{
		meshUpdateNeeded = true;
	}

	public Mesh Mesh
	{
		get
		{
			if ( quadBatchUpdateNeeded )
			{
				RecreateQuadBatch ();
				NotifyQuadBatchHaveBeenChanged ();
				quadBatchUpdateNeeded = false;
			}

			if ( meshUpdateNeeded )
			{
				mesh = quadBatch.CreateMesh ( mesh );
				meshUpdateNeeded = false;
			}

			return mesh;
		}
	}

	public string Text
	{
		get { return text; }
		set
		{
			text = value;
			quadBatchUpdateNeeded = true;
		}
	}

	public Vector2 Dimension
	{
		get { return dimension; }
	}

// 	public List<LineDescriptor> Lines
// 	{
// 		get
// 		{
// 			return lineList;
// 		}
// 	}

	private class CreateTextGeometryResult
	{
		public int processedCharCount;
		public int renderedCharCount;
		public int spaceRemovedAtBeginning;
	}

	private CreateTextGeometryResult createTextGeometry ( Vector3 position, string text, int startIndex, int length, int quadStartIndex, bool lineIsIssuedFromLineCut, float scaleFactor, float lineMaxWidth, bool preserveWords )
	{
		CreateTextGeometryResult result = new CreateTextGeometryResult ();

		if ( lineIsIssuedFromLineCut )
		{
			// trim spaces at start
			while ( ( startIndex < startIndex + length ) && ( ( text[startIndex] == 0x20 ) || ( text[startIndex] == 0xA0 ) ) )
			{
				startIndex++;
				length--;
				result.processedCharCount++;
				result.spaceRemovedAtBeginning++;
			}
		}

		// 		float pageWidth = font.getRegion ().getRegionWidth ();
		// 		float pageHeight = font.getRegion ().getRegionHeight ();
		// 		Glyph glyph = null;
		CharacterInfo glyph;
		// Font.fontChar letterStruct = null;
		Vector3 letterPosition = new Vector3 ();
		Vector2 letterDimension = new Vector2 ();
		Vector2 letterTopLeftTextCoord = new Vector2 ();
		Vector2 letterBottomRightTextCoord = new Vector2 ();
		float advance = 0;
		int lastSpaceIndex = startIndex;
		for ( int i = startIndex; i < startIndex + length; i++ )
		{
			char charToRender = text[i];
			if ( charToRender == 0xA0 )
				charToRender = (char)0x20; // convert unbreakable space into space

			if ( !font.GetCharacterInfo ( charToRender, out glyph ) )
				font.GetCharacterInfo ( ' ', out glyph );

			// 			glyph = font.getData ().getGlyph ( charToRender );
			// 			if (glyph == null)
			// 				// throw new RuntimeException ( "Character '" + charToRender + "' is not defined in font" );
			// 				glyph = font.getData ().getGlyph ( ' ' );

			int kerning = 0; // kerning to add
			// if (i > 0)
			// {
			// char previousCharToRender = text.charAt ( i - 1 );
			// if (previousCharToRender == 0xA0)
			// previousCharToRender = (char) 0x20; // convert unbreakable space into space
			//
			// font.kerningTable.TryGetValue ( new KeyValuePair<char, char> ( previousCharToRender, charToRender ), out kerning );
			// }

			if ( charToRender == 0x20 )
			{
				lastSpaceIndex = i; // keep index of last space

				// space special case
				letterPosition.x = advance + kerning + glyph.vert.x;
				letterPosition.y = ( glyph.vert.y + glyph.vert.height );
				letterDimension.x = (float)glyph.vert.width;
				letterDimension.y = glyph.vert.height; // 0.0f;
				letterTopLeftTextCoord.x = glyph.uv.xMin;
				letterTopLeftTextCoord.y = glyph.uv.yMin;
				letterBottomRightTextCoord.x = glyph.uv.xMax;
				letterBottomRightTextCoord.y = glyph.uv.yMax;
			}
			else
			{
				letterPosition.x = advance + kerning + glyph.vert.x;
				letterPosition.y = ( glyph.vert.y + glyph.vert.height );
				letterDimension.x = glyph.vert.width;
				letterDimension.y = -glyph.vert.height;
				letterTopLeftTextCoord.x = glyph.uv.xMin;
				letterTopLeftTextCoord.y = glyph.uv.yMin;
				letterBottomRightTextCoord.x = glyph.uv.xMax;
				letterBottomRightTextCoord.y = glyph.uv.yMax;
			}

			letterPosition *= scaleFactor; // apply scaling
			letterDimension *= scaleFactor;

			// check if text is over the max line width
			if ( letterPosition.x + letterDimension.x > lineMaxWidth )
			{
				// generation is not finished, return count of processed characters
				if ( preserveWords && ( lastSpaceIndex > startIndex ) )
				{
					// cut where the last space char was found, will truncate and discard all chars after
					result.processedCharCount -= ( i - lastSpaceIndex );
					result.renderedCharCount -= ( i - lastSpaceIndex );
					break;
				}
				else
				{
					break; // line break in the middle of a word
				}
			}

			letterPosition += position; // then offset by position

			quadBatch.setQuad ( quadStartIndex + result.renderedCharCount, letterPosition, letterDimension, letterTopLeftTextCoord, letterBottomRightTextCoord, color );
			if ( glyph.flipped )
				quadBatch.flipUvs ( quadStartIndex + result.renderedCharCount );

			advance += glyph.width;
			result.processedCharCount++;
			result.renderedCharCount++;
		}

		return result;
	}

// 	public void setText ( string text )
// 	{
// 		multiline = false;
// 		this.text = text;
// 
// 		quadBatch.setSize ( text.Length );
// 
// 		if ( text.Length > 0 )
// 		{
// 			CreateTextGeometryResult result = createTextGeometry ( position, text, 0, text.Length, 0, true, scale, lineMaxWidth, preserveWords );
// 			if ( quadBatch.Count != result.renderedCharCount )
// 				quadBatch.setSize ( result.renderedCharCount );
// 
// 			Rect quadBounds = quadBatch.getBounds ();
// 			dimension.x = quadBounds.width;
// 			dimension.y = quadBounds.height;
// 			bounds.width = quadBounds.width;
// 			bounds.height = quadBounds.height;
// 
// 			if ( trimTopLeftSpace ) // remove top left empty space created by glyph xoffset/yoffset
// 			{
// 				position.x = quadBounds.x;
// 				position.y = quadBounds.y;
// 				bounds.x = quadBounds.x;
// 				bounds.y = quadBounds.y;
// 			}
// 		}
// 		else
// 		{
// 			dimension.x = 0;
// 			dimension.y = 0;
// 			bounds.width = 0;
// 			bounds.height = 0;
// 		}
// 	}

	public void RecreateQuadBatch ()
	{
		//
		// first, cut into lines
		//

		// the line list is a list of pair <first char index, line text>
		lineList.Clear ();
		{
			int startIndex = 0;
			int lineBreakIndex = -1;
			while ( ( lineBreakIndex = text.IndexOf ( '\n', startIndex ) ) != -1 )
			{
				lineList.Add ( new LineDescriptor ( startIndex, lineBreakIndex - startIndex, false ) );
				startIndex = lineBreakIndex + 1; // start index of next line
			}
			lineList.Add ( new LineDescriptor ( startIndex, text.Length - startIndex, false ) );
		}

		//
		// new create lines geometry
		//

		quadBatch.setSize ( text.Length );

		int lineIndex = 0;
		Vector3 linePosition = new Vector3 ( position.x, position.y, position.z );
		int quadStartIndex = 0;
		while ( lineIndex < lineList.Count )
		{
			// TODO : optim : "lineList.get( lineIndex )" as local ?
			int startIndex = lineList[lineIndex].charStartIndexInText;
			int length = lineList[lineIndex].charCount;
			bool issuedFromLineCut = lineList[lineIndex].issuedFromLineCut;

			CreateTextGeometryResult result = createTextGeometry ( linePosition, text, startIndex, length, quadStartIndex, issuedFromLineCut, scale, lineMaxWidth, preserveWords );

			if ( ( result.processedCharCount > 0 ) && ( result.processedCharCount != length ) ) // line not fully processed ? (see line break near end of
			// createTextGeometry method)
			{
				// insert remaining of the line
				lineList.Insert ( lineIndex + 1, new LineDescriptor ( startIndex + result.processedCharCount, length - result.processedCharCount, true ) );
			}

			// updates line description
			lineList[lineIndex] = new LineDescriptor ( startIndex + result.spaceRemovedAtBeginning, result.processedCharCount - result.spaceRemovedAtBeginning, quadStartIndex, result.renderedCharCount, issuedFromLineCut );

			quadStartIndex += result.renderedCharCount; // moves into the quad buffer

			linePosition.y -= ( fontData.LineHeight * scale ); // TODO : might be an option instead of lineHeight : result.boundingRectMax.Y -
			// result.boundingRectMin.Y;
			lineIndex++;
		}

		quadBatch.setSize ( quadStartIndex );

		//
		// align each line's geometry
		//

		dimension.Set ( 0, 0 );
		Rect quadBounds = new Rect ( 0, 0, -1, -1 );

		if ( lineList.Count > 1 )
		{
			float lineWidth = lineMaxWidth;

			if ( lineMaxWidth == float.MaxValue )
			{
				quadBounds = quadBatch.getBounds ();
				dimension.x = quadBounds.width;
				dimension.y = quadBounds.height;
				lineWidth = dimension.x;
			}

			switch ( alignment )
			{
				case TextAlignment.Left:
					// default alignment
					break;

				case TextAlignment.Center:
					for ( int i = 0; i < lineList.Count; i++ )
					{
						LineDescriptor lineDesc = lineList[i];
						Rect boundingRect = quadBatch.getBounds ( lineDesc.quadStartIndexInBatch, lineDesc.quadCount );
						quadBatch.translate ( lineDesc.quadStartIndexInBatch, lineDesc.quadCount, new Vector3 ( ( lineWidth - boundingRect.width ) / 2, 0, 0 ) );
					}
					break;

				case TextAlignment.Right:
					for ( int i = 0; i < lineList.Count; i++ )
					{
						LineDescriptor lineDesc = lineList[i];
						Rect boundingRect = quadBatch.getBounds ( lineDesc.quadStartIndexInBatch, lineDesc.quadCount );
						quadBatch.translate ( lineDesc.quadStartIndexInBatch, lineDesc.quadCount, new Vector3 ( lineWidth - boundingRect.width, 0, 0 ) );
					}
					break;

				case TextAlignment.Justify:
					for ( int i = 0; i < lineList.Count; i++ )
					{
						LineDescriptor lineDesc = lineList[i];

						// line is cut if next line is issued from the cut
						bool lineWasCut = ( i < lineList.Count - 1 ) && ( lineList[i + 1].issuedFromLineCut );

						// TODO : add right/centered aligned justify as Photoshop do ?
						if ( lineWasCut ) // only align line that are cut, others are already left aligned
						{
							// get space count in that line
							List<int> charCountsToProcess = new List<int> ();
							int noSpaceCharCount = 0;
							for ( int j = 0; j < lineDesc.charCount; j++ )
							{
								char charToRender = text[lineDesc.charStartIndexInText + j];
								if ( charToRender == 0xA0 )
									charToRender = (char)0x20; // convert unbreakable space into space

								if ( charToRender == 0x20 )
								{
									charCountsToProcess.Add ( noSpaceCharCount );
									noSpaceCharCount = 0;
								}
								else
									noSpaceCharCount++;
							}
							charCountsToProcess.Add ( noSpaceCharCount );
							if ( charCountsToProcess.Count > 1 )
							{
								Rect boundingRect = quadBatch.getBounds ( lineDesc.quadStartIndexInBatch, lineDesc.quadCount );
								float spaceToAdd = ( lineWidth - boundingRect.width ) / ( charCountsToProcess.Count - 1 );

								if ( spaceToAdd > 0 )
								{
									// distribute translation to each spacing
									int quadIndex = lineDesc.quadStartIndexInBatch;
									int quadCount = 0;
									for ( int j = 0; j < charCountsToProcess.Count; j++ )
									{
										quadCount = charCountsToProcess[j];
										if ( quadCount > 0 )
										{
											// don't process first word, that stay left aligned
											if ( j > 0 )
											{
												// quadBatch.colorize ( quadIndex, 1, Color.RED ); // DEBUG : colorize first letter in red
												quadBatch.translate ( quadIndex, quadCount, new Vector3 ( spaceToAdd * j, 0, 0 ) );
											}
											quadIndex += charCountsToProcess[j];
										}

										quadIndex++; // skip space char, that have not to be processed
									}
								}
							}
						}
					}
					break;
			}
		}

		if ( quadBounds.width == -1 || quadBounds.height == -1 ) // might have been already computed above, don't do it twice
		{
			quadBounds = quadBatch.getBounds ();
			dimension.x = quadBounds.width;
			dimension.y = quadBounds.height;
		}

		bounds.width = dimension.x;
		bounds.height = dimension.y;

// 		if ( trimTopLeftSpace ) // remove empty top left space created by glyph xoffset/yoffset
// 		{
// 			position.x = quadBounds.x;
// 			position.y = quadBounds.y;
// 			bounds.x = quadBounds.x;
// 			bounds.y = quadBounds.y;
// 		}

		float dx = 0;
		switch ( anchor )
		{
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				dx = - bounds.width / 2;
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				dx = - bounds.width;
				break;
		}

		float dy = -( quadBounds.y + quadBounds.height );
		switch ( anchor )
		{
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleRight:
				dy += bounds.height / 2;
				break;
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerRight:
				dy += bounds.height;
				break;
		}

		quadBatch.translate ( new Vector3 ( dx, dy, 0 ) );
	}

	public void AlignVerticesOnIntegers ()
	{
		quadBatch.alignVerticesOnIntegers ();
	}

	public Rect Bounds
	{
		get { return bounds; }
	}

// 	public float getLineBaseHeight ()
// 	{
// 		if ( multiline == true )
// 			return -1;
// 
// 		string ypqgj = "ypqgj";
// 
// 		float baseLineHeight = 0.0f;
// 		for ( int i = 0; i < text.Length; i++ )
// 		{
// 			char c = text[i];
// 			float height = 0;
// 			if ( char.IsLower ( c ) && ( ypqgj.IndexOf ( c ) != -1 ) )
// 				height = fontData.XHeight;
// 			else
// 			{
// 				CharacterInfo ci = font.characterInfo[0];
// 				// 				Glyph glyph = font.getData ().getGlyph ( c );
// 				// 				height = glyph.height /* + glyph.yoffset* /;
// 			}
// 
// 			baseLineHeight = System.Math.Max ( baseLineHeight, height );
// 		}
// 
// 		return baseLineHeight * scale;
// 	}
}

