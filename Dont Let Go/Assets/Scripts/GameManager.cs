using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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
	IEnumerator GameOverState()
	{
		yield return new WaitForSeconds (6f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		yield return null;
	}

	void Update () 
	{
	
	}
}
