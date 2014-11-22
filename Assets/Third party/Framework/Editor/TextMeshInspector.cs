using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
 
[CustomEditor(typeof(TextMesh))]
public class TextMeshInspector : Editor
{
    public override void OnInspectorGUI ()
    {
        this.DrawDefaultInspector();    
        TextMesh textMesh = target as TextMesh;
 
        EditorGUILayout.LabelField ( "   Text:", "" );
        textMesh.text = EditorGUILayout.TextArea ( textMesh.text, GUILayout.MaxHeight(500f) );

		TextMeshWrapper textMeshWrapper = textMesh.gameObject.GetComponent<TextMeshWrapper>();
		if ( textMeshWrapper != null )
		{
			if ( textMeshWrapper.LineLength > 1 )
			{
				if ( GUILayout.Button ( "Wrap" ) )
				{
					string text = textMesh.text.Wrap ( textMeshWrapper.LineLength, textMeshWrapper.WrappingMode );
					textMesh.text = text;
				}
			}
		}

		// http://twistedoakstudios.com/blog/Post373_unity-font-extraction
    }
}


