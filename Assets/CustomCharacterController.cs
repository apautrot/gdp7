using UnityEngine;
using System.Collections;

public class CustomCharacterController : MonoBehaviour {

	public float topSpeed = 50f;
	public float accelerationFactor = 10;
	public float frictionFactor = 0.25f;
	public Vector3 velocity = Vector3.zero;
	public Vector3 Axis = Vector3.zero;

	public Vector3 cameraOffset = Vector3.zero;


	// Use this for initialization
	void Start ()
	{
		cameraOffset = gameObject.transform.position - Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector3 acceleration = Vector3.zero;
		acceleration.x = Input.GetAxis ("Horizontal") * accelerationFactor;
		acceleration.z = Input.GetAxis ("Vertical") * accelerationFactor;

		float accelerationMagnitude = acceleration.magnitude;
		if (Mathf.Abs ( accelerationMagnitude ) > accelerationFactor)
			acceleration = acceleration.normalized * accelerationFactor;

		Axis.x = Input.GetAxis ("Horizontal");
		Axis.z = Input.GetAxis ("Vertical");

		velocity += acceleration;

		velocity *= frictionFactor;

		if (velocity.magnitude > topSpeed)
		{
			velocity = velocity.normalized * topSpeed;
		}

		transform.position += ( velocity * Time.fixedDeltaTime );

		//Move the camera according the player position
		float cameraPosX = gameObject.transform.position.x - cameraOffset.x;
		float cameraPosZ = gameObject.transform.position.z - cameraOffset.z;

		Camera.main.transform.position = new Vector3(cameraPosX, Camera.main.transform.position.y ,cameraPosZ);
		
		gameObject.transform.LookAt( gameObject.transform.position + velocity, Vector3.up);
	}
}
