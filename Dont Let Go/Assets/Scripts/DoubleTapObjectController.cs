using UnityEngine;
using System.Collections;

public class DoubleTapObjectController : MonoBehaviour {

	public float moveSpeed = 10f;
	bool collided = false;

	// Use this for initialization
	void Start () 
	{
		bool collided = false;
		StartCoroutine ("MoveObject");
	}
	
	// Update is called once per frame
	IEnumerator MoveObject () 
	{
		while (true) 
		{
			transform.Translate (Vector3.down * moveSpeed * Time.deltaTime, Space.World);
			yield return null;
		}
	}
	void OnTriggerEnter(Collider colider)
	{
		if (colider.gameObject.tag == "Obstacle") 
		{
			Destroy (this.gameObject);
		}
	}
}
