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
	float boostSpeed = 45f;
	float boostSpawnTime = 0.35f;

	//Particle Systems
	public ParticleSystem speedFaking;
	public ParticleSystem speedFaking_2;
	public ParticleSystem boostEffect_1;

	float speedFakeWait = 0f;
	float speedFakeRate;
	float speedFakeStart = 1f;
	float speedFakeEnd = 85f;
	float speedFakeRateDuration = 600f;

	//Special lvl stuff
	bool specialLevelGoing = false;
	float tempGameSpeed = 0;

	//DeathQuote
	public Text deathQuote;

	//UIStuff
	public GameObject uiParent;

	// Use this for initialization
	void Start () 
	{
		//OBS!!! Deleting playerprefs for testing purposes
		PlayerPrefs.DeleteAll();

		uiParent.gameObject.SetActive(false);
		StartCoroutine("UiRoutine");

		highScore = PlayerPrefs.GetFloat ("Highscore");
		Debug.Log ("highscore: " + highScore);

		//Handling particle systems
		ParticleSystem.EmissionModule em = speedFaking.emission;
		em.enabled = true;
		ParticleSystem.EmissionModule em_2 = speedFaking_2.emission;
		em_2.enabled = true;
		ParticleSystem.EmissionModule em_3 = boostEffect_1.emission;
		em_3.enabled = false;

		//Handling collectables
		amountCollected = 0;
		amountCollected_text.text = "" + amountCollected;

		currentLvlNumb = 1;
		gameOver = false;
		pauseButton.enabled = true;
		playButton.enabled = false;
		restartButton.gameObject.SetActive (false);

		//Events listening
		CharCollison.OnCollision += GameOver;
		CollectDetector.OnCollect += Collected;
		CollectDetector.OnCollectBoost += BoostCollected;
		CollectDetector.OnCollectBig += BigCollected;
		ScoreDetector.OnCollided += AfterBoostCollided;
		ObstacleManager.OnSpecialStart += SpecialLevelStarted;
		ObstacleManager.OnSpecialStop += SpecialLevelStopped;

		//Setup up UI for deathqoute
		deathQuote.enabled = false;

		StartCoroutine("GameProgression");
	}

	IEnumerator UiRoutine()
	{
		yield return new WaitForSeconds (1.5f);
		uiParent.gameObject.SetActive(true);
		yield return null;
	}

	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver; 
		CollectDetector.OnCollect -= Collected;
		CollectDetector.OnCollectBoost -= BoostCollected;
		CollectDetector.OnCollectBig -= BigCollected;
		ScoreDetector.OnCollided -= AfterBoostCollided;
		ObstacleManager.OnSpecialStart -= SpecialLevelStarted;
		ObstacleManager.OnSpecialStop -= SpecialLevelStopped;
	}
	void SpecialLevelStarted()
	{
		StartCoroutine("SpecialLevelInit");
	}
	IEnumerator SpecialLevelInit()
	{
		specialLevelGoing = true;
		tempGameSpeed = gameSpeed;
		float t = 0;
		if(gameSpeed < 23)
		{
			while(t < 1)
			{
				t += Time.deltaTime / 2f;
				gameSpeed = Mathf.Lerp(tempGameSpeed, 23, t);
				yield return null;
			}
		}
		yield return null;
	}
	void SpecialLevelStopped()
	{
		StartCoroutine("SpecialLevelEnd");
	}
	IEnumerator SpecialLevelEnd()
	{
		float t = 0;
		float newTempSpeed = gameSpeed;
		if(tempGameSpeed < 23)
		{
			while(t < 1)
			{
				t += Time.deltaTime / 2f;
				gameSpeed = Mathf.Lerp(newTempSpeed, tempGameSpeed, t);
				yield return null;
			}
		}
		specialLevelGoing = false;
		yield return null;
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

		ParticleSystem.EmissionModule em = boostEffect_1.emission;
		em.enabled = true;

		float tempGameSpeed;
		float tempSpawnTime;
		tempSpawnTime = spawnTime;
		tempGameSpeed = gameSpeed;
		float t = 0;
		float camPos = 0;

		while(t < 1)
		{
			t += Time.deltaTime / 1f;
			gameSpeed = Mathf.Lerp (tempGameSpeed, boostSpeed, t);
			spawnTime = Mathf.Lerp (tempSpawnTime, boostSpawnTime, t);
//			camPos = Mathf.Lerp(camStart.position.y, camEnd.position.y, t);
//			mainCam.transform.position = new Vector3(0, camPos, 0);
//			mainCam.fieldOfView = Mathf.Lerp(camFOVstart, camFOVend, t);
			yield return null;
		}

		t = 0f;
		yield return new WaitForSeconds(boostTime/2);

		while(t < 1)
		{
			t += Time.deltaTime / (boostTime/2);
			spawnTime = Mathf.Lerp (boostSpawnTime, 1.5f, t);
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
//			camPos = Mathf.Lerp(camEnd.position.y, camStart.position.y, t);
//			mainCam.transform.position = new Vector3(0, camPos, 0);
//			mainCam.fieldOfView = Mathf.Lerp(camFOVend, camFOVstart, t);
			yield return null;
		}

		em = boostEffect_1.emission;
		em.enabled = false;

		gameSpeed = tempGameSpeed;
		spawnTime = tempSpawnTime;
		boostOngoing = false;
		waitForCollide = false;
		endBoost = false;

		t = 0f;
	}

	IEnumerator GameProgression () 
	{
		if(currentLvlNumb == 1)
		{
			while (t_speed < 1)
				{
					if(!boostOngoing && !specialLevelGoing)
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
				if(!boostOngoing && !specialLevelGoing)
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
				if(!boostOngoing && !specialLevelGoing)
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
				if(!boostOngoing && !specialLevelGoing)
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
		uiParent.gameObject.SetActive(false);
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

		deathQuote.enabled = true;

		Color startColor = new Color32(0,0,50,0);
		Color endColor = new Color32(0,0,50,255);
		deathQuote.color = startColor;
		float t = 0;

		yield return new WaitForSeconds(1f);

		while(t < 1)
		{
			t += Time.deltaTime/2;
			deathQuote.color = Color.Lerp(startColor, endColor,t);
			yield return null;
		}

		t = 0;
		yield return new WaitForSeconds(0.35f);
		while(t < 1)
		{
			t += Time.deltaTime;
			deathQuote.color = Color.Lerp(endColor,startColor,t);
			yield return null;
		}
		deathQuote.enabled = false;
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
