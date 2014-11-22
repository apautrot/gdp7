using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

public class MessageSender : MonoBehaviour
{
	public GameObject target;
	public string messageName;
	public string paramString;
	public GameObject paramObject;
	public bool paramSelf;
	public bool broadcastToChilds;

	protected void SendMessage ()
	{
		if ( target != null )
		{
			if ( broadcastToChilds )
			{
				if ( paramSelf )					target.BroadcastMessage ( messageName, gameObject );
				else if ( paramObject != null )		target.BroadcastMessage ( messageName, paramObject );
				else								target.BroadcastMessage ( messageName, paramString );
			}
			else
			{
				if ( paramSelf )					target.SendMessage ( messageName, gameObject );
				else if ( paramObject != null )		target.SendMessage ( messageName, paramObject );
				else								target.SendMessage ( messageName, paramString );
			}
		}
		else
		{
			GameObjects.BroadcastMessageToScene ( messageName, paramString );
		}
	}
}

public class OnClick : MessageSender
{
	public bool scaleOnClick = true;
	public float scaleFactor = 1.1f;

	private Vector3 originalScale;
	private List<GoTween> tweeners;

	void OnMouseDown ()
	{
		if ( scaleOnClick )
		{
			originalScale = transform.localScale;
			transform.localScale = new Vector3 ( originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor );
			tweeners = Go.tweensWithTarget ( transform, true );
			foreach ( GoTween t in tweeners )
				t.pause ();
		}
	}

	void OnMouseUp ()
	{
		if ( scaleOnClick )
		{
			transform.localScale = originalScale;
			foreach ( GoTween t in tweeners )
				t.play ();
		}

		if ( gameObject.IsUnderMouse () )
		{
			OnClicked ();
		}
	}

	protected virtual void OnClicked ()
	{
		SendMessage ();
	}
	
}