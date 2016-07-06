using UnityEngine;
using System.Collections;

public class CollectDetector : MonoBehaviour {

	public delegate void OnCollectCollisionEvent();
	public static event OnCollectCollisionEvent OnCollect;

	public delegate void OnCollectBoostCollisionEvent();
	public static event OnCollectBoostCollisionEvent OnCollectBoost;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Collectable") 
		{
			Destroy (collider.gameObject);
			OnCollect ();
		}
		if(collider.tag == "Collectable_Boost")
		{
			Debug.Log("Boost collected!");
			Destroy (collider.gameObject);
			OnCollectBoost();
		}
	}
}
