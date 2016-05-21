using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

	// Use this for initialization
	void Start () 
	{
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
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		yield return null;
	}

}
