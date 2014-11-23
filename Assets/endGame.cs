using UnityEngine;
using System.Collections;

public class endGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.tag == "Player") {
			Application.LoadLevel("End Game");
		}
	}
}
