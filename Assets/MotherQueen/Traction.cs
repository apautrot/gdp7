using UnityEngine;
using System.Collections;

public class Traction : SceneSingleton<Traction> {

	public GameObject player;
	public float radius = 2f;
	public bool isPushed = false;
	public float distance;
	public float dragDistance = 2.5F;
	public float consumRate = 0.02f;
	internal bool soundLockPlayed = false;
	internal bool soundUnLockPlayed = false;
	// Use this for initialization
	void Start () {
		player = CustomCharacterController.Instance.gameObject;

		isPushed = false;
	
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (player != null) 
		{
			CustomCharacterController ccc = player.GetComponents<CustomCharacterController>()[0];

			if(this.isPushed)
			{
				Vector3 playerDistance = ccc.transform.position - gameObject.transform.position;
				//If the player is not too far from Queen
				if(playerDistance.magnitude <= dragDistance && ccc.Energy > 0f) {
					playSoundLock();

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
					playSoundUnLock();
				}
			}else{
				ccc.finishPushing();
			}
		}
	}

	void playSoundLock() {
		if(!soundLockPlayed){
			Sounds.Instance.PlaySound(Sounds.Instance.motherActivation, Sounds.soundMode.Standard, false);
			soundLockPlayed = true;
			soundUnLockPlayed = false;
		}
	}
	
	void playSoundUnLock() {
		if(!soundUnLockPlayed){
			Sounds.Instance.PlaySound(Sounds.Instance.motherDesactivation, Sounds.soundMode.Standard, false);
			soundUnLockPlayed = true;
			soundLockPlayed = false;
		}
	}
}
