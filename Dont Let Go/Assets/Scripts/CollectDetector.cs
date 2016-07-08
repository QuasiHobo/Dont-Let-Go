using UnityEngine;
using System.Collections;

public class CollectDetector : MonoBehaviour {

	public delegate void OnCollectCollisionEvent();
	public static event OnCollectCollisionEvent OnCollect;

	public delegate void OnCollectBoostCollisionEvent();
	public static event OnCollectBoostCollisionEvent OnCollectBoost;

	public delegate void OnCollectBigCollisionEvent();
	public static event OnCollectBigCollisionEvent OnCollectBig;

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
			Destroy (collider.gameObject);
				if(GameManager.Instance.boostOngoing == false)
				OnCollectBoost();
		}
		if(collider.tag == "Collectable_Big")
		{
			Destroy (collider.gameObject);
			OnCollectBig ();
		}
	}
}
