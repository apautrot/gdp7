using UnityEngine;
using System.Collections;

public class Traction : MonoBehaviour {

	public GameObject player;
	public float radius = 2f;
	public bool isPushed = false;
	public float magnitude;

	// Use this for initialization
	void Start () {
		isPushed = false;
	
	}


	// Update is called once per frame
	void Update () {
		if (player != null) 
		{
			CustomCharacterController ccc = player.GetComponents<CustomCharacterController>()[0];

			if(isPushed)
			{
				ccc.setPushing();

				Vector3 objectif = player.transform.position;
				Vector3 direction = objectif - transform.position;

				magnitude = direction.magnitude;
				if(direction.magnitude > radius){
				// je suis mon objectif (le perso)

					Vector3 velocity = direction * (ccc.velocity.magnitude/direction.magnitude);
					this.transform.position += velocity * Time.deltaTime;
				}
				else{
					ccc.finishPushing();
				}
			
			}else{
				ccc.finishPushing();
			}
		}
	
	}
}
