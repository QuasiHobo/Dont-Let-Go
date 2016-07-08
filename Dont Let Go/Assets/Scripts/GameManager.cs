using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{

	// Singleton
	private static GameManager instance;
	// Construct
	private GameManager ()
	{
	}
	// Instance
	public static GameManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(GameManager)) as GameManager;

			return instance;
		}
	}

	float endScore;
	float highScore;

	//Collectables
	float amountCollected;
	public Text amountCollected_text;

	public int collectReward = 1;
	public int collectReward_Big = 10;

	public int currentLvlNumb;
	//lvl 1
	public float startSpeed = 5;
	public float endSpeed = 20;
	public float endSpeedDuration = 60;

	public float startSpawnTime = 2.5f;
	public float endSpawnTime = 1f;
	//lvl 2
	float startSpeed_2 = 5;
	public float endSpeed_2 = 20;
	public float endSpeedDuration_2 = 60;
	float startSpawnTime_2 = 2.5f;
	public float endSpawnTime_2 = 1f;
	//lvl 3
	float startSpeed_3 = 5;
	public float endSpeed_3 = 20;
	public float endSpeedDuration_3 = 60;
	float startSpawnTime_3 = 2.5f;
	public float endSpawnTime_3 = 1f;
	//lvl 4
	float startSpeed_4 = 5;
	public float endSpeed_4 = 20;
	public float endSpeedDuration_4 = 60;
	float startSpawnTime_4 = 2.5f;
	public float endSpawnTime_4 = 1f;


	float t_speed = 0;
	public float gameSpeed;

	public float spawnTime;

	public Image pauseButton;
	public Image playButton;

	public Button restartButton;

	public bool gameOver = false;

	public int bonusLevelChance = 20;
	public int bonusLevelSpawnAmount = 32;

	//Special collectables
	public bool boostOngoing = false;
	public float boostTime = 5f;
	public Camera mainCam;
	public Transform camStart;
	public Transform camEnd;
	float camFOVstart = 65f;
	float camFOVend = 110f;
	bool endBoost = false;
	bool waitForCollide = false;

	//Particle Systems
	public ParticleSystem speedFaking;
	public ParticleSystem speedFaking_2;
	float speedFakeWait = 0f;
	float speedFakeRate;
	float speedFakeStart = 1f;
	float speedFakeEnd = 85f;
	float speedFakeRateDuration = 600f;

	// Use this for initialization
	void Start () 
	{
		//OBS!!! Deleting playerprefs for testing purposes
		PlayerPrefs.DeleteAll();

		highScore = PlayerPrefs.GetFloat ("Highscore");
		Debug.Log ("highscore: " + highScore);

		//Handling particle systems
		ParticleSystem.EmissionModule em = speedFaking.emission;
		em.enabled = true;
		ParticleSystem.EmissionModule em_2 = speedFaking_2.emission;
		em_2.enabled = true;

		//Handling collectables
		amountCollected = 0;
		amountCollected_text.text = "" + amountCollected;

		currentLvlNumb = 1;
		gameOver = false;
		pauseButton.enabled = true;
		playButton.enabled = false;
		restartButton.gameObject.SetActive (false);

		CharCollison.OnCollision += GameOver;
		CollectDetector.OnCollect += Collected;
		CollectDetector.OnCollectBoost += BoostCollected;
		CollectDetector.OnCollectBig += BigCollected;
		ScoreDetector.OnCollided += AfterBoostCollided;

		StartCoroutine("GameProgression");
	}

	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver; 
		CollectDetector.OnCollect -= Collected;
		CollectDetector.OnCollectBoost -= BoostCollected;
		CollectDetector.OnCollectBig -= BigCollected;
		ScoreDetector.OnCollided -= AfterBoostCollided;
	}
	void AfterBoostCollided()
	{
		endBoost = false;
		if(waitForCollide)
		endBoost = true;
	}
	void GameOver(string lol)
	{
		gameOver = true;
		StartCoroutine ("GameOverState");
	}
	void Collected()
	{
		amountCollected += collectReward;
		amountCollected_text.text = "" + amountCollected;
	}
	void BigCollected()
	{
		amountCollected += collectReward_Big;
		amountCollected_text.gameObject.GetComponent<Animation>().Play();
		amountCollected_text.text = "" + amountCollected;
	}
	void BoostCollected()
	{
		StartCoroutine("DoBoost");
	}
	IEnumerator DoBoost()
	{
		boostOngoing = true;
		float tempGameSpeed;
		float tempSpawnTime;
		tempSpawnTime = spawnTime;
		tempGameSpeed = gameSpeed;
		float t = 0;
		float camPos = 0;

		while(t < 1)
		{
			t += Time.deltaTime / 1f;
			gameSpeed = Mathf.Lerp (tempGameSpeed, 40, t);
			spawnTime = Mathf.Lerp (tempSpawnTime, 0.35f, t);
			camPos = Mathf.Lerp(camStart.position.y, camEnd.position.y, t);
			mainCam.transform.position = new Vector3(0, camPos, 0);
			mainCam.fieldOfView = Mathf.Lerp(camFOVstart, camFOVend, t);
			yield return null;
		}

		t = 0f;
		yield return new WaitForSeconds(boostTime/2);

		while(t < 1)
		{
			t += Time.deltaTime / (boostTime/2);
			spawnTime = Mathf.Lerp (0.35f, 1.5f, t);
			yield return null;
		}

		Debug.Log("BoostTime done!!");

		t = 0f;
		waitForCollide = true;

		while(endBoost == false)
		{
			yield return null;
		}

		while(t < 1)
		{
			t += Time.deltaTime / 0.25f;
			gameSpeed = Mathf.Lerp (gameSpeed, tempGameSpeed, t);
			spawnTime = Mathf.Lerp (spawnTime, tempSpawnTime, t);
			camPos = Mathf.Lerp(camEnd.position.y, camStart.position.y, t);
			mainCam.transform.position = new Vector3(0, camPos, 0);
			mainCam.fieldOfView = Mathf.Lerp(camFOVend, camFOVstart, t);
			yield return null;
		}
			
		gameSpeed = tempGameSpeed;
		spawnTime = tempSpawnTime;
		boostOngoing = false;
		waitForCollide = false;
		endBoost = false;
	}

	IEnumerator GameProgression () 
	{
		if(currentLvlNumb == 1)
		{
			while (t_speed < 1)
				{
					if(!boostOngoing)
					{
						t_speed += Time.deltaTime / endSpeedDuration;
						gameSpeed = Mathf.Lerp (startSpeed, endSpeed, t_speed);
						spawnTime = Mathf.Lerp (startSpawnTime, endSpawnTime, t_speed);
						yield return null;
					}
				yield return null;
				}
			t_speed = 0;
			currentLvlNumb += 1;
		}
		if(currentLvlNumb == 2)
		{
			startSpeed_2 = gameSpeed;
			startSpawnTime_2 = spawnTime;

			while (t_speed < 1)
			{
				if(!boostOngoing)
				{
					t_speed += Time.deltaTime / endSpeedDuration_2;
					gameSpeed = Mathf.Lerp (startSpeed_2, endSpeed_2, t_speed);
					spawnTime = Mathf.Lerp (startSpawnTime_2, endSpawnTime_2, t_speed);
					yield return null;
				}
				yield return null;
			}
			t_speed = 0;
			currentLvlNumb += 1;
		}
		if(currentLvlNumb == 3)
		{
			startSpeed_3 = gameSpeed;
			startSpawnTime_3 = spawnTime;

			while (t_speed < 1)
			{
				if(!boostOngoing)
				{
					t_speed += Time.deltaTime / endSpeedDuration_3;
					gameSpeed = Mathf.Lerp (startSpeed_3, endSpeed_3, t_speed);
					spawnTime = Mathf.Lerp (startSpawnTime_3, endSpawnTime_3, t_speed);
					yield return null;
				}
				yield return null;
			}
			t_speed = 0;
			currentLvlNumb += 1;
		}
		if(currentLvlNumb == 4)
		{
			startSpeed_4 = gameSpeed;
			startSpawnTime_4 = spawnTime;

			while (t_speed < 1)
			{
				if(!boostOngoing)
				{
					t_speed += Time.deltaTime / endSpeedDuration_4;
					gameSpeed = Mathf.Lerp (startSpeed_4, endSpeed_4, t_speed);
					spawnTime = Mathf.Lerp (startSpawnTime_4, endSpawnTime_4, t_speed);
					yield return null;
				}
				yield return null;
			}
			t_speed = 0;
			currentLvlNumb += 1;
		}

	}

	void Update()
	{
		if(!gameOver)
		{
			if(speedFakeWait < 1)
			{
				speedFakeWait += Time.deltaTime / speedFakeRateDuration;

				var em = speedFaking_2.emission;
				var rate = em.rate;
				rate.mode = ParticleSystemCurveMode.Constant;

				speedFakeRate = Mathf.Lerp(speedFakeStart, speedFakeEnd, speedFakeWait);
				rate.constantMin = speedFakeRate;
				rate.constantMax = speedFakeRate;

				em.rate = rate;
			}

			speedFaking.gravityModifier = -(gameSpeed);
			speedFaking_2.gravityModifier = -(gameSpeed);
//			speedFaking_2.emission.rate = 
		}
	}

	IEnumerator GameOverState()
	{
		endScore = ScoreDetector.Instance.totalScore;

		ParticleSystem.EmissionModule em = speedFaking.emission;
		em.enabled = false;
		ParticleSystem.EmissionModule em_2 = speedFaking_2.emission;
		em_2.enabled = false;

		if (endScore > highScore) 
		{
			PlayerPrefs.SetFloat ("Highscore", endScore);
			highScore = PlayerPrefs.GetFloat ("Highscore");
		}

		Debug.Log ("SCORE: " + endScore);
		Debug.Log ("Highscore: " + highScore);

		yield return new WaitForSeconds (1.5f);
		restartButton.gameObject.SetActive (true);
		yield return null;
	}

	public void RestartButtonPressed()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void PauseGame()
	{
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			pauseButton.enabled = false;
			playButton.enabled = true;
		} else {
			Time.timeScale = 1;
			pauseButton.enabled = true;
			playButton.enabled = false;
		}
	}
}
