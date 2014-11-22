using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DebugWindow : SceneSingleton<DebugWindow>
{
    private Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
    private bool isVisible = true;

    public Font Font = null;
	public int LineHeight = 22;

	public void AddEntry ( string group, string key, int value )
	{
		AddEntry ( group, key, value.ToString () );
	}

	public void AddEntry ( string group, string key, float value )
	{
		AddEntry ( group, key, value.ToString () );
	}

	public void AddEntry ( string group, string key, Vector3 value )
	{
		AddEntry ( group, key, value.ToString () );
	}

	public void AddEntry(string group, string key, string value)
    {
        if ( ! values.ContainsKey ( group ) )
        {
            values[group] = new Dictionary<string, string>();
        }
        values[group][key] = value;
    }

	public void RemoveGroup ( string group )
	{
		values.Remove ( group );
	}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1) == true)
        {
            isVisible = !isVisible;
        }
    }

	private void OnGUI ()
	{
		if ( Font != null )
			GUI.skin.font = Font;

		GUI.color = new Color ( 1, 1, 1, 1 );

		if ( ! isVisible )
			GUI.Label ( new Rect ( 10, 5, 300, LineHeight ), "press F1 to debug" );
		else
		{
			int y = 5;
			int halfHeight = LineHeight / 2;

			foreach ( KeyValuePair<string, Dictionary<string, string>> entry in values )
			{
				GUI.Label ( new Rect ( 10, y, 300, LineHeight ), "" + entry.Key + "" );
				foreach ( KeyValuePair<string, string> key_val in entry.Value )
				{
					y += halfHeight;
					GUI.Label ( new Rect ( 10, y, 300, LineHeight ), " " + key_val.Key + ": " + key_val.Value );
				}
				y += halfHeight;
			}
		}
	}
}
