using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("COLLISION EVENT ENTER : " + other.name);

	}
	
	void OnTriggerExit(Collider other) {
		Debug.Log ("COLLISION EVENT EXIT : " + other.name);
		
	}
}
