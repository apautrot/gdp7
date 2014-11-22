using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour
{
	internal void OnMouseDown ()
	{
		if ( IsDraggable() )
			DragDrop.Instance.DragSource = this;
	}

	internal void OnDestroy ()
	{
		if ( DragDrop.InstanceCreated )
			if ( DragDrop.Instance.DragSource == this )
				DragDrop.Instance.DragSource = null;
	}

	internal virtual bool IsDraggable ()
	{
		return true;
	}

	internal virtual void OnStartDrag ()
	{
	}

	internal virtual void OnDrop ( DropTarget dropTarget )
	{
	}
}
