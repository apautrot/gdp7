using UnityEngine;
using System.Collections;

public class spawnerEnemy : MonoBehaviour {

	public float delay =5f;
	public float timeDelay;
	public float distanceMin = 50f;
	public GameObject enemyPrefab;

	internal GameObject player;


	// Use this for initialization
	void Start () {
		player = CustomCharacterController.Instance.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float distancePlayer = (player.transform.position - this.transform.position).magnitude;
		if (distancePlayer <= distanceMin) {
			timeDelay -= Time.deltaTime;
			if (timeDelay <= 0) {
				GameObject enemy = (GameObject)GameObject.Instantiate (enemyPrefab);
				enemy.transform.position = transform.position;
				timeDelay = delay - timeDelay;
			}
		}
	}
}
