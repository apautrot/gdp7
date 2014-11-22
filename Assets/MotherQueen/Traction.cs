using UnityEngine;
using System.Collections;

public class Traction : SceneSingleton<Traction> {

	public GameObject player;
	public float radius = 2f;
	public bool isPushed = false;
	public float distance;
	public float dragDistance = 2.5F;
	public float consumRate = 0.02f;
	// Use this for initialization
	void Start () {
		isPushed = false;
	
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (player != null) 
		{
			CustomCharacterController ccc = player.GetComponents<CustomCharacterController>()[0];

			if(isPushed)
			{
				Vector3 playerDistance = ccc.transform.position - gameObject.transform.position;
				//If the player is not too far from Queen
				if(playerDistance.magnitude <= dragDistance && ccc.getEnergie() > 0f) {
					ccc.setPushing();

					Vector3 objectif = player.transform.position;
					Vector3 direction = objectif - transform.position;

					distance = direction.magnitude;
					if(distance > radius){
					// je suis mon objectif (le perso)

						Vector3 velocity = direction * (ccc.velocity.magnitude/distance)*1.125f;
						//apply displacement
						this.transform.position += velocity * Time.fixedDeltaTime;

						//consume player energie
						ccc.consumeEnergie(velocity.magnitude * consumRate);


						Vector3 pos = ccc.transform.position + velocity;
						pos.y = gameObject.transform.position.y;
						this.transform.LookAt( pos, Vector3.up);
					}

				} else {
					// The player is too far from the queen
					ccc.finishPushing();
				}
			}else{
				ccc.finishPushing();
			}
		}
	
	}
	
}
