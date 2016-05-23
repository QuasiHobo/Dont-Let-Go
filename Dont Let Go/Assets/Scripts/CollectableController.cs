using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableController : MonoBehaviour {

	float moveSpeed;
	public Renderer rend;

	Color startColor = Color.white;
	public Color endColor = new Color32(193,60,60,1);

	public GameObject distanceObject;
	float startDistance;
	public float duration = 5.0f;
	float t = 0f;
	bool gameOver = false;

	// Use this for initialization
	void Start () 
	{
		gameOver = false;
		CharCollison.OnCollision += GameOver;

		moveSpeed = GameManager.Instance.gameSpeed;
		distanceObject = GameObject.FindGameObjectWithTag ("DistanceTarget");
		startDistance = this.transform.position.y - distanceObject.transform.position.y; 
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= GameOver;
	}
	void GameOver(string lol)
	{
		gameOver = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver) 
		{
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime, Space.World); 

			float distance = this.transform.position.y - distanceObject.transform.position.y;

			rend.material.color = Color.Lerp (startColor, endColor, t);

			if (t < 1) 
			{
				t = 1- (distance / startDistance);

			}

		}

		if (gameOver) 
		{
			rend.gameObject.GetComponent<Collider> ().enabled = false;
			rend.material.color = Color.Lerp (rend.material.color, startColor, t);

			if (t < 1) 
			{
				t += Time.deltaTime / duration;
			}
			if (t >= 1) 
			{
				Destroy (this.gameObject);
			}
		}
	}
}
