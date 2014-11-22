using UnityEngine;
using System.Collections;

public class ValueBonus : MonoBehaviour {

	internal GameObject reineMere;
	public float baseValue = 100f;
	public float distanceFactor = 5f;
	public float distanceMin = 10f;
	public float value;

	public GameObject glowLight;

	// Use this for initialization
	void Start () {
		reineMere = Traction.Instance.gameObject;
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

		if (glowLight != null) {
			Light l = glowLight.GetComponent<Light>();
			l.intensity = value /2f;
		}

	}


	public float consumeEnergy ()
	{
		if (value > 0) {
			GameObject.Destroy (this.gameObject);
		}
		return value;
	}
}
