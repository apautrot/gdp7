using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	internal GameObject player;
	internal GameObject reineMere;

	public float distanceDetection = 12f;
	public float distanceMinMere = 5f;
	public float distanceMinPlayer = 1.5f;

	public float topSpeed =3.25f;
	public float accelerationFactor = 10f;
	public float frictionFactor = 0.2f;

	public float life = 10.0f;

	public float stunDuration = 10f;
	public float avoidDuration = 5f;
	public float timeStun;
	public float timeAvoid;

	public Vector3 velocity = Vector3.zero;

	public float timeBeforeAttack;
	public float timeMinAttack = 2f;
	public float timeMaxAttack = 5f;
	public bool isAttacking = false;
	public float attackDistance = 1f;
	public float attackRadius = 0.5f;
	public float attackIntensity = 2f;

	// Sound
	internal Sounds soundManager;

	// Use this for initialization
	void Start () {
		player = CustomCharacterController.Instance.gameObject;
		reineMere = Traction.Instance.gameObject;
		timeBeforeAttack = rand (timeMinAttack, timeMaxAttack);
		soundManager = Sounds.Instance;

	}
	
	// Update is called once per frame
	void Update () {
		if (player != null && reineMere != null) {	

			Vector3 positionPlayer = player.transform.position;
			Vector3 directionPlayer = positionPlayer - this.transform.position;
			float distancePlayer = directionPlayer.magnitude;

			Vector3 positionReineMere = reineMere.transform.position;
			Vector3 directionReineMere = positionReineMere - this.transform.position;
			float distanceReineMere = directionReineMere.magnitude;

			if(timeStun > 0 || timeAvoid > 0){
				//the enemy was hit and he is confused
				if(timeAvoid > 0){ // == (stunDuration + avoidDuration) - avoidDuration
					//first go away from the player
					goAwayFrom(positionPlayer);
					timeAvoid -= Time.deltaTime;
					if(timeAvoid < 0)
						timeAvoid = 0;
				}
				else{ //just idle
					timeStun -= Time.deltaTime;
					if(timeStun < 0)
						timeStun = 0;
				}
			}
			else{

				if(distancePlayer < distanceDetection && distancePlayer > distanceMinPlayer){
					//se deplace vers le joueur
					goCloserTo(positionPlayer);
				
				}
				if(distancePlayer <= distanceMinPlayer){
					// Debug.Log ( "J'attaque le joueur " + Time.frameCount );
					//attaque le joueur
					attaquePlayer();
				}
			
				if(distanceReineMere < distanceMinMere){
					goAwayFrom(positionReineMere);
				}
			}
		}
	
	}

	private void goCloserTo (Vector3 positionCible)
	{

		updateVelocity(positionCible);

		transform.position += ( velocity * Time.deltaTime );
		/*
		Vector3 pos = positionCible;
		pos.y = gameObject.transform.position.y;
		this.transform.LookAt( pos, Vector3.up);*/
	}
	
	private void goAwayFrom (Vector3 positionCible)
	{

		updateVelocity(positionCible);
		transform.position -= ( velocity * Time.deltaTime );
		/*
		Vector3 pos = positionCible;
		pos.y = gameObject.transform.position.y;
		this.transform.LookAt( pos, Vector3.up);*/
	}

	private void updateVelocity(Vector3 positionCible){
		// 
		Vector3 directionCible = positionCible - this.transform.position;
		float distanceCible = directionCible.magnitude;
		
		float accelerationMagnitude = distanceCible;
		Vector3 acceleration = directionCible * accelerationFactor;
		
		if (accelerationMagnitude > accelerationFactor)
			acceleration = acceleration.normalized * accelerationFactor;
		
		velocity += acceleration;
		
		velocity *= frictionFactor;
		
		
		if (velocity.magnitude > topSpeed)
			velocity = velocity.normalized * topSpeed;

		velocity.y = 0;
	}

	void attaquePlayer ()
	{
		//do the attack
		if (timeBeforeAttack > 0) { 
			//wait
			timeBeforeAttack -= Time.deltaTime;
		}
		if (timeBeforeAttack <= 0) {

			//do the attack
			isAttacking = true;

			Vector3 directionPlayer = (player.transform.position - this.transform.position);
			directionPlayer.y = 0;
			directionPlayer.Normalize();
			directionPlayer = directionPlayer.normalized;

			Vector3 attackPosition = new Vector3 ( attackDistance*directionPlayer.x, 0, attackDistance *directionPlayer.z );
			Vector3 absolutedAttackPosition = transform.TransformPoint ( attackPosition );
			Collider[] colliders = Physics.OverlapSphere ( absolutedAttackPosition, attackRadius );
			// Debug.DrawLine (absolutedAttackPosition, transform.position);

			for(int i = 0; i < colliders.Length ;i++){
				Collider collider = colliders[i];
					if ( collider.tag == "Player" )
					{
						Vector3 collider2DPosition = collider.transform.position; collider2DPosition.y = 0;
						Vector3 attack2DPosition = absolutedAttackPosition; attack2DPosition.y = 0;
						float distance = ( collider2DPosition - attack2DPosition ).magnitude;
						float maxDistance = attackRadius + ( collider as CapsuleCollider ).radius;
						float hitFactor = 1 - ( distance / maxDistance );
						collider.gameObject.GetComponent<CustomCharacterController>().OnHitByEnemie ( hitFactor * attackIntensity, hitFactor );
					break;
					}
				}
			isAttacking = false;

			//new time before next attack
			timeBeforeAttack = rand (timeMinAttack, timeMaxAttack);
		}
	}

	internal void OnHitByPlayer ( float value, float hitFactor )
	{
		soundManager.PlaySoundAt(soundManager.playerAttackHit,Sounds.soundMode.Standard,this.transform.position,false,hitFactor*2f,0f,true);
		Debug.Log ( "Enemy " + name + " hitted by " +  value );
		life -= value;
		updateEnemyState ();
		timeStun = stunDuration * hitFactor;
		timeAvoid = avoidDuration * hitFactor;


	}

	internal void updateEnemyState()
	{
		if (life <= 0) 
		{
			soundManager.PlaySoundAt(soundManager.enemyExplosion,Sounds.soundMode.Standard,this.transform.position,false,2f,0f,true);
			this.gameObject.DestroySelf();
		}
	}

	internal float rand(float binf, float bsupp){
		return Random.value * (bsupp - binf) + binf;
	}

}
