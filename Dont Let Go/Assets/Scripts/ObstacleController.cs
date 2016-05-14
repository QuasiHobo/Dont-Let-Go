﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObstacleController : MonoBehaviour {

	public float moveSpeed = 10f;
	public Renderer rend;
	public List<Renderer> myRenderers = new List<Renderer>();
	public float duration = 5.0f;
	Color startColor = Color.white;
	Color endColor = new Color32(49,47,47,1);
	public float obstacleScore = 1f;

	public GameObject distanceObject;

	public bool gameOver = false;
	float t = 0f;

	// Use this for initialization 
	void Start () 
	{
		bool gameOver = false;
		CharCollison.OnCollision += GameOver;
		myRenderers.Add(this.gameObject.GetComponent<Renderer>());

		distanceObject = GameObject.FindGameObjectWithTag ("DistanceTarget");

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
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime, Space.World); 

			float distance = distanceObject.transform.position.y - this.transform.position.y;

			foreach (Renderer rend in myRenderers) 
			{
				rend.material.color = Color.Lerp (startColor, endColor, t);
			}
			if (t < 1) 
			{
				t += Time.deltaTime / Mathf.Clamp (distance, 1, 4);
			}
//			if (t < 1) {
//				t += Time.deltaTime / duration;
//			}
		}

		if (gameOver) 
		{
			foreach (Renderer rend in myRenderers) 
			{
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
}