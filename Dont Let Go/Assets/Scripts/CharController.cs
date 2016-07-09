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

	Camera mainCam;

	public float startBodyWeight = 0.2f;
	public float hugBodyWeight = 0.0f;
	public float hugDuration = 3.0f;

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
		CollectDetector.OnCollectBoost += BoostHug;

		startColor = charRend.material.color;
		FBBIK = this.gameObject.GetComponent<FullBodyBipedIK> ();

		legController = this.transform.Find ("legRot").gameObject;
		legPosDown = this.transform.Find ("LegPosDown").gameObject;
		legPosUp = this.transform.Find ("LegPosUp").gameObject;

		FBBIK.solver.bodyEffector.positionWeight = startBodyWeight;
		legController.transform.position = legPosDown.transform.position;

		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void OnDisable()
	{
		FallingEventScript.OnFallHit -= CharCollided;
		RotatorManager.OnHug -= StartHug;
		RotatorManager.OnStopHug -= StopHug;
		CollectDetector.OnCollectBoost -= BoostHug;
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
	void BoostHug()
	{
		StartCoroutine("StartBoostHug");
	}
	IEnumerator StartBoostHug()
	{
		float t = 0f;
		while (t < 1) {
			t += Time.deltaTime / 0.25f;
			legController.transform.localPosition = Vector3.Lerp (legController.transform.localPosition, legPosUp.transform.localPosition, t);
			FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (startBodyWeight, hugBodyWeight, t);
			yield return null;
		}

		while (GameManager.Instance.boostOngoing) {
			yield return null;
		}

		t = 0f;
		while (t < 1) {
			t += Time.deltaTime / 0.25f;
			legController.transform.localPosition = Vector3.Lerp (legPosUp.transform.localPosition, legPosDown.transform.localPosition, t);
			FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (hugBodyWeight, startBodyWeight, t);
			yield return null;
		}
		yield return null;
	}
	public IEnumerator Hug()
	{
		yield return new WaitForSeconds (0.15f);
		if (doingHug && GameManager.Instance.boostOngoing == false) {
			float t = 0f;
			while (t < 1) {
				t += Time.deltaTime / TransformDuration;
				legController.transform.localPosition = Vector3.Lerp (legPosDown.transform.localPosition, legPosUp.transform.localPosition, t);
				FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (startBodyWeight, hugBodyWeight, t);
				yield return null;
			}

			while (doingHug && GameManager.Instance.boostOngoing == false) {
				yield return null;
			}

			t = 0f;
			while (t < 1 && GameManager.Instance.boostOngoing == false) {
				t += Time.deltaTime / TransformDuration;
				legController.transform.localPosition = Vector3.Lerp (legPosUp.transform.localPosition, legPosDown.transform.localPosition, t);
				FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (hugBodyWeight, startBodyWeight, t);
				yield return null;
			}
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
		Color tempColor = mainCam.backgroundColor;
		while (t < 1) 
		{
			tempColor = mainCam.backgroundColor;
			t += Time.deltaTime / duration;
			charRend.material.color = Color.Lerp(startColor, tempColor, t);
			yield return null;
		}
		yield return null;
	}
}
