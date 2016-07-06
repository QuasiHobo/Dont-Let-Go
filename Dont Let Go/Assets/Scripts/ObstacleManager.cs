using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstacleManager : MonoBehaviour {

	// Singleton
	private static ObstacleManager instance;
	// Construct
	private ObstacleManager ()
	{
	}
	// Instance
	public static ObstacleManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(ObstacleManager)) as ObstacleManager;

			return instance;
		}
	}

	GameObject collectables_1;
	GameObject collectable_Boost;

	public List<GameObject> spawnPoints = new List<GameObject> ();
	public GameObject obstacle_1;
	public GameObject obstalce_Big_Rot_1;
	public GameObject obstalce_Big_Rot_2;
	public GameObject obstacle_Middle_1;
	public GameObject obstacle_Big_Ring_1;
	public GameObject obstacle_Ring_Tunnel_1;
	public GameObject obstacle_Big_2;
	public GameObject obstacle_PikeBall_1;
	public GameObject obstacle_PikeBall_2;
	public GameObject obstacle_SinglePikeBall_1;
	public GameObject obstacle_VanillaRing_1;
	public GameObject obstacle_Big_Rot_Fast_1;
	public GameObject obstacle_Big_Rot_Fast_2;
	public GameObject obstacle_Big_2_Rot_1;
	public GameObject obstacle_Big_2_Rot_2;

	bool spawning;
	public bool doingBoost = false;

	public GameObject midSpawn;

	//For checking fillamount
	public Image hugBar;

	int startCheck = 0;

	// Use this for initialization
	void Start () 
	{
		//Load in Obstacle Objects
		obstacle_1 = Resources.Load("Prefabs/Obstacles/Obstacle_1") as GameObject;
		obstalce_Big_Rot_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_1") as GameObject;
		obstalce_Big_Rot_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_2") as GameObject;
		obstacle_Middle_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Middle_1") as GameObject;
		obstacle_Big_Ring_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Ring_1") as GameObject;
		obstacle_Ring_Tunnel_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Ring_Tunnel_1") as GameObject;
		obstacle_Big_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2") as GameObject;
		obstacle_PikeBall_1 = Resources.Load("Prefabs/Obstacles/Obstacle_PikeBall_1") as GameObject;
		obstacle_PikeBall_2 = Resources.Load("Prefabs/Obstacles/Obstacle_PikeBall_2") as GameObject;
		obstacle_SinglePikeBall_1 = Resources.Load("Prefabs/Obstacles/Obstacle_SinglePikeBall_1") as GameObject;
		obstacle_VanillaRing_1 = Resources.Load("Prefabs/Obstacles/Obstacle_VanillaRing_1") as GameObject;
		obstacle_Big_Rot_Fast_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_fast_1") as GameObject;
		obstacle_Big_Rot_Fast_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_fast_2") as GameObject;
		obstacle_Big_2_Rot_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2_Rot_1") as GameObject;
		obstacle_Big_2_Rot_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2_Rot_2") as GameObject;

		//Load collectable Objects
		collectables_1 = Resources.Load("Prefabs/Collectables/Collectable_2") as GameObject;
		collectable_Boost = Resources.Load("Prefabs/Collectables/Collectable_Boost_1") as GameObject;

		CharCollison.OnCollision += GameOver;
		CharCollison.OnCollision += StopSpawning;
		spawning = false;

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
		CharCollison.OnCollision -= StopSpawning;
	}

	void StopSpawning(string charNumb)
	{
		spawning = false;
	}

	IEnumerator Spawner()
	{
		spawning = true;

		while (spawning && GameManager.Instance.gameOver == false) 
		{
			startCheck += 1;
			int spawnDirection = Random.Range (0, 2);
			int spawnCheck = Random.Range (0, 4);

			if (startCheck >= 2 && spawnCheck == 1) 
			{
				bool spawnCollectables = true;
				bool boostSpawned = false;
				float tempTime = GameManager.Instance.spawnTime;
				int tempPoint = Random.Range (0, spawnPoints.Count);
				int spawnAmount = 0;
				int maxSpawnAmount = Random.Range (6, 16);

				int chanceForBonusLevel = Random.Range (0, GameManager.Instance.bonusLevelChance);
				if (chanceForBonusLevel == 1)
					maxSpawnAmount = GameManager.Instance.bonusLevelSpawnAmount;

				yield return new WaitForSeconds (tempTime / 2f);
				while (spawnCollectables && GameManager.Instance.gameOver == false) {

					yield return new WaitForSeconds (tempTime / 7.5f);

					if(spawnDirection == 0)
						tempPoint += 1;
					if (spawnDirection == 1)
						tempPoint -= 1;

					if (tempPoint == spawnPoints.Count && spawnDirection == 0)
						tempPoint = 0;
					if (tempPoint < 0 && spawnDirection == 1)
						tempPoint = spawnPoints.Count-1;

					int specialSpawn = Random.Range(0, 35);
					if(specialSpawn == 1 && boostSpawned == false && GameManager.Instance.boostOngoing == false)
					{
						Instantiate (collectable_Boost, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);
						boostSpawned = true;
					}
					else
						Instantiate (collectables_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);

					spawnAmount += 1;

					if (spawnAmount == maxSpawnAmount)
						spawnCollectables = false;

					yield return null;
				}

				yield return new WaitForSeconds (tempTime / 2f);
			}

				else if(startCheck == 1)
				yield return new WaitForSeconds (GameManager.Instance.spawnTime);
				if (spawnCheck != 1)
				yield return new WaitForSeconds (GameManager.Instance.spawnTime);

				if(GameManager.Instance.currentLvlNumb == 1)
					StartCoroutine ("SpawnLevel_1", spawnDirection);
				if(GameManager.Instance.currentLvlNumb == 2)
					StartCoroutine ("SpawnLevel_2", spawnDirection);
				if(GameManager.Instance.currentLvlNumb == 3)
					StartCoroutine ("SpawnLevel_3", spawnDirection);
				if(GameManager.Instance.currentLvlNumb == 4)
					StartCoroutine ("SpawnLevel_3", spawnDirection);
			
		}
	}

	IEnumerator SpawnLevel_1(int spawnDirection)
	{
		int tempIns = Random.Range (0, spawnPoints.Count);	
		int randomNr = Random.Range (0, 13);

		if (randomNr == 1 || randomNr == 2 || randomNr == 3 || randomNr == 4) 
		{
			if(spawnDirection == 0)
				Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if (randomNr == 5 || randomNr == 6 || randomNr == 7) 
		{
			Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.10f)
			Instantiate (obstacle_VanillaRing_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		yield return null;
	}

	IEnumerator SpawnLevel_2(int spawnDirection)
	{
		int tempIns = Random.Range (0, spawnPoints.Count);	
		int randomNr = Random.Range (0, 14);

		if (randomNr == 1 || randomNr == 2 || randomNr == 3 || randomNr == 4 || randomNr == 5) 
		{
			if(spawnDirection == 0)
				Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if (randomNr == 6 || randomNr == 7) 
		{
			Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.10f)
		{
			Instantiate (obstacle_VanillaRing_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if(randomNr == 11)
		{
			Instantiate (obstacle_Big_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 12) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		yield return null;
	}
	IEnumerator SpawnLevel_3(int spawnDirection)
	{
		int tempIns = Random.Range (0, spawnPoints.Count);	
		int randomNr = Random.Range (0, 15);

		if (randomNr == 1 || randomNr == 2 || randomNr == 3) 
		{
			if(spawnDirection == 0)
				Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 4 || randomNr == 5) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 6) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_PikeBall_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_PikeBall_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 7) 
		{
			Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.10f)
		{
			Instantiate (obstacle_Big_Ring_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if(randomNr == 11 || randomNr == 12)
		{
			Instantiate (obstacle_Big_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else
			Instantiate (obstacle_SinglePikeBall_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		yield return null;
	}
	IEnumerator SpawnLevel_4(int spawnDirection)
	{
		int tempIns = Random.Range (0, spawnPoints.Count);	
		int randomNr = Random.Range (0, 16);

		if (randomNr == 1) 
		{
			if(spawnDirection == 0)
				Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 2 || randomNr == 3 || randomNr == 4 || randomNr == 5) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 6 || randomNr == 7) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_PikeBall_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_PikeBall_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 8) 
		{
			Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}

		else if(randomNr == 9 || randomNr == 10 || randomNr == 11 && hugBar.fillAmount >= 0.10f)
		{
			Instantiate (obstacle_Big_Ring_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if(randomNr == 12)
		{
			Instantiate (obstacle_Big_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 13 || randomNr == 14) 
		{
			if(spawnDirection == 0)
				Instantiate (obstacle_Big_2_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstacle_Big_2_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else
			Instantiate (obstacle_SinglePikeBall_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		yield return null;
	}
	void GameOver(string lol)
	{
		spawning = false;
	}
}
