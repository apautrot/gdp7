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
	public GameObject playerSpawn;

	public float attackDistance = 1;
	public float attackRadius = 1;
	public float attackIntensity = 5;

	private bool keyAttack = false;
	private bool isAttacking = false;

	public float MaxEnergie = 100f;
	public float InitialEnergie = 20.0f;

	private float energy;
	internal float Energy
	{
		get { return energy; }
		set
		{
			energy = Mathf.Max ( 0, Mathf.Min ( MaxEnergie, value ) );
			EnergyCircle.Instance.GetComponent<EnergyCircle> ().Progress = energy / MaxEnergie;
			if ( DebugWindow.InstanceCreated )
				DebugWindow.Instance.AddEntry ( "Player", "Energy", energy );
		}
	}

	public float health = 100f;

	// Sound
	internal Sounds soundManager;

	public GameObject Geometry;

	// Use this for initialization
	void Start ()
	{
		cameraOffset = gameObject.transform.position - Camera.main.transform.position;

		reineMere = Traction.Instance.gameObject;
		soundManager = Sounds.Instance;
		this.weapon.SetActive (false);

		Energy = InitialEnergie;
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

		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
				} else {
			soundManager.PlaySound(soundManager.playerMove, Sounds.soundMode.Standard, true);
				}

		// ------------ Push the queen while pressing a button
		if ( reineMere != null )
		{
			Traction trac = reineMere.GetComponent<Traction> ();
			trac.isPushed = false;
			if ( Input.GetKey ( KeyCode.Space ) || Input.GetKey ( KeyCode.Joystick1Button0 ) )
			{
				trac.isPushed = true;
			}
			else
			{
				trac.isPushed = false;
			}
		}
		if ( keyAttack )
		{
			attack ();
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
		Sounds.Instance.PlaySound (soundManager.playerAttack, Sounds.soundMode.Standard);
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

	public bool isAlive(){
		return energy > 0;
	}

	public void OnHitByEnemie (float value, float hitFactor)
	{
		soundManager.PlaySoundAt(soundManager.enemyAttackHit,Sounds.soundMode.Standard,this.transform.position,false,hitFactor*2.5f,0f,true);
		consumeEnergie(value);
		updatePlayerState ();
	}

	void updatePlayerState ()
	{
		if (!isAlive ()) {
			Debug.Log("Vous avez perdu !");
		}
	}
	
	public void consumeEnergie(float amount)
	{
		Energy -= amount;
		if (Energy <= 0) {
			this.playerDie();
		}
	}

	private void OnTriggerEnter(Collider col){
		//Debug.Log("collide with : " + col.name);
		if (col.tag == "Energy")
		{
			if(energy  < MaxEnergie) {
				Energy += col.gameObject.GetComponent<ValueBonus> ().consumeEnergy ();
				soundManager.PlaySound(soundManager.bonusGrabbed, Sounds.soundMode.Standard);
			}
		}
	}

	public void playerDie() {
		soundManager.PlaySound (soundManager.playerExplosion, Sounds.soundMode.Standard);
		this.Geometry.SetActive (false);
		gameObject.transform.positionTo (0.8f, this.playerSpawn.transform.position, false).setOnCompleteHandler (c => {
			this.Geometry.SetActive (true);
		});
		this.energy = 100f;
	}
}
