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

	// Use this for initialization
	void Start () 
	{

	}

	void GameOver(string myChar)
	{
		this.gameObject.GetComponent<Animation> ().Stop ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
