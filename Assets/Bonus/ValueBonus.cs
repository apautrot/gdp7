using UnityEngine;
using System.Collections;

public class ValueBonus : MonoBehaviour {

	internal GameObject reineMere;
	public float baseValue = 100f;
	public float distanceFactor = 5f;
	public float distanceMin = 10f;
	public float value;

	private float initialScale;

	public GameObject glowLight;

	internal bool isFadingOut;

	// Use this for initialization
	void Start () {
		reineMere = Traction.Instance.gameObject;
		initialScale = transform.lossyScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (reineMere != null) {
			float distanceQueen = (reineMere.transform.position - this.transform.position).magnitude;

			if(distanceQueen < (baseValue * distanceFactor)){
				value = baseValue / ((distanceFactor+baseValue) / distanceQueen) - distanceMin;
			}
			if(value < 0){
				value = 0;
			}
		}

		updateRender ();

	}

	private void updateRender(){
		if (glowLight != null) {
			Light l = glowLight.GetComponent<Light>();
			l.intensity = value /2f;
		}
		transform.SetScale ( Mathf.Min ( initialScale, initialScale * value ) );
		//too close, if no light(== no value) => no mesh render
		// this.gameObject.GetComponent<MeshRenderer>().enabled = (value != 0);
		//no render => no colisions
		// this.gameObject.GetComponent<BoxCollider> ().enabled = (value != 0);
		// this.gameObject.GetComponent<ParticleSystem> ().enabled = (value != 0);
		// 
	}

	public float consumeEnergy ()
	{
		if ( value > 0 )
		{
			isFadingOut = true;

			this.transform.scaleTo ( 1.0f, 0 );
			this.transform.positionTo ( 1.0f, new Vector3 ( 0, 2.0f, 0 ), true ).setOnCompleteHandler ( c => {
				this.gameObject.DestroySelf ();
			} );
		}
		return value;
	}
}
