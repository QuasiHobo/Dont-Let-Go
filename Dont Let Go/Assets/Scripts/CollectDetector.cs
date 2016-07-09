using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectDetector : MonoBehaviour {

	public delegate void OnCollectCollisionEvent();
	public static event OnCollectCollisionEvent OnCollect;

	public delegate void OnCollectBoostCollisionEvent();
	public static event OnCollectBoostCollisionEvent OnCollectBoost;

	public delegate void OnCollectBigCollisionEvent();
	public static event OnCollectBigCollisionEvent OnCollectBig;

	GameObject collectEffect_big;
	GameObject collectEffect_boost;

	// Use this for initialization
	void Start () 
	{
		collectEffect_big = Resources.Load("Prefabs/Effects/Effect_Collectable_1") as GameObject;
		collectEffect_boost = Resources.Load("Prefabs/Effects/Effect_Collectable_2") as GameObject;
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
//			GameObject tempEffect = Instantiate(collectEffect_boost, this.gameObject.transform.position, collectEffect_boost.transform.rotation) as GameObject;
//			tempEffect.transform.parent = this.gameObject.transform;

				if(GameManager.Instance.boostOngoing == false)
				OnCollectBoost();
		}
		if(collider.tag == "Collectable_Big")
		{
			Destroy (collider.gameObject);
//			GameObject tempEffect = Instantiate(collectEffect_big, this.gameObject.transform.position, collectEffect_big.transform.rotation) as GameObject;
//			tempEffect.transform.parent = this.gameObject.transform;
			OnCollectBig ();
		}
	}
}
