using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Advertisements;

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
	//lvl 5
	float startSpeed_5 = 5;
	public float endSpeed_5 = 20;
	public float endSpeedDuration_5 = 120;
	float startSpawnTime_5 = 2.5f;
	public float endSpawnTime_5 = 1f;


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
//	public Transform camStart;
//	public Transform camEnd;
	float camFOVstart = 65f;
	float camFOVend = 110f;
	bool endBoost = false;
	bool waitForCollide = false;
	public float boostSpeed = 45f;
	public float boostSpawnTime = 0.35f;

	//Particle Systems
	public ParticleSystem speedFaking;
	public ParticleSystem speedFaking_2;
	public ParticleSystem boostEffect_1;

	float speedFakeWait = 0f;
	float speedFakeRate;
	float speedFakeStart = 1f;
	float speedFakeEnd = 85f;
	float speedFakeRateDuration = 300f;
	float gravityFakeRate;

	//Camera STuff
	public Animation camAnimation;
	private BloomOptimized myBloom;
	float startBloomIntensity;
	float startBloomThreshold;
	float endBloomIntensity = 1.1f;
	float endBloomThreshold = 0.55f;

	//Touch UI stuff
	public ParticleSystem rightButtonParticles;
	public ParticleSystem leftButtonParticles;

	//Special lvl stuff
	bool specialLevelGoing = false;
	float tempGameSpeed = 0;

	//Hug Related stuff
	bool hugging;

	//DeathQuote
	public Text deathQuote;

	//UIStuff
	public GameObject uiParent;

	//Events
	public delegate void OnGameBeginsEvent();
	public static event OnGameBeginsEvent OnGameBegins;

	public delegate void OnBoostEndsEvent();
	public static event OnBoostEndsEvent OnBoostEnds;

	public delegate void OnShowSimpleAdEvent();
	public static event OnShowSimpleAdEvent OnShowSimpleAd;

	//UI STUFF
	public Button startButton;
	public Text startText;
	public Text highscoreText;
	public GameObject mainMenuUI;

	public GameObject mainMusic;
	public bool gameStarted = false;

	public SkinnedMeshRenderer charRenderer1;
	public SkinnedMeshRenderer charRenderer2;
	public MeshRenderer heart1;
	public MeshRenderer heart2;

	public GameObject char1;
	public GameObject char2;

	public int currentLevel;
	float levelProgress;
	float levelTier = 1.2f;
	float startXPrequired = 50f;
	float xpRequired;

	public Image levelProgressBar;
	public Image LevelProgressBarRun;
	public Text currentLevelText;
	public Text currentLevelTextRun;
	public Text scoreMultiplierText;

	// Use this for initialization
	void Start () 
	{
		//OBS!!! Deleting playerprefs for testing purposes
//  		PlayerPrefs.DeleteAll();

		// PLAYERPREFS STARTS
		highScore = PlayerPrefs.GetFloat ("Highscore");
		highscoreText.text = ""+highScore;
		Debug.Log ("highscore: " + highScore);

		currentLevel = PlayerPrefs.GetInt ("CurrentLevel");
		levelProgress = PlayerPrefs.GetFloat ("LevelProgress");
		xpRequired = PlayerPrefs.GetFloat ("XpRequired");
		if(currentLevel == 0)
		{
			currentLevel += 1;
		}
		if(xpRequired == 0)
		{
			xpRequired = startXPrequired;
		}

		levelProgressBar.fillAmount = levelProgress;
		LevelProgressBarRun.fillAmount = levelProgress;
		currentLevelText.text = "Level "+(currentLevel);
		currentLevelTextRun.text = "Level "+(currentLevel);
		scoreMultiplierText.text = "X"+(currentLevel);

		//// Playerprefs End

		charRenderer1.enabled = false;
		charRenderer2.enabled = false;
		heart1.enabled = false;
		heart2.enabled = false;

		myBloom = mainCam.GetComponent<BloomOptimized>();
		startBloomIntensity = myBloom.intensity;
		startBloomThreshold = myBloom.threshold;

		uiParent.gameObject.SetActive(false);
		mainMenuUI.gameObject.SetActive(true);

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
		RotatorManager.OnHug += StartHug;
		RotatorManager.OnStopHug += StopHug;
//		AdManager.OnAdEnded += AdEnded;

		//Setup up UI for deathqoute
		deathQuote.enabled = false;

		StartCoroutine("StartButtonFade");
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
		RotatorManager.OnHug -= StartHug;
		RotatorManager.OnStopHug -= StopHug;
//		AdManager.OnAdEnded -= AdEnded;
	}
	IEnumerator StartButtonFade()
	{
		startButton.interactable = false;
		Color startColor = new Color32(0,0,0,0);
		Color endColor = new Color32(48,46,46,215);
		startText.color = startColor;

		float t = 0;

		yield return new WaitForSeconds(1f);

		while(t < 1)
		{
			t += Time.deltaTime/1.5f;
			startText.color = Color.Lerp(startColor, endColor,t);
			yield return null;
		}

		startButton.interactable = true;
		t = 0;

		yield return null;
	}
	public void StartButtonPressed()
	{
		StartCoroutine("GameStart");
	}
	IEnumerator GameStart()
	{
		gameStarted = true;
		highscoreText.enabled = false;
		startButton.gameObject.SetActive(false);
		mainMusic.GetComponent<AudioSource>().Play();

		StartCoroutine("UiRoutine");
		camAnimation.Play("Cam_OnStart_Anim_1");

		charRenderer1.enabled = true;
		charRenderer2.enabled = true;
		heart1.enabled = true;
		heart2.enabled = true;

		StartCoroutine("CharMeet");

		OnGameBegins();
		StartCoroutine("GameProgression");
		yield return null;
	}
	IEnumerator CharMeet()
	{
		yield return new WaitForSeconds(0.5f);
		char1.gameObject.GetComponent<Animation>().Play();
		char2.gameObject.GetComponent<Animation>().Play();
		yield return null;
	}
	void StartHug()
	{
		hugging = true;
		StartCoroutine("WhileHugging");
	}
	void StopHug()
	{
		hugging = false;
	}
	IEnumerator WhileHugging()
	{
		float t = 0;
		while(hugging)
		{
			if(t < 1)
			{
				t += Time.deltaTime / 1f;
				myBloom.intensity = Mathf.Lerp(startBloomIntensity, endBloomIntensity, t);
				myBloom.threshold = Mathf.Lerp(startBloomThreshold, endBloomThreshold, t);
			}
			yield return null;
		}
		t = 0;
		float tempIntensity = myBloom.intensity;
		while(t < 1 && !hugging)
		{
			t += Time.deltaTime / 0.5f;
			myBloom.intensity = Mathf.Lerp(tempIntensity, startBloomIntensity, t);
//			myBloom.threshold = Mathf.Lerp(tempIntensity, startBloomThreshold, t);
			yield return null;
		}
		t = 0;
		while(t < 1 && !hugging)
		{
			t += Time.deltaTime / 0.5f;
//			myBloom.intensity = Mathf.Lerp(tempIntensity, startBloomIntensity, t);
			myBloom.threshold = Mathf.Lerp(tempIntensity, startBloomThreshold, t);
			yield return null;
		}
	}

	IEnumerator UiRoutine()
	{
		mainMenuUI.gameObject.SetActive(false);
		yield return new WaitForSeconds (1.5f);
		uiParent.gameObject.SetActive(true);
		yield return null;
	}
	void SpecialLevelStarted()
	{
		StartCoroutine("SpecialLevelInit");
		StartCoroutine("SpecialLevelChars");
	}
	IEnumerator SpecialLevelChars()
	{
		float t = 0;
		float startPos = 2.35f;
		float tempPos;

		while(t < 1)
		{
			t += Time.deltaTime/0.5f;
			tempPos = Mathf.Lerp(2.35f, 4f, t);
			char1.transform.localPosition = new Vector3(0,0,tempPos);
			char2.transform.localPosition = new Vector3(0,0,-tempPos);
			yield return null;
		}
		t = 0;
		yield return new WaitForSeconds(0.25f);
		while(t < 1)
		{
			t += Time.deltaTime/0.5f;
			tempPos = Mathf.Lerp(4f, 1.8f, t);
			char1.transform.localPosition = new Vector3(0,0,tempPos);
			char2.transform.localPosition = new Vector3(0,0,-tempPos);
			yield return null;
		}
		t= 0;
		while(t < 1)
		{
			t += Time.deltaTime/0.25f;
			tempPos = Mathf.Lerp(1.8f, 2.35f, t);
			char1.transform.localPosition = new Vector3(0,0,tempPos);
			char2.transform.localPosition = new Vector3(0,0,-tempPos);
			yield return null;
		}
	}
	IEnumerator SpecialLevelInit()
	{
		specialLevelGoing = true;
		tempGameSpeed = gameSpeed;
		float specialGameSpeed = tempGameSpeed+4f;
		float t = 0;
		if(gameSpeed <= 26)
		{
			while(t < 1)
			{
				t += Time.deltaTime / 2f;
				gameSpeed = Mathf.Lerp(tempGameSpeed, specialGameSpeed, t);
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
		if(tempGameSpeed <= 26)
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
		levelProgress += 1/xpRequired;
		LevelProgressBarRun.fillAmount = levelProgress;

		if(levelProgress > 1)
		{
			LevelUp();
			levelProgress = 0;
			LevelProgressBarRun.fillAmount = 0;
		}

		amountCollected += collectReward;
		amountCollected_text.text = "" + amountCollected;
	}
		
	void BigCollected()
	{
		levelProgress += 1/xpRequired;
		LevelProgressBarRun.fillAmount = levelProgress;

		if(levelProgress > 1)
		{
			LevelUp();
			levelProgress = 0;
			LevelProgressBarRun.fillAmount = 0;
		}

		amountCollected += collectReward_Big;
		amountCollected_text.gameObject.GetComponent<Animation>().Play();
		amountCollected_text.text = "" + amountCollected;
	}
	void BoostCollected()
	{
		StartCoroutine("DoBoost");
	}

	void LevelUp()
	{
		xpRequired *= levelTier;
		currentLevel += 1;
		currentLevelTextRun.text = "Level "+currentLevel;
		scoreMultiplierText.text = "X"+currentLevel;

		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
		PlayerPrefs.SetFloat("LevelProgress", levelProgress);
		PlayerPrefs.SetFloat("XpRequired", xpRequired);
	}

	IEnumerator DoBoost()
	{
		boostOngoing = true;

		leftButtonParticles.gameObject.SetActive(false);
		rightButtonParticles.gameObject.SetActive(false);

		ParticleSystem.EmissionModule em = boostEffect_1.emission;
		em.enabled = true;

		float tempGameSpeed;
		float tempSpawnTime;
		tempSpawnTime = spawnTime;
		tempGameSpeed = gameSpeed;
		float t = 0;
		float camPos = 0;

		camAnimation.Play("Cam_BoostStart_Anim_1");

		while(t < 1)
		{
			t += Time.deltaTime / 1f;
			gameSpeed = Mathf.Lerp (tempGameSpeed, boostSpeed, t);
			spawnTime = Mathf.Lerp (tempSpawnTime, boostSpawnTime, t);
			myBloom.intensity = Mathf.Lerp (startBloomIntensity, endBloomIntensity, t);
			myBloom.threshold = Mathf.Lerp (startBloomThreshold, endBloomThreshold, t);
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

		camAnimation.Play("Cam_BoostEnd_Anim_1");
		OnBoostEnds();

		while(t < 1)
		{
			t += Time.deltaTime / 0.25f;
			gameSpeed = Mathf.Lerp (gameSpeed, tempGameSpeed, t);
			spawnTime = Mathf.Lerp (spawnTime, tempSpawnTime, t);
			myBloom.intensity = Mathf.Lerp (endBloomIntensity, startBloomIntensity, t);
			myBloom.threshold = Mathf.Lerp (endBloomThreshold, startBloomThreshold, t);
//			camPos = Mathf.Lerp(camEnd.position.y, camStart.position.y, t);
//			mainCam.transform.position = new Vector3(0, camPos, 0);
//			mainCam.fieldOfView = Mathf.Lerp(camFOVend, camFOVstart, t);
			yield return null;
		}

		leftButtonParticles.gameObject.SetActive(true);
		rightButtonParticles.gameObject.SetActive(true);

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
		if(currentLvlNumb == 5)
		{
			startSpeed_5 = gameSpeed;
			startSpawnTime_5 = spawnTime;

			while (t_speed < 1)
			{
				if(!boostOngoing && !specialLevelGoing)
				{
					t_speed += Time.deltaTime / endSpeedDuration_5;
					gameSpeed = Mathf.Lerp (startSpeed_5, endSpeed_5, t_speed);
					spawnTime = Mathf.Lerp (startSpawnTime_5, endSpawnTime_5, t_speed);
					yield return null;
				}
				yield return null;
			}
//			t_speed = 0;
//			currentLvlNumb += 1;
		}

	}


	IEnumerator GameOverState()
	{
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
		PlayerPrefs.SetFloat("LevelProgress", levelProgress);
		PlayerPrefs.SetFloat("XpRequired", xpRequired);

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
		Color endColor = new Color32(48,46,46,215);
		deathQuote.color = startColor;
		float t = 0;

		yield return new WaitForSeconds(4f);

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		OnShowSimpleAd();
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
