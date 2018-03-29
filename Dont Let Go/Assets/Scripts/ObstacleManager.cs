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
	GameObject collectable_Big;
	GameObject collectable_Boost;

	public List<GameObject> spawnPoints = new List<GameObject> ();
	public GameObject obstacle_1;
	public GameObject obstacle_Big_1;
	public GameObject obstalce_Big_Rot_1;
	public GameObject obstalce_Big_Rot_2;
	public GameObject obstacle_Middle_1;
	public GameObject obstacle_Big_Ring_1;
	public GameObject obstacle_Ring_Tunnel_1;
	public GameObject obstacle_Big_2;
	public GameObject obstacle_PikeBall_1;
	public GameObject obstacle_PikeBall_2;
	public GameObject obstacle_SinglePikeBall_1;
	public GameObject obstacle_SinglePikeBall_Small_1;
	public GameObject obstacle_VanillaRing_1;
	public GameObject obstacle_Big_Rot_Fast_1;
	public GameObject obstacle_Big_Rot_Fast_2;
	public GameObject obstacle_Big_2_Rot_1;
	public GameObject obstacle_Big_2_Rot_2;

	//Particle effects
	public ParticleSystem speedFake_2;
	public ParticleSystem specialEffect_1;

	bool spawning;
	public bool doingBoost = false;

	//For special lvls
	Camera mainCam;
	public bool spawningSpecial = false;
	Color normalColor;
	public Color specialColor_1;
	public Color specialColor_2;
	public Color specialColor_3;

	float specialSpawnInterval = 0.8f;

	public GameObject midSpawn;

	//For checking fillamount
	public Image hugBar;

	public int startCheck = 0;

	//Events
	public delegate void OnSpecialStartEvent();
	public static event OnSpecialStartEvent OnSpecialStart;

	public delegate void OnSpecialStopEvent();
	public static event OnSpecialStopEvent OnSpecialStop;

	// Use this for initialization
	void Start () 
	{
		//Load in Obstacle Objects
		obstacle_1 = Resources.Load("Prefabs/Obstacles/Obstacle_1") as GameObject;
		obstacle_Big_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_1") as GameObject;
		obstalce_Big_Rot_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_1") as GameObject;
		obstalce_Big_Rot_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_2") as GameObject;
		obstacle_Middle_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Middle_1") as GameObject;
		obstacle_Big_Ring_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Ring_1") as GameObject;
		obstacle_Ring_Tunnel_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Ring_Tunnel_1") as GameObject;
		obstacle_Big_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2") as GameObject;
		obstacle_PikeBall_1 = Resources.Load("Prefabs/Obstacles/Obstacle_PikeBall_1") as GameObject;
		obstacle_PikeBall_2 = Resources.Load("Prefabs/Obstacles/Obstacle_PikeBall_2") as GameObject;
		obstacle_SinglePikeBall_1 = Resources.Load("Prefabs/Obstacles/Obstacle_SinglePikeBall_1") as GameObject;
		obstacle_SinglePikeBall_Small_1 = Resources.Load("Prefabs/Obstacles/Obstacle_SinglePikeBall_Small_1") as GameObject;
		obstacle_VanillaRing_1 = Resources.Load("Prefabs/Obstacles/Obstacle_VanillaRing_1") as GameObject;
		obstacle_Big_Rot_Fast_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_fast_1") as GameObject;
		obstacle_Big_Rot_Fast_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_Rot_fast_2") as GameObject;
		obstacle_Big_2_Rot_1 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2_Rot_1") as GameObject;
		obstacle_Big_2_Rot_2 = Resources.Load("Prefabs/Obstacles/Obstacle_Big_2_Rot_2") as GameObject;

		//Load collectable Objects
		collectables_1 = Resources.Load("Prefabs/Collectables/Collectable_2") as GameObject;
		collectable_Big = Resources.Load("Prefabs/Collectables/Collectable_Big") as GameObject;
		collectable_Boost = Resources.Load("Prefabs/Collectables/Collectable_Boost_1") as GameObject;

		//particle effects
		ParticleSystem.EmissionModule em = specialEffect_1.emission;
		em.enabled = false;

		spawning = false;

		foreach (Transform child in transform) 
		{
			GameObject obj = child.gameObject;
			spawnPoints.Add (obj);
		}

		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		normalColor = mainCam.backgroundColor;


	}
	void Awake()
	{
		CharCollison.OnCollision += GameOver;
		CharCollison.OnCollision += StopSpawning;
		GameManager.OnGameBegins += OnGameBegins;
	}

	void OnGameBegins ()
	{
		spawning = true;
		StartCoroutine ("Spawner");
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
		CharCollison.OnCollision -= StopSpawning;
		GameManager.OnGameBegins -= OnGameBegins;
	}

	void StopSpawning(string charNumb)
	{
		spawning = false;
	}

	IEnumerator Spawner()
	{
//		spawning = true;

		while (spawning && GameManager.Instance.gameOver == false) 
		{
			if(spawningSpecial)
			{
				while(spawningSpecial)
				{
					yield return null;
				}
			}

			startCheck += 1;
			int spawnDirection = Random.Range (0, 2);
			int spawnCheck = Random.Range (1, 7);

			if (startCheck >= 3 && spawnCheck == 1) 
			{
				bool spawnCollectables = true;
				bool boostSpawned = false;
				float tempTime = GameManager.Instance.spawnTime;

//				int tempPoint = Random.Range (0, spawnPoints.Count);
//				int tempPoint_2 = Random.Range(0, spawnPoints.Count);

				int tempPoint = 0;
				int tempPoint_2 = 4;

				int spawnAmount = 0;
				int maxSpawnAmount = Random.Range (6, 12);

				int chanceForBonusLevel = Random.Range (0, GameManager.Instance.bonusLevelChance);
				if (chanceForBonusLevel == 1)
					maxSpawnAmount = GameManager.Instance.bonusLevelSpawnAmount;

				bool doubleHelix = false;
				int doubleHelixCheck = Random.Range(0,2);
				if(doubleHelixCheck == 1)
					doubleHelix = true;

				yield return new WaitForSeconds (tempTime / 2f);
				while (spawnCollectables && GameManager.Instance.gameOver == false && GameManager.Instance.boostOngoing == false) {

					yield return new WaitForSeconds (tempTime / 8.5f);

					if(spawnDirection == 0)
					{
						tempPoint += 1;
						tempPoint_2 += 1;
					}
					if (spawnDirection == 1)
					{
						tempPoint -= 1;
						tempPoint_2 -= 1;
					}

					if (tempPoint == spawnPoints.Count && spawnDirection == 0)
						tempPoint = 0;
					if (tempPoint_2 == spawnPoints.Count && spawnDirection == 0)
						tempPoint_2 = 0;
					if (tempPoint < 0 && spawnDirection == 1)
						tempPoint = spawnPoints.Count-1;
					if (tempPoint_2 < 0 && spawnDirection == 1)
						tempPoint_2 = spawnPoints.Count-1;

					int specialSpawn = Random.Range(0, 22);
					if(specialSpawn == 1 && boostSpawned == false && GameManager.Instance.boostOngoing == false && spawningSpecial == false)
					{
						Instantiate (collectable_Boost, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);

						if(doubleHelix)
						Instantiate (collectables_1, spawnPoints [tempPoint_2].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);
						
						boostSpawned = true;
					}
					else if(specialSpawn == 2 || specialSpawn == 3)
					{
						Instantiate (collectable_Big, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);

						if(doubleHelix)
						Instantiate (collectables_1, spawnPoints [tempPoint_2].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);
					}
					else
					{
						Instantiate (collectables_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);

						if(doubleHelix)
						Instantiate (collectables_1, spawnPoints [tempPoint_2].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);
					}

					spawnAmount += 1;

					if (spawnAmount == maxSpawnAmount)
						spawnCollectables = false;

					yield return null;
				}
				yield return null;
			}

//				else if(startCheck == 1)
//				yield return new WaitForSeconds (GameManager.Instance.spawnTime);
//				if (spawnCheck != 1)
				yield return new WaitForSeconds (GameManager.Instance.spawnTime);

			if(GameManager.Instance.gameOver == false)
			{
				int specialCheck = Random.Range(0, 15); //16
				if(specialCheck == 1 && GameManager.Instance.boostOngoing == false && startCheck > 12)
				{
					spawningSpecial = true;
					yield return new WaitForSeconds (1.2f);

					if(GameManager.Instance.currentLvlNumb == 1)
					{
						int ranCheck = Random.Range(1, 3);
						if(ranCheck == 1)
							StartCoroutine("SpawnSpecial_1");
						if(ranCheck == 2)
							StartCoroutine("SpawnSpecial_2");
					}
					else
					{
						int ranCheck = Random.Range(1, 5);
						if(ranCheck == 1)
							StartCoroutine("SpawnSpecial_1");
						if(ranCheck == 2)
							StartCoroutine("SpawnSpecial_2");
						if(ranCheck == 3)
							StartCoroutine("SpawnSpecial_3");
						if(ranCheck == 4)
							StartCoroutine("SpawnSpecial_4");
					}
					
				}
				else
				{
					if(GameManager.Instance.currentLvlNumb == 1)
						StartCoroutine ("SpawnLevel_1", spawnDirection);
					if(GameManager.Instance.currentLvlNumb == 2)
						StartCoroutine ("SpawnLevel_2", spawnDirection);
					if(GameManager.Instance.currentLvlNumb == 3)
						StartCoroutine ("SpawnLevel_3", spawnDirection);
					if(GameManager.Instance.currentLvlNumb == 4)
						StartCoroutine ("SpawnLevel_4", spawnDirection);
					if(GameManager.Instance.currentLvlNumb == 5)
						StartCoroutine ("SpawnLevel_4", spawnDirection);
				}
			}
		}
	}

	IEnumerator SpawnSpecial_1()
	{
		yield return new WaitForSeconds(1.25f);
	
		ParticleSystem.EmissionModule em = speedFake_2.emission;
		em.enabled = false;
		ParticleSystem.EmissionModule em_2 = specialEffect_1.emission;
		em_2.enabled = true;

		int spawnDirectionSpecial = Random.Range(0, 2);
		float t = 0;
		OnSpecialStart();

		int tempInt = Random.Range(1, 4);
		Color tempColor = specialColor_1;
		if(tempInt == 1)
			tempColor = specialColor_1;
		if(tempInt == 2)
			tempColor = specialColor_2;
		if(tempInt == 3)
			tempColor = specialColor_3;

		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(normalColor, tempColor, t);
			yield return null;
		}
		while(spawningSpecial && GameManager.Instance.gameOver == false && spawning == true)
		{
			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		yield return new WaitForSeconds(specialSpawnInterval);

			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		yield return new WaitForSeconds(specialSpawnInterval);

			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		yield return new WaitForSeconds(specialSpawnInterval);

			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		yield return new WaitForSeconds(specialSpawnInterval);

			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		yield return new WaitForSeconds(specialSpawnInterval);

			Instantiate (obstacle_Big_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		spawningSpecial = false;

		yield return null;
		}

		t = 0;
		yield return new WaitForSeconds(1);

		if(GameManager.Instance.gameOver == false)
		{
			em.enabled = true;
		}

		em_2.enabled = false;

		OnSpecialStop();
		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(tempColor, normalColor, t);
			yield return null;
		}
		yield return null;
	}
	IEnumerator SpawnSpecial_2()
	{
		yield return new WaitForSeconds(1.25f);

		ParticleSystem.EmissionModule em = speedFake_2.emission;
		em.enabled = false;
		ParticleSystem.EmissionModule em_2 = specialEffect_1.emission;
		em_2.enabled = true;

		int spawnDirectionSpecial = Random.Range(0, 2);
		float t = 0;
		OnSpecialStart();

		int tempInt = Random.Range(1, 4);
		Color tempColor = specialColor_1;
		if(tempInt == 1)
			tempColor = specialColor_1;
		if(tempInt == 2)
			tempColor = specialColor_2;
		if(tempInt == 3)
			tempColor = specialColor_3;

		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(normalColor, tempColor, t);
			yield return null;
		}
		while(spawningSpecial && GameManager.Instance.gameOver == false && spawning == true)
		{
			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			yield return new WaitForSeconds(specialSpawnInterval);

			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			yield return new WaitForSeconds(specialSpawnInterval);

			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			yield return new WaitForSeconds(specialSpawnInterval);

			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			yield return new WaitForSeconds(specialSpawnInterval);

			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			yield return new WaitForSeconds(specialSpawnInterval);

			if(spawnDirectionSpecial == 0)
				Instantiate (obstacle_Big_Rot_Fast_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirectionSpecial == 1)
				Instantiate (obstacle_Big_Rot_Fast_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

			spawningSpecial = false;
			yield return null;
		}

		t = 0;
		yield return new WaitForSeconds(1);

		if(GameManager.Instance.gameOver == false)
		{
			em.enabled = true;
		}

		em_2.enabled = false;

		OnSpecialStop();
		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(tempColor, normalColor, t);
			yield return null;
		}
		yield return null;
	}

	IEnumerator SpawnSpecial_3()
	{
		yield return new WaitForSeconds(1.25f);

		ParticleSystem.EmissionModule em = speedFake_2.emission;
		em.enabled = false;
		ParticleSystem.EmissionModule em_2 = specialEffect_1.emission;
		em_2.enabled = true;

		int spawnDirectionSpecial = Random.Range(0, 2);
		float t = 0;
		OnSpecialStart();

		int tempInt = Random.Range(1, 4);
		Color tempColor = specialColor_1;
		if(tempInt == 1)
			tempColor = specialColor_1;
		if(tempInt == 2)
			tempColor = specialColor_2;
		if(tempInt == 3)
			tempColor = specialColor_3;

		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(normalColor, tempColor, t);
			yield return null;
		}

		int spawnAmount = 0;
		int tempPoint = Random.Range(0, spawnPoints.Count);
		int maxSpawnAmount = Random.Range(10, 18);

		while(spawningSpecial && GameManager.Instance.gameOver == false && spawning == true)
		{
				if(spawnDirectionSpecial == 0)
				{
					tempPoint += 1;
				}
				if (spawnDirectionSpecial == 1)
				{
					tempPoint -= 1;
				}

			if (tempPoint == spawnPoints.Count && spawnDirectionSpecial == 0)
					tempPoint = 0;
			if (tempPoint < 0 && spawnDirectionSpecial == 1)
					tempPoint = spawnPoints.Count-1;

			Instantiate (obstacle_SinglePikeBall_Small_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);


				spawnAmount += 1;

				if (spawnAmount == maxSpawnAmount)
				spawningSpecial = false;

			yield return new WaitForSeconds(0.25f);
			
		}

		t = 0;
		yield return new WaitForSeconds(1);

		if(GameManager.Instance.gameOver == false)
		{
			em.enabled = true;
		}

		em_2.enabled = false;

		OnSpecialStop();
		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(tempColor, normalColor, t);
			yield return null;
		}
		yield return null;
	}
	IEnumerator SpawnSpecial_4()
	{
		yield return new WaitForSeconds(1.25f);

		ParticleSystem.EmissionModule em = speedFake_2.emission;
		em.enabled = false;
		ParticleSystem.EmissionModule em_2 = specialEffect_1.emission;
		em_2.enabled = true;

		int spawnDirectionSpecial = Random.Range(0, 2);
		float t = 0;
		OnSpecialStart();

		int tempInt = Random.Range(1, 4);
		Color tempColor = specialColor_1;
		if(tempInt == 1)
			tempColor = specialColor_1;
		if(tempInt == 2)
			tempColor = specialColor_2;
		if(tempInt == 3)
			tempColor = specialColor_3;

		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(normalColor, tempColor, t);
			yield return null;
		}

		int spawnAmount = 0;
		int tempPoint = Random.Range(0, spawnPoints.Count);
		int maxSpawnAmount = Random.Range(10, 18);

		while(spawningSpecial && GameManager.Instance.gameOver == false && spawning == true)
		{
			if(spawnDirectionSpecial == 0)
			{
				tempPoint += 1;
			}
			if (spawnDirectionSpecial == 1)
			{
				tempPoint -= 1;
			}

			if (tempPoint == spawnPoints.Count && spawnDirectionSpecial == 0)
				tempPoint = 0;
			if (tempPoint < 0 && spawnDirectionSpecial == 1)
				tempPoint = spawnPoints.Count-1;

			Instantiate (obstacle_1, spawnPoints [tempPoint].gameObject.transform.position, spawnPoints [tempPoint].gameObject.transform.rotation);


			spawnAmount += 1;

			if (spawnAmount == maxSpawnAmount)
				spawningSpecial = false;

			yield return new WaitForSeconds(0.45f);

		}

		t = 0;
		yield return new WaitForSeconds(1);

		if(GameManager.Instance.gameOver == false)
		{
			em.enabled = true;
		}

		em_2.enabled = false;

		OnSpecialStop();
		while(t < 1)
		{
			t += Time.deltaTime / 2f;
			mainCam.backgroundColor = Color.Lerp(tempColor, normalColor, t);
			yield return null;
		}
		yield return null;
	}
	IEnumerator SpawnLevel_1(int spawnDirection)
	{
		int tempIns = Random.Range (1, spawnPoints.Count);	
		int randomNr = Random.Range (1, 13);

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

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.1f)
			Instantiate (obstacle_VanillaRing_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);

		yield return null;
	}

	IEnumerator SpawnLevel_2(int spawnDirection)
	{
		int tempIns = Random.Range (1, spawnPoints.Count);	
		int randomNr = Random.Range (1, 14);

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

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.1f)
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
		int tempIns = Random.Range (1, spawnPoints.Count);	
		int randomNr = Random.Range (1, 15);

		if (randomNr == 1 || randomNr == 2) 
		{
			if(spawnDirection == 0)
				Instantiate (obstalce_Big_Rot_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			if(spawnDirection == 1)
				Instantiate (obstalce_Big_Rot_2, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
		}
		else if (randomNr == 3 || randomNr == 4 || randomNr == 5) 
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

		else if(randomNr == 8 || randomNr == 9 || randomNr == 10 && hugBar.fillAmount >= 0.1f)
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
		int tempIns = Random.Range (1, spawnPoints.Count);	
		int randomNr = Random.Range (1, 16);

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

		else if(randomNr == 9 || randomNr == 10 || randomNr == 11 && hugBar.fillAmount >= 0.1f)
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
