using UnityEngine;
using System.Collections;

public class CustomCharacterController : SceneSingleton<CustomCharacterController> {

	public float topSpeed = 50f;
	public float pushingTopSpeed = 2f;
	public float accelerationFactor = 10;
	public float frictionFactor = 0.25f;
	public Vector3 velocity = Vector3.zero;
	public Vector3 Axis = Vector3.zero;
	public bool isPushing = false;
	internal GameObject reineMere;

	public Vector3 cameraOffset = Vector3.zero;
	public Vector3 lblEnergyOffset = Vector3.zero;
	public GameObject weapon;
	public GameObject lblPlayerEnergy;

	public float attackDistance = 1;
	public float attackRadius = 1;
	public float attackIntensity = 5;

	private bool keyAttack = false;
	private bool isAttacking = false;

	public float energie;

	// Sound
	internal Sounds soundManager;
	internal AudioSource source;

	// Use this for initialization
	void Start ()
	{
		cameraOffset = gameObject.transform.position - Camera.main.transform.position;
		lblEnergyOffset = gameObject.transform.position - lblPlayerEnergy.transform.position;

		reineMere = Traction.Instance.gameObject;
		soundManager = Sounds.Instance;
		source = new AudioSource ();
		this.weapon.SetActive (false);
		hideLblEnergy ();
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

		//Move the lblPlayerEnergy according the player position
		float lblEnergyPosX = gameObject.transform.position.x - lblEnergyOffset.x;
		float lblEnergyPosZ = gameObject.transform.position.z - lblEnergyOffset.z;
		
		Camera.main.transform.position = new Vector3(cameraPosX, Camera.main.transform.position.y ,cameraPosZ);

		lblPlayerEnergy.transform.position = new Vector3 (lblEnergyPosX, lblPlayerEnergy.transform.position.y, lblEnergyPosZ);
		
		gameObject.transform.LookAt( gameObject.transform.position + velocity, Vector3.up);

		// ------------ Push the queen while pressing a button
		if (reineMere != null) {
						Traction trac = reineMere.GetComponent<Traction> ();
						trac.isPushed = true;
						if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.Joystick1Button0)) {
				showLblEnergy ();
				updateLblEnergy (Mathf.Floor(energie).ToString());
				}
			else{
				trac.isPushed = false;
				hideLblEnergy();
			}
		}
		if(keyAttack) {
			attack();
			keyAttack = false;
		}

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
		//source.audio.clip = soundManager.playerAttack;
		//source.PlayScheduled (10f);
		this.weapon.SetActive (true);
		this.weapon.transform.localEularAnglesTo ( 0.5f, new Vector3 ( 0, 180, 0 ) ).setOnCompleteHandler ( c => {
			this.weapon.SetActive ( false );
			this.weapon.transform.localEulerAngles = Vector3.zero;
			isAttacking = false;
		});

		Vector3 attackPosition = new Vector3 ( 0, 0, attackDistance );
		Vector3 absolutedAttackPosition = transform.TransformPoint ( attackPosition );
		Collider[] colliders = Physics.OverlapSphere ( absolutedAttackPosition, attackRadius );
		// Debug.DrawLine (absolutedAttackPosition, transform.position);

		foreach ( Collider collider in colliders )
		{
			if ( collider.tag == "Enemy" )
			{
				Vector3 collider2DPosition = collider.transform.position; collider2DPosition.y = 0;
				Vector3 attack2DPosition = absolutedAttackPosition; attack2DPosition.y = 0;
				float distance = ( collider2DPosition - attack2DPosition ).magnitude;
				float maxDistance = attackRadius + ( collider as CapsuleCollider ).radius;
				float hitFactor = 1 - ( distance / maxDistance );
				collider.gameObject.GetComponent<EnemyAI>().OnHitByPlayer ( hitFactor * attackIntensity, hitFactor );
			}
		}
	}

	public void consumeEnergie(float amount){
		this.energie -= amount;
		if (this.energie < 0)
			this.energie = 0;
	}

	public float getEnergie(){
		return this.energie;
	}

	private void OnTriggerEnter(Collider col){
		//Debug.Log("collide with : " + col.name);
		if (col.tag == "Energy") {
			if(energie < 100f){
				this.energie += col.gameObject.GetComponent<ValueBonus>().consumeEnergy();
			}
		}
	}
		
	public void showLblEnergy(){
		this.lblPlayerEnergy.renderer.enabled = true;
	}
	
	public void hideLblEnergy(){
		this.lblPlayerEnergy.renderer.enabled = false;
	}

	public void updateLblEnergy(string value) {
		this.lblPlayerEnergy.GetComponent<TextMesh>().text = value;
	}
}
