using UnityEngine;
using System.Collections;

public class FloorCollison : MonoBehaviour {

	public ParticleSystem bloodSplatter;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Floor") 
		{
			ContactPoint contact = collision.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (bloodSplatter, pos, rot);
		}
	}
}
