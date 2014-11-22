﻿using UnityEngine;
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

	public Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		player = CustomCharacterController.Instance.gameObject;
		reineMere = Traction.Instance.gameObject;
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


			if(distancePlayer < distanceDetection && distancePlayer > distanceMinPlayer){
				goCloserTo(positionPlayer);
				
			}
			if(distanceReineMere < distanceMinMere){
				goAwayFrom(positionReineMere);

			}
		}
	
	}

	private void goCloserTo (Vector3 positionCible)
	{

		updateVelocity(positionCible);

		transform.position += ( velocity * Time.deltaTime );
	}

	private void goAwayFrom (Vector3 positionCible)
	{

		updateVelocity(positionCible);
		transform.position -= ( velocity * Time.deltaTime );
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
}
