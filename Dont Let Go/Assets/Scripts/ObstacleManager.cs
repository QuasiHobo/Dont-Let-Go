using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	public GameObject collectables_1;

	public List<GameObject> spawnPoints = new List<GameObject> ();
	public GameObject obstacle_1;
	public GameObject obstalce_Big_Rot_1;
	public GameObject obstalce_Big_Rot_2;
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
		while (spawning) 
		{
			startCheck += 1;
			int spawnDirection = Random.Range (0, 2);
			int spawnCheck = Random.Range (0, 4);
			if (startCheck >= 2 && spawnCheck == 1) 
			{
				bool spawnCollectables = true;
				float tempTime = GameManager.Instance.spawnTime;
				int tempPoint = Random.Range (0, spawnPoints.Count);
				int spawnAmount = 0;
				int maxSpawnAmount = Random.Range (6, 12);

				int chanceForBonusLevel = Random.Range (0, GameManager.Instance.bonusLevelChance);
				if (chanceForBonusLevel == 1)
					maxSpawnAmount = GameManager.Instance.bonusLevelSpawnAmount;

				yield return new WaitForSeconds (tempTime / 2f);
				while (spawnCollectables) {

					yield return new WaitForSeconds (tempTime / 7.5f);
					Instantiate (collectables_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);

					if(spawnDirection == 0)
						tempPoint += 1;
					if (spawnDirection == 1)
						tempPoint -= 1;
					
					spawnAmount += 1;

					if (tempPoint == spawnPoints.Count && spawnDirection == 0)
						tempPoint = 0;
					if (tempPoint <= 0 && spawnDirection == 1)
						tempPoint = spawnPoints.Count -1;
					
					if (spawnAmount == maxSpawnAmount)
						spawnCollectables = false;
				}

				yield return new WaitForSeconds (tempTime / 2f);
			}

				else if(startCheck == 1)
				yield return new WaitForSeconds (GameManager.Instance.spawnTime);
			if (spawnCheck != 1)
				yield return new WaitForSeconds (GameManager.Instance.spawnTime);

			int tempIns = Random.Range (0, spawnPoints.Count);	
			int randomNr = Random.Range (0, 12);
				
			if (randomNr == 1 || randomNr == 2 || randomNr == 3 || randomNr == 4 || randomNr == 5) 
				{
				if(spawnDirection == 0)
					Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
				if(spawnDirection == 1)
					Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
				}

//			else if (randomNr == 4 || randomNr == 5) 
//			{
//				Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
//			}

			else if(randomNr == 6)
				Instantiate (obstacle_Big_Ring_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
			yield return null;
		}
	}

	void GameOver(string lol)
	{
		spawning = false;
	}
}
