using UnityEngine;
using System.Collections;

public class spawnerEnemy : MonoBehaviour {

	public float delay =5f;
	public float timeDelay;
	public GameObject enemyPrefab;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeDelay -= Time.deltaTime;
		if (timeDelay <= 0) {
			GameObject enemy = (GameObject)GameObject.Instantiate (enemyPrefab);
			enemy.transform.position = transform.position;

			timeDelay = delay - timeDelay;
		}
	}
}
