using UnityEngine;
using System.Collections;

public class DropTarget : MonoBehaviour
{
	internal void OnMouseEnter ()
	{
		DragDrop.Instance.DropTarget = this;
	}

	internal void OnMouseExit ()
	{
		if ( DragDrop.Instance.DropTarget == this )
			DragDrop.Instance.DropTarget = null;
	}

	internal virtual bool IsAccepting ( Draggable dropped )
	{
		return true;
	}

	internal virtual void OnDrop ( Draggable dropped )
	{
	}
}
