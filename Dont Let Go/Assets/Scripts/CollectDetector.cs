using UnityEngine;
using System.Collections;

public class CollectDetector : MonoBehaviour {

	public delegate void OnCollectCollisionEvent();
	public static event OnCollectCollisionEvent OnCollect;

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
	}
}
