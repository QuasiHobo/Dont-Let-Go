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

	public float startSpeed = 5;
	public float endSpeed = 20;
	public float endSpeedDuration = 60;

	float t_speed = 0;
	public float gameSpeed;

	public float startSpawnTime = 2.5f;
	public float endSpawnTime = 1f;
	public float endSpawnDuration = 60;

	float t_spawn = 0;
	public float spawnTime;

	bool gamePaused;
	public Image pauseButton;
	public Image playButton;

	public Button restartButton;

	public bool gameOver = false;

	// Use this for initialization
	void Start () 
	{
		//OBS!!! Deleting playerprefs for testing purposes
		PlayerPrefs.DeleteAll();

		highScore = PlayerPrefs.GetFloat ("Highscore");
		Debug.Log ("highscore: " + highScore);

		gamePaused = false;
		pauseButton.enabled = true;
		playButton.enabled = false;
		restartButton.gameObject.SetActive (false);

		CharCollison.OnCollision += GameOver;
	}

	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver; 
	}

	void GameOver(string lol)
	{
		StartCoroutine ("GameOverState");
	}

	void Update () 
	{
		if (t_speed < 1) {
			t_speed += Time.deltaTime / endSpeedDuration;
			gameSpeed = Mathf.Lerp (startSpeed, endSpeed, t_speed);
		}
		if (t_spawn < 1) {
			t_spawn += Time.deltaTime / endSpawnDuration;
			spawnTime = Mathf.Lerp (startSpawnTime, endSpawnTime, t_spawn);
		}
	}

	IEnumerator GameOverState()
	{
		endScore = ScoreDetector.Instance.totalScore;

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
