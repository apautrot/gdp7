using UnityEngine;
using System.Collections;

public class DragDrop : SceneSingleton<DragDrop>
{
	private Draggable dragSource;
	internal Draggable DragSource
	{
		get { return dragSource; }
		set
		{
			if ( dragSource != null )
				dragSource.collider2D.enabled = true;
			
			dragSource = value;
			
			if ( dragSource != null )
			{
				dragSource.collider2D.enabled = false;
				startPosition = dragSource.transform.position;
				DragSource.OnStartDrag ();
			}
		}
	}

	private DropTarget dropTarget;
	internal DropTarget DropTarget
	{
		get { return dropTarget; }
		set { dropTarget = value; }
	}

	private Vector3 startPosition;

	void FixedUpdate ()
	{
		if ( DragSource != null )
		{
			if ( TouchInput.isDragging )
				DragSource.transform.position = startPosition + TouchInput.dragVector;

			if ( !TouchInput.isDown )
			{
				if ( DropTarget != null )
				{
					if ( DropTarget.IsAccepting ( DragSource ) )
					{
						DropTarget.OnDrop ( DragSource );
						DragSource.OnDrop ( DropTarget );
					}
					else
						DragSource.transform.positionTo ( 0.1f, startPosition );
				}
				else
					DragSource.transform.positionTo ( 0.1f, startPosition );

				DropTarget = null;
				DragSource = null;
			}
		}
	}
}
