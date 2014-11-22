/*
using UnityEngine;
using System;
using System.Collections;


public class EmptyProperty : AbstractTweenProperty, IGenericProperty
{
	public delegate void Callback();

	public string propertyName { get { return "Empty"; } }
	private Callback callback;
	private bool completeImmediatly;

	public EmptyProperty ( Callback callback, bool completeImmediatly )
		: base ( false )
	{
		this.callback = callback;
		this.completeImmediatly = completeImmediatly;
	}

	public override void prepareForUse ()
	{
	}

	public override void tick ( float totalElapsedTime )
	{
		callback();
		if ( completeImmediatly )
		{
			_ownerTween.complete ();
			//_ownerTween.state = GoTweenState.Complete;
		}
	}
}

*/
