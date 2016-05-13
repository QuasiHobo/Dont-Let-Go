using UnityEngine;
using System.Collections;

public class CharCollison : MonoBehaviour {

	//Events
	public delegate void OnCollisionEvent(string charNumb);
	public static event OnCollisionEvent OnCollision;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Obstacle") 
		{
			OnCollision (this.gameObject.tag); 
			collider.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			collider.gameObject.GetComponent<Rigidbody> ().useGravity = true;
			Debug.Log ("RAMT!!!");
		}
	}
}
