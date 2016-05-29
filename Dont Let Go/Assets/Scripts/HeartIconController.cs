using UnityEngine;
using System.Collections;

public class HeartIconController : MonoBehaviour {

	void Awake()
	{
		CharCollison.OnCollision += GameOver;
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
	}

	void GameOver(string myChar)
	{
		this.gameObject.GetComponent<Animation> ().Stop ();
	}

}
