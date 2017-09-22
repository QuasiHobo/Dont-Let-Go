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

	// Use this for initialization
	void Start () 
	{
		//Listening to events
		CharCollison.OnCollision += GameOver;
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
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
	// Update is called once per frame
	void Update () {
		
	}
}
