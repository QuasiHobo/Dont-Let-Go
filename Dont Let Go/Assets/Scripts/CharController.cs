using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine.UI;

public class CharController : MonoBehaviour {

	public string charNumb;
	FullBodyBipedIK FBBIK;

	GameObject legController;

	GameObject legPosDown;
	GameObject legPosUp;

	public float startBodyWeight = 0.2f;
	public float hugBodyWeight = 0.0f;
	public float hugDuration = 3.0f;

	public Button hugButton;
	public Renderer charRend;
	public GameObject distanceObject;

	float TransformDuration = 0.35f;

	bool doingHug = false;

	Color startColor;

	// Use this for initialization
	void Start () 
	{
		FallingEventScript.OnFallHit += CharCollided;
		RotatorManager.OnHug += StartHug;
		RotatorManager.OnStopHug += StopHug;

		startColor = charRend.material.color;

		hugButton.enabled = true;
		FBBIK = this.gameObject.GetComponent<FullBodyBipedIK> ();

		legController = this.transform.Find ("legRot").gameObject;
		legPosDown = this.transform.Find ("LegPosDown").gameObject;
		legPosUp = this.transform.Find ("LegPosUp").gameObject;

		FBBIK.solver.bodyEffector.positionWeight = startBodyWeight;
		legController.transform.position = legPosDown.transform.position;
	}

	void OnDisable()
	{
		FallingEventScript.OnFallHit -= CharCollided;
		RotatorManager.OnHug -= StartHug;
		RotatorManager.OnStopHug -= StopHug;
	}

	void StartHug()
	{
		doingHug = true;
		StartCoroutine ("Hug");
	}
	void StopHug ()
	{
		doingHug = false;
	}

	public IEnumerator Hug()
	{
		yield return new WaitForSeconds (0.15f);
		if (doingHug) {
			hugButton.enabled = false;
			float t = 0f;
			while (t < 1) {
				t += Time.deltaTime / TransformDuration;
				legController.transform.position = Vector3.Lerp (legPosDown.transform.position, legPosUp.transform.position, t);
				FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (startBodyWeight, hugBodyWeight, t);
				yield return null;
			}

			while (doingHug) {
				yield return null;
			}

			t = 0f;
			while (t < 1) {
				t += Time.deltaTime / TransformDuration;
				legController.transform.position = Vector3.Lerp (legPosUp.transform.position, legPosDown.transform.position, t);
				FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (hugBodyWeight, startBodyWeight, t);
				yield return null;
			}
			hugButton.enabled = true;
		}
	}

	void CharCollided(string charNumbPass)
	{
		if(charNumbPass == charNumb)
		StartCoroutine ("FadeCharColor");
	}
	IEnumerator FadeCharColor()
	{
		float t = 0;
		float duration = 2.5f;
		while (t < 1) 
		{
			t += Time.deltaTime / duration;
			charRend.material.color = Color.Lerp(startColor, Color.white, t);
			yield return null;
		}
		yield return null;
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
