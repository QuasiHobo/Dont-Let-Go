using UnityEngine;
using System.Collections;

public class ObstacleDetector : MonoBehaviour {

	public delegate void OnSeperateEvent();
	public static event OnSeperateEvent OnSeperate;

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "SeperationObstacle" && collider.gameObject.GetComponent<ObstacleController>().firstSpawn == true) 
		{
			OnSeperate ();
		}
	}
}
