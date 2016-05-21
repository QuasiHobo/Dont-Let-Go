using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDetector : MonoBehaviour {

	public Text scoreText;
	float totalScore;

	// Use this for initialization
	void Start () 
	{
		totalScore = 0f;
		scoreText.text = "" + totalScore;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Obstacle") 
		{
			if (collider.gameObject.GetComponent<ObstacleController> () != null) 
			{
				totalScore += collider.gameObject.GetComponent<ObstacleController> ().obstacleScore;
				scoreText.gameObject.GetComponent<Animation> ().Play ();
				scoreText.text = "" + totalScore;
//				Debug.Log ("SCORE!!!");

				StartCoroutine ("DestroyObstacle", collider.gameObject);
			}
		}
	}

	IEnumerator DestroyObstacle(GameObject obstacle)
	{
		yield return new WaitForSeconds (2f);
		Destroy(obstacle.gameObject);
	}
}
