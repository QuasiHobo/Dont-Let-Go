using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class RagdollManager : MonoBehaviour {

	public GameObject FloorOBJ;
	public ParticleSystem speedFake;

	public List<GameObject> listOfRb = new List<GameObject>();
	public FullBodyBipedIK FBBIK;
	public Animator myAnim;

	public Rigidbody hips;

	//Events
	public delegate void OnRagdollEvent();
	public static event OnRagdollEvent OnRagdoll;


	void Start () 
	{
		CharCollison.OnCollision += EnableRagDolls;

		ParticleSystem.EmissionModule em = speedFake.emission;
		em.enabled = true;

		if (this.gameObject.tag == "Char1") 
		{
			listOfRb.AddRange (GameObject.FindGameObjectsWithTag ("Char1_fys"));
			foreach (GameObject go in listOfRb) {
				go.GetComponent<Rigidbody> ().isKinematic = true;
				go.GetComponent<Rigidbody> ().useGravity = false;
				go.GetComponent<Collider> ().isTrigger = true;
			}
		}
		if (this.gameObject.tag == "Char2") 
		{
			listOfRb.AddRange (GameObject.FindGameObjectsWithTag ("Char2_fys"));
			foreach (GameObject go in listOfRb) {
				go.GetComponent<Rigidbody> ().isKinematic = true;
				go.GetComponent<Rigidbody> ().useGravity = false;
				go.GetComponent<Collider> ().isTrigger = true;
			}
		}
	}

	void OnDisable()
	{
		CharCollison.OnCollision -= EnableRagDolls;
	}

	void EnableRagDolls(string charNumb)
	{
		FBBIK.enabled = false;
		myAnim.enabled = false;

		ParticleSystem.EmissionModule em = speedFake.emission;
		em.enabled = false;
		if (this.gameObject.tag == "Char1") 
		{
			listOfRb.AddRange (GameObject.FindGameObjectsWithTag ("Char1_fys"));

			foreach (GameObject go in listOfRb) {
				go.GetComponent<Rigidbody> ().isKinematic = false;
				go.GetComponent<Rigidbody> ().useGravity = true;
				go.GetComponent<Collider> ().isTrigger = false;
				go.GetComponent<Collider> ().enabled = true;

				float colStrength = 110f;
				if (charNumb == "Char1_fys")
					go.GetComponent<Rigidbody> ().AddForce (transform.up * colStrength);
			}
			if(charNumb == "lol")
				hips.GetComponent<Rigidbody> ().AddForce (transform.forward * -3333);
		}
		if (this.gameObject.tag == "Char2") 
		{
			listOfRb.AddRange (GameObject.FindGameObjectsWithTag ("Char2_fys"));

			foreach (GameObject go in listOfRb) {
				go.GetComponent<Rigidbody> ().isKinematic = false;
				go.GetComponent<Rigidbody> ().useGravity = true;
				go.GetComponent<Collider> ().isTrigger = false;
				go.GetComponent<Collider> ().enabled = true;

				float colStrength = 110f; 
				if (charNumb == "Char2_fys")
					go.GetComponent<Rigidbody> ().AddForce (transform.up * colStrength);
			}
			if(charNumb == "lol")
				hips.GetComponent<Rigidbody> ().AddForce (transform.forward * -3333);
		}
			
	}
		
}
