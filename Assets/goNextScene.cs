using UnityEngine;
using System.Collections;

public class goNextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ( KeyCode.Joystick1Button7) || Input.GetKey ( KeyCode.Space)) {
			Application.LoadLevel("Level 1");
				}
	}
}
