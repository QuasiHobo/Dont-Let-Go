using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	public GameObject collectables_1;

	public List<GameObject> spawnPoints = new List<GameObject> ();
	public GameObject obstacle_1;
	public GameObject obstalce_Big_1;
	public GameObject obstacle_Middle_1;
	public GameObject obstacle_Big_Ring_1;

	bool spawning;

	public GameObject midSpawn;
	public GameObject lastSpawn;

	int startCheck = 0;

	// Use this for initialization
	void Start () 
	{
		CharCollison.OnCollision += GameOver;

		spawning = true;
		CharCollison.OnCollision += StopSpawning;

		foreach (Transform child in transform) 
		{
			GameObject obj = child.gameObject;
			spawnPoints.Add (obj);
		}

		StartCoroutine ("Spawner");
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
	}

	void StopSpawning(string charNumb)
	{
		spawning = false;
	}

	IEnumerator Spawner()
	{
		startCheck += 1;
		while (spawning) 
		{
			if (startCheck >= 2) {
				bool spawnCollectables = true;
				float tempTime = GameManager.Instance.spawnTime;
				int tempPoint = Random.Range (0, spawnPoints.Count);
				int spawnAmount = 0;

				while (spawnCollectables) {
					yield return new WaitForSeconds (tempTime / 6);
					Instantiate (collectables_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);
					tempPoint += 1;
					spawnAmount += 1;

					if (tempPoint == spawnPoints.Count)
						tempPoint = 0;
					if (spawnAmount == 5)
						spawnCollectables = false;
				}

				yield return new WaitForSeconds (tempTime / 6);
		}

			int tempIns = Random.Range (0, spawnPoints.Count);	
			int randomNr = Random.Range (0, 12);
				
			if (randomNr == 1 || randomNr == 2 || randomNr == 3) 
				{
				Instantiate (obstalce_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
				}

			else if (randomNr == 4 || randomNr == 5) 
			{
				Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			}

			else if(randomNr == 6)
				Instantiate (obstacle_Big_Ring_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		}
	}

	void GameOver(string lol)
	{
		spawning = false;
	}
}
