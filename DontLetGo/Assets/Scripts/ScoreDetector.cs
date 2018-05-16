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
	float collectableBigValue = 25;

	public delegate void OnCollidedEvent();
	public static event OnCollidedEvent OnCollided;

	// Use this for initialization
	void Start () 
	{
		CollectDetector.OnCollect += Collected;
		CollectDetector.OnCollectBoost += BoostCollected;
		CollectDetector.OnCollectBig += BigCollected;

		totalScore = 0f;
		scoreText.text = "" + totalScore;
	}

	void OnDisable()
	{
		CollectDetector.OnCollect -= Collected;
		CollectDetector.OnCollectBoost -= BoostCollected;
		CollectDetector.OnCollectBig -= BigCollected;
	}
	
	void Collected()
	{
//		totalScore += collectableValue*GameManager.Instance.currentLevel;
		totalScore += collectableValue;
		scoreText.text = "" + totalScore;
	}
	void BoostCollected()
	{
		StartCoroutine("BoostScore");
	}
	void BigCollected()
	{
//		totalScore += collectableBigValue*GameManager.Instance.currentLevel;
		totalScore += collectableBigValue;
		scoreText.text = "" + totalScore;
	}
	IEnumerator BoostScore()
	{
		while(GameManager.Instance.boostOngoing)
		{
//			totalScore += 1*GameManager.Instance.currentLevel;
			totalScore += 1;
			scoreText.text = "" + totalScore;
			yield return new WaitForSeconds(0.05f);
			yield return null;
		}
	}
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Obstacle") 
		{
			if (collider.gameObject.GetComponent<ObstacleController> () != null) 
			{
				OnCollided();
//				totalScore += collider.gameObject.GetComponent<ObstacleController> ().obstacleScore*GameManager.Instance.currentLevel;
				totalScore += collider.gameObject.GetComponent<ObstacleController> ().obstacleScore;
				scoreText.gameObject.GetComponent<Animation> ().Play ();
				scoreText.text = "" + totalScore;

				StartCoroutine ("DestroyObstacle", collider.gameObject);
			}
		}

		if (collider.gameObject.tag == "Collectable") 
		{
			StartCoroutine ("DestroyObstacle", collider.gameObject);
		}
		if (collider.gameObject.tag == "Collectable_Boost") 
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
