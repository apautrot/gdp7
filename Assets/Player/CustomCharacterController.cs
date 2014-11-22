using UnityEngine;
using System.Collections;

public class CustomCharacterController : MonoBehaviour {

	public float topSpeed = 50f;
	public float pushingTopSpeed = 2f;
	public float accelerationFactor = 10;
	public float frictionFactor = 0.25f;
	public Vector3 velocity = Vector3.zero;
	public Vector3 Axis = Vector3.zero;
	public bool isPushing = false;
	public GameObject reineMere;

	public Vector3 cameraOffset = Vector3.zero;
	public GameObject weapon;
	
	private bool keyAttack = false;
	private bool isAttacking = false;

	// Use this for initialization
	void Start ()
	{
		cameraOffset = gameObject.transform.position - Camera.main.transform.position;
		this.weapon.SetActive (false);
	}
	
	void Update()
	{
		if ( ! isAttacking )
		if (Input.GetKeyDown (KeyCode.LeftControl) || Input.GetKey (KeyCode.Joystick1Button2)) {
			keyAttack = true;
		}
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

		if (isPushing) {
			if (velocity.magnitude > pushingTopSpeed)
				velocity = velocity.normalized * pushingTopSpeed;
		} else {
			if (velocity.magnitude > topSpeed)
				velocity = velocity.normalized * topSpeed;
		}

		transform.position += ( velocity * Time.fixedDeltaTime );
		
		//Move the camera according the player position
		float cameraPosX = gameObject.transform.position.x - cameraOffset.x;
		float cameraPosZ = gameObject.transform.position.z - cameraOffset.z;
		
		Camera.main.transform.position = new Vector3(cameraPosX, Camera.main.transform.position.y ,cameraPosZ);
		
		gameObject.transform.LookAt( gameObject.transform.position + velocity, Vector3.up);

		// ------------ Push the queen while pressing a button
		if (reineMere != null) {
						Traction trac = reineMere.GetComponents<Traction> () [0];
						trac.isPushed = true;
						if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.Joystick1Button0)) {

				}
			else{
				trac.isPushed = false;
			}
		}
		if(keyAttack) {
			attack();
			keyAttack = false;
		}
				/*		setPushing ();
				} else {
						finishPushing ();
				}*/
	}

	/**
	 * Returns whether the player is pushing or not
	 */
	public bool getPushing(){
				return isPushing;
		}

	public void setPushing(){
		if (!isPushing) {
						isPushing = true;
				}
	}

	public void finishPushing(){
		if (isPushing) {
						isPushing = false;
				}
	}
	
	public void attack(){
		isAttacking = true;
		this.weapon.SetActive (true);
		this.weapon.transform.localEularAnglesTo ( 0.5f, new Vector3 ( 0, 180, 0 ) ).setOnCompleteHandler ( c => {
			this.weapon.SetActive ( false );
			this.weapon.transform.localEulerAngles = Vector3.zero;
			isAttacking = false;
		});
	}

}
