using UnityEngine;
using System.Collections;

public class CharCollison : MonoBehaviour {

	//Events
	public delegate void OnCollisionEvent(string charNumb);
	public static event OnCollisionEvent OnCollision;

	// Use this for initialization
	void Start () 
	{

	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Obstacle") 
		{
			OnCollision (this.gameObject.tag); 
			collider.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			collider.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		}
		if (collider.tag == "SeperationObstacle") 
		{
			OnCollision (this.gameObject.tag); 
			collider.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			collider.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		}
	}
}
