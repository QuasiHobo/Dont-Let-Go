﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObstacleController : MonoBehaviour {

	float moveSpeed;
	public Renderer rend;
	public List<Renderer> myRenderers = new List<Renderer>();
	float duration = 5.0f;
	Color startColor = Color.white;
	Color endColor = new Color32(49,47,47,1);
	public float obstacleScore = 1f;

	public GameObject distanceObject;
	float startDistance;

	public bool gameOver = false;
	float t = 0f;

	public bool lastSpawn = false;
	public bool firstSpawn = false;

	// Use this for initialization 
	void Start () 
	{
		gameOver = false;
		CharCollison.OnCollision += GameOver;
		myRenderers.Add(this.gameObject.GetComponent<Renderer>());
		moveSpeed = GameManager.Instance.gameSpeed;
		distanceObject = GameObject.FindGameObjectWithTag ("DistanceTarget");
		startDistance = this.transform.position.y - distanceObject.transform.position.y; 
		foreach (Transform child in transform) 
		{
			GameObject obj = child.gameObject;
			myRenderers.Add (obj.GetComponent<Renderer> ());
		}
		foreach (Renderer rend in myRenderers) 
		{
			rend.material.color = Color.white;
		}
	}

	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver) 
		{
			transform.Translate (Vector3.up * GameManager.Instance.gameSpeed * Time.deltaTime, Space.World); 

			float distance = this.transform.position.y - distanceObject.transform.position.y;

			foreach (Renderer rend in myRenderers) 
			{
				rend.material.color = Color.Lerp (startColor, endColor, t);
			}

			if (t < 1) 
			{
				t = 1- (distance / startDistance);
				 
			}

		}

		if (gameOver) 
		{
			foreach (Renderer rend in myRenderers) 
			{
				rend.gameObject.GetComponent<Collider> ().enabled = false;
				rend.material.color = Color.Lerp (rend.material.color, startColor, t);
			}
			if (t < 1) {
				t += Time.deltaTime / duration;
			}
		}
	}

	void GameOver(string lol)
	{
		t = 0f;
		moveSpeed = 0f;
		gameOver = true;

		StartCoroutine ("DestroyMe");
	}

	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds (1.5f);
		Destroy (this.gameObject);
	}
	IEnumerator DoubleTapDestroyed()
	{
		t = 0;
		gameOver = true;
		Destroy (this.gameObject);
		yield return null;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "DoubleTapObject") 
		{
			StartCoroutine ("DoubleTapDestroyed");
		}
	}
}
