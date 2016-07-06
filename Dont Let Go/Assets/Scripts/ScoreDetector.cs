using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDetector : MonoBehaviour {

	// Singleton
	private static ScoreDetector instance;
	// Construct
	private ScoreDetector ()
	{
	}
	// Instance
	public static ScoreDetector Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(ScoreDetector)) as ScoreDetector;

			return instance;
		}
	}

	public Text scoreText;
	public float totalScore;

	float collectableValue = 3;

	public delegate void OnSeperateStopEvent();
	public static event OnSeperateStopEvent OnSeperateStop;

	// Use this for initialization
	void Start () 
	{
		CollectDetector.OnCollect += Collected;
		CollectDetector.OnCollectBoost += BoostCollected;

		totalScore = 0f;
		scoreText.text = "" + totalScore;
	}

	void OnDisable()
	{
		CollectDetector.OnCollect -= Collected;
	}
	
	void Collected()
	{
		totalScore += collectableValue;
		scoreText.text = "" + totalScore;
	}
	void BoostCollected()
	{
		StartCoroutine("BoostScore");
	}
	IEnumerator BoostScore()
	{
		while(GameManager.Instance.boostOngoing)
		{
			totalScore += 2;
			scoreText.text = "" + totalScore;
			yield return new WaitForSeconds(0.05f);
		}
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

				StartCoroutine ("DestroyObstacle", collider.gameObject);
			}
		}
		if (collider.gameObject.tag == "SeperationObstacle") 
		{
			if (collider.gameObject.GetComponent<ObstacleController> () != null) 
			{
				totalScore += collider.gameObject.GetComponent<ObstacleController> ().obstacleScore;
				scoreText.gameObject.GetComponent<Animation> ().Play ();
				scoreText.text = "" + totalScore;

				if (collider.gameObject.GetComponent<ObstacleController> ().lastSpawn == true)
					OnSeperateStop ();

				Destroy (collider.gameObject);
			}
		}
		if (collider.gameObject.tag == "Collectable") 
		{
			StartCoroutine ("DestroyObstacle", collider.gameObject);
		}
	}

	IEnumerator DestroyObstacle(GameObject obstacle)
	{
		yield return new WaitForSeconds (2f);
		Destroy(obstacle.gameObject);
	}
}
