using UnityEngine;
using System.Collections;

public class EnergyCircle : SceneSingleton<EnergyCircle>
{
	private bool IsFadingIn
	{ get; set; }

	private bool IsFadingOut
	{ get; set; }

	private System.Action onAction;

	internal float progress;
	public float Progress
	{
		get
		{
			return progress;
		}
		set
		{
			progress = value;

			// back.renderer.material.SetAlpha ( backBaseAlpha * ( 1 - progress ) );

			// front.renderer.material.SetAlpha ( ( 1 - frontBaseAlpha ) + frontBaseAlpha * progress );

			if ( front.renderer.material.HasProperty ( "_CutOff" ) )
				front.renderer.material.SetFloat ( "_CutOff", 1 - value );

			transform.localScale = baseScale * ( 1 + ( 0.5f * progress ) );
		}
	}

	private float backBaseAlpha;
	private float frontBaseAlpha;
	private Vector3 baseScale;

	private GameObject front;
	private GameObject back;

	private GoTween tween;

	private Vector3 offsetToPlayer;

	void Awake ()
	{
		back = gameObject.FindChildByName ( "back" );
		front = gameObject.FindChildByName ( "front" );

		backBaseAlpha = back.renderer.material.color.a;
		frontBaseAlpha = front.renderer.material.color.a;
		baseScale = transform.localScale;

		Progress = 0;

		offsetToPlayer = transform.position - CustomCharacterController.Instance.transform.position;

		FadeIn ();
	}

	private void FixedUpdate ()
	{
		transform.position = CustomCharacterController.Instance.gameObject.transform.position + offsetToPlayer;
	}

	private void FadeIn ()
	{
		transform.localScale = new Vector3 ( 0.9f, 0.9f, 0.9f );
 		back.renderer.material.SetAlpha ( 0 );
 		front.renderer.material.SetAlpha ( 0 );

		const float duration = 2.0f;

		IsFadingIn = true;
		GoTweenFlow flow = new GoTweenFlow ();
		flow.insert ( 0, transform.scaleTo ( duration, baseScale ) );
		flow.insert ( 0, back.renderer.material.alphaTo ( duration, backBaseAlpha ).eases ( GoEaseType.Linear ) );
		flow.insert ( 0, front.renderer.material.alphaTo ( duration, frontBaseAlpha ).eases ( GoEaseType.Linear ) );
		flow.setOnCompleteHandler ( c => IsFadingIn = false );
		Go.addTween ( flow );
	}

	private void FadeOut ()
	{
		IsFadingOut = true;
		GoTweenFlow flow = new GoTweenFlow ();
		flow.insert ( 0, transform.scaleTo ( 0.5f, 4 ) );
		flow.insert ( 0, back.renderer.material.alphaTo ( 0.5f, 0 ) );
		flow.insert ( 0, front.renderer.material.alphaTo ( 0.5f, 0 ) );
		flow.setOnCompleteHandler ( c => {
			gameObject.DestroySelf ();
		} );
		Go.addTween ( flow );
	}

	void OnMouseDown ()
	{
		if ( IsFadingIn )
			return;

		if ( IsFadingOut )
			return;

 		if ( tween != null )
 			tween.destroy ();

		tween = this.floatTo ( "Progress", 1, 1 ).eases ( GoEaseType.Linear );
	}

	void OnMouseUp ()
	{
		if ( IsFadingIn )
			return;

		if ( IsFadingOut )
			return;

		if ( tween != null )
			tween.destroy ();

		tween = this.floatTo ( "Progress", 0.5f, 0 ).eases ( GoEaseType.BounceOut );
	}

	internal void SetAction ( System.Action action )
	{
		this.onAction = action;
	}
}
