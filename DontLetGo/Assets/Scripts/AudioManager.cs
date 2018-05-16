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
	public AudioSource specialSpawnSound;

	float vanilaPitch = 1.0f;
	float boostPitchStart = 0.75f;
	float boostPitch = 1.1f;

	// Use this for initialization
	void Start () 
	{
		//Listening to events
		CharCollison.OnCollision += GameOver;
		CollectDetector.OnCollectBoost += BoostCollected;
		GameManager.OnBoostEnds += BoostEnds;
		ObstacleManager.OnSpecialStart += SpecialLevelStarted;
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
		CollectDetector.OnCollectBoost -= BoostCollected;
		GameManager.OnBoostEnds -= BoostEnds;
		ObstacleManager.OnSpecialStart -= SpecialLevelStarted;
	}
	void SpecialLevelStarted()
	{
		specialSpawnSound.Play();
	}
	void BoostCollected()
	{
		StartCoroutine("BoostMusic");
	}
	void BoostEnds()
	{
		StartCoroutine("BoostEnded");
	}
	IEnumerator BoostMusic()
	{
		float t = 0f;
		while (t < 1)
		{
			t += Time.deltaTime;
			mainMusic.pitch = Mathf.Lerp(vanilaPitch, boostPitchStart, t);
			yield return null;
		}
		t = 0f;
		yield return new WaitForSeconds(0.15f);
		while (t < 1)
		{
			t += Time.deltaTime;
			mainMusic.pitch = Mathf.Lerp(boostPitchStart, vanilaPitch, t);
			yield return null;
		}

		yield return null;
	}
	IEnumerator BoostEnded()
	{
		yield return null;
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

}
