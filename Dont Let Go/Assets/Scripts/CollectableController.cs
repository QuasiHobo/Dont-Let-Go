using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableController : MonoBehaviour {

	float moveSpeed;
	public Renderer rend;

	Color startColor = Color.white;
	public Color endColor = new Color32(193,60,60,1);

	Camera mainCam;
	AudioSource myAudio;

	public GameObject distanceObject;
	float startDistance;
	float duration = 5.0f;
	float t = 0f;
	bool gameOver = false;

	GameObject targetObj_1;
	GameObject targetObj_2;
	public float minDistChar = 1f;
	public float suckSpeed = 5.5f;

	// Use this for initialization
	void Start () 
	{
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		startColor = mainCam.backgroundColor;
		this.gameObject.GetComponent<Renderer>().material.color = mainCam.backgroundColor;

		myAudio = gameObject.AddComponent<AudioSource>();
		myAudio.clip = Resources.Load("Sounds & Music/Sounds/WaterDrop") as AudioClip;
			
		gameOver = false;
		CharCollison.OnCollision += GameOver;

		moveSpeed = GameManager.Instance.gameSpeed;
		distanceObject = GameObject.FindGameObjectWithTag ("DistanceTarget");
		startDistance = this.transform.position.y - distanceObject.transform.position.y;

		targetObj_1 = GameObject.FindGameObjectWithTag ("legRot_char1");
		targetObj_2 = GameObject.FindGameObjectWithTag ("legRot_char2");

	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
	}
	void GameOver(string lol)
	{
		gameOver = true;
		Destroy (this.gameObject);
	}

	public void WasCollected()
	{
		myAudio.Play();
		StartCoroutine("WaitToDie");
	}
	IEnumerator WaitToDie()
	{
		yield return new WaitForSeconds (1.5f);
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver) 
		{
			// Moving the object
			transform.Translate (Vector3.up * GameManager.Instance.gameSpeed * Time.deltaTime, Space.World); 

			float distance = this.transform.position.y - distanceObject.transform.position.y;

			// Changing its color
			rend.material.color = Color.Lerp (startColor, endColor, t);

			if (t < 1) 
			{
				t = 1- (distance / startDistance);

			}

			if(GameManager.Instance.boostOngoing)
			{
				suckSpeed = 45f;
				minDistChar = 2.5f;
			}

			//Distance to chars
			float char1Distance = Vector3.Distance(this.transform.position, targetObj_1.transform.position);
			float char2Distance = Vector3.Distance (this.transform.position, targetObj_2.transform.position);

			if (char1Distance <= minDistChar) 
			{
				moveSpeed = 0;
				float ta = suckSpeed* Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, targetObj_1.transform.position, ta);
				transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0f,0f,0f), ta);
			}
			if (char2Distance <= minDistChar) 
			{
				moveSpeed = 0;
				float tb = suckSpeed* Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, targetObj_2.transform.position, tb);
				transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0f,0f,0f), tb);
			}


		}
	}
}
