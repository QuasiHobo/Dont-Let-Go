using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	// Singleton
	private static AudioManager instance;
	// Construct
	private AudioManager ()
	{
	}
	// Instance
	public static AudioManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(AudioManager)) as AudioManager;

			return instance;
		}
	}

	public AudioSource mainMusic;
	public AudioSource gameOverSound;
	public AudioSource obstaclePass;

	// Use this for initialization
	void Start () 
	{
		//Listening to events
		CharCollison.OnCollision += GameOver;
		ScoreDetector.OnCollided += ObstaclePassed;
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
		ScoreDetector.OnCollided -= ObstaclePassed;
	}
	void GameOver(string lol)
	{
		gameOverSound.Play();
		StartCoroutine("StopMusic");
	}
	IEnumerator StopMusic()
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			mainMusic.volume = Mathf.Lerp(1, 0, t);
			yield return null;
		}
	}
	void ObstaclePassed()
	{
		obstaclePass.Play();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
